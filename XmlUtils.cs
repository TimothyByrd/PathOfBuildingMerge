using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace PathOfBuildingMerge
{

    //
    // The code in this file is from https://stackoverflow.com/questions/7318157/best-way-to-compare-xelement-objects
    //

    public static class MyExtensions
    {
        public static string ToStringAlignAttributes(this XDocument document)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true,
                OmitXmlDeclaration = true,
                NewLineOnAttributes = true
            };
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
                document.WriteTo(xmlWriter);
            return stringBuilder.ToString();
        }
    }

    internal class XmlUtils
    {
        public static void SaveXDocumentWithoutBom(XDocument document, string filePath)
        {
            // Create a UTF8Encoding instance, specifying 'false' for the 'emitBOM' parameter.
            var utf8WithoutBom = new UTF8Encoding(false);

            // Create a StreamWriter with the specified encoding.
            using var writer = new StreamWriter(filePath, false, utf8WithoutBom);

            // Save the XDocument to the StreamWriter.
            document.Save(writer);
        }

        public static bool DeepEqualsWithNormalization(XDocument doc1, XDocument doc2,
            XmlSchemaSet schemaSet)
        {
            XDocument d1 = Normalize(doc1, schemaSet);
            XDocument d2 = Normalize(doc2, schemaSet);
            return XNode.DeepEquals(d1, d2);
        }

        private static class Xsi
        {
            public static XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

            public static XName schemaLocation = xsi + "schemaLocation";
            public static XName noNamespaceSchemaLocation = xsi + "noNamespaceSchemaLocation";
        }

        public static XDocument Normalize(XDocument source, XmlSchemaSet schema)
        {
            bool havePSVI = false;
            // validate, throw errors, add PSVI information
            if (schema != null)
            {
                source.Validate(schema, null, true);
                havePSVI = true;
            }
            return new XDocument(
                source.Declaration,
                source.Nodes().Select(n =>
                {
                    // Remove comments, processing instructions, and text nodes that are
                    // children of XDocument.  Only white space text nodes are allowed as
                    // children of a document, so we can remove all text nodes.
                    if (n is XComment || n is XProcessingInstruction || n is XText)
                        return null;
                    if (n is XElement e)
                        return NormalizeElement(e, havePSVI);
                    return n;
                }
                )
            );
        }

        private static IEnumerable<XAttribute> NormalizeAttributes(XElement element,
            bool havePSVI)
        {
            return element.Attributes()
                    .Where(a => !a.IsNamespaceDeclaration &&
                        a.Name != Xsi.schemaLocation &&
                        a.Name != Xsi.noNamespaceSchemaLocation)
                    .OrderBy(a => a.Name.NamespaceName)
                    .ThenBy(a => a.Name.LocalName)
                    .Select(
                        a =>
                        {
                            if (havePSVI)
                            {
                                var dt = a.GetSchemaInfo()?.SchemaType?.TypeCode ?? XmlTypeCode.String;
                                switch (dt)
                                {
                                    case XmlTypeCode.Boolean:
                                        return new XAttribute(a.Name, (bool)a);
                                    case XmlTypeCode.DateTime:
                                        return new XAttribute(a.Name, (DateTime)a);
                                    case XmlTypeCode.Decimal:
                                        return new XAttribute(a.Name, (decimal)a);
                                    case XmlTypeCode.Double:
                                        return new XAttribute(a.Name, (double)a);
                                    case XmlTypeCode.Float:
                                        return new XAttribute(a.Name, (float)a);
                                    case XmlTypeCode.HexBinary:
                                    case XmlTypeCode.Language:
                                        return new XAttribute(a.Name,
                                            ((string)a).ToLower());
                                }
                            }
                            return a;
                        }
                    );
        }

        private static XNode? NormalizeNode(XNode node, bool havePSVI)
        {
            // trim comments and processing instructions from normalized tree
            if (node is XComment || node is XProcessingInstruction)
                return null;
            if (node is XElement e)
                return NormalizeElement(e, havePSVI);
            // Only thing left is XCData and XText, so clone them
            return node;
        }

        public static XElement NormalizeElement(XElement element, bool havePSVI)
        {
            if (havePSVI)
            {
                var dt = element.GetSchemaInfo();
                return (dt?.SchemaType?.TypeCode ?? XmlTypeCode.String) switch
                {
                    XmlTypeCode.Boolean => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                (bool)element),
                    XmlTypeCode.DateTime => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                (DateTime)element),
                    XmlTypeCode.Decimal => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                (decimal)element),
                    XmlTypeCode.Double => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                (double)element),
                    XmlTypeCode.Float => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                (float)element),
                    XmlTypeCode.HexBinary or XmlTypeCode.Language => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                ((string)element).ToLower()),
                    _ => new XElement(element.Name,
                                                NormalizeAttributes(element, havePSVI),
                                                element.Nodes().Select(n => NormalizeNode(n, havePSVI))
                                            ),
                };
            }
            else
            {
                return new XElement(element.Name,
                    NormalizeAttributes(element, havePSVI),
                    element.Nodes().Select(n => NormalizeNode(n, havePSVI))
                );
            }
        }
    }
}
