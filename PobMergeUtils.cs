using System.Xml.Linq;

namespace PathOfBuildingMerge
{
    internal class PobMergeUtils
    {
        internal struct SetType
        {
            public string Node;
            public string Element;
            public string Active;
        }

        static SetType Items = new() { Node = "Items", Element = "ItemSet", Active = "activeItemSet" };
        static SetType Skills = new() { Node = "Skills", Element = "SkillSet", Active = "activeSkillSet" };
        static SetType Config = new() { Node = "Config", Element = "ConfigSet", Active = "activeConfigSet" };
        static SetType Tree = new() { Node = "Tree", Element = "Spec", Active = "activeSpec" };


        internal static void Merge(string mainPob, string pobToAdd, string newLoadoutName, string pobResult, bool onlyAddUsedItems = true, bool reuseExistingItems = true)
        {
            var baseFileName = Path.GetFileName(mainPob);
            var baseDoc = XDocument.Load(mainPob);

            var baseTreeNode = GetRootNode(baseDoc, baseFileName, Tree.Node);
            var baseSkillsNode = GetRootNode(baseDoc, baseFileName, Skills.Node);
            var baseItemsNode = GetRootNode(baseDoc, baseFileName, Items.Node);
            var baseConfigNode = GetRootNode(baseDoc, baseFileName, Config.Node);

            RemoveTreeSpec(baseTreeNode, newLoadoutName);
            RemoveSkillSet(baseSkillsNode, newLoadoutName);
            RemoveItemSet(baseItemsNode, newLoadoutName);
            RemoveConfigSet(baseConfigNode, newLoadoutName);

            var addFileName = Path.GetFileName(pobToAdd);
            var docToAdd = XDocument.Load(pobToAdd);
            var addItemsNode = GetRootNode(docToAdd, addFileName, Items.Node);
            var itemsToAdd = addItemsNode.Elements("Item");

            List<int> usedItemIds = [];
            if (onlyAddUsedItems)
            {
                CollectUsedItemIds(usedItemIds, GetActiveTreeSpec(docToAdd, addFileName));
                CollectUsedItemIds(usedItemIds, GetActiveSkillSet(docToAdd, addFileName));
                CollectUsedItemIds(usedItemIds, GetActiveItemSet(docToAdd, addFileName));
                CollectUsedItemIds(usedItemIds, GetActiveConfigSet(docToAdd, addFileName));
            }

            var addedItemsIdDictionary = new Dictionary<int, int>();

            var normalizedBaseItems = baseItemsNode.Elements("Item").Select(x => XmlUtils.NormalizeElement(x, false));
            var normalizedItemsDictionary = new Dictionary<int, XElement>();
            foreach (var item in normalizedBaseItems)
            {
                var id = (int?)item.Attribute("id") ?? 0;
                if (id <= 0) continue;
                item.SetAttributeValue("id", null);
                normalizedItemsDictionary[id] = item;
            }

            var nextItemIdToUse = 1;
            foreach (var item in itemsToAdd)
            {
                if (item == null) continue;
                var id = (int?)item.Attribute("id") ?? 0;
                if (id <= 0) continue;

                if (onlyAddUsedItems && !usedItemIds.Contains(id))
                    continue;

                var normalizedItem = XmlUtils.NormalizeElement(item, false);
                normalizedItem.SetAttributeValue("id", null);

                // check if item is a duplicate and if so continue
                //
                if (reuseExistingItems)
                {
                    var foundItem = normalizedItemsDictionary.FirstOrDefault(x => (XNode.DeepEquals(x.Value, normalizedItem)));
                    if (foundItem.Value != null)
                    {
                        addedItemsIdDictionary[id] = foundItem.Key;
                        continue;
                    }
                }

                // renumber ids of added items so they don't clash with existing items.
                //
                var newId = id;
                if (normalizedItemsDictionary.ContainsKey(id))
                {
                    while (normalizedItemsDictionary.ContainsKey(nextItemIdToUse)) ++nextItemIdToUse;
                    newId = nextItemIdToUse;
                }

                addedItemsIdDictionary[id] = newId;
                normalizedItemsDictionary[newId] = normalizedItem;

                item.SetAttributeValue("id", newId.ToString());
                baseItemsNode.Add(item);
            }

            AddTreeSpec(docToAdd, addFileName, baseTreeNode, newLoadoutName, addedItemsIdDictionary);
            AddSkillSet(docToAdd, addFileName, baseSkillsNode, newLoadoutName, addedItemsIdDictionary);
            AddItemSet(docToAdd, addFileName, baseItemsNode, newLoadoutName, addedItemsIdDictionary);
            AddConfigSet(docToAdd, addFileName, baseConfigNode, newLoadoutName, addedItemsIdDictionary);

            XmlUtils.SaveXDocumentWithoutBom(baseDoc, pobResult);
        }

        private static void CollectUsedItemIds(List<int> usedItemIds, XElement node)
        {
            foreach (var child in node.Descendants())
            {
                if (!child.HasAttributes) continue;
                var itemId = (int?)child.Attribute("itemId") ?? 0;
                if (itemId > 0)
                    usedItemIds.Add(itemId);
            }
        }

        private static void UpdateItemIds(XElement node, Dictionary<int, int> idDictionary)
        {
            foreach (var child in node.Descendants())
            {
                if (!child.HasAttributes) continue;
                var itemId = (int?)child.Attribute("itemId") ?? 0;
                if (itemId > 0)
                    child.SetAttributeValue("itemId", idDictionary[itemId]);
            }
        }

        private static void AddTreeSpec(XDocument doc, string fileName, XElement destinationNode, string newLoadoutName, Dictionary<int, int> idDictionary)
        {
            XElement found = GetActiveTreeSpec(doc, fileName);
            found.SetAttributeValue("title", newLoadoutName);
            UpdateItemIds(found, idDictionary);
            destinationNode.Add(found);
        }

        private static XElement GetActiveTreeSpec(XDocument doc, string fileName)
        {
            var treeNode = GetRootNode(doc, fileName, Tree.Node);
            var activeSpec = (int?)treeNode.Attribute(Tree.Active) ?? 1;

            var specs = treeNode.Elements(Tree.Element);

            if (activeSpec > 1 && specs.Count() >= activeSpec) specs = specs.Skip(activeSpec - 1);
            var found = specs.FirstOrDefault() ?? throw new Exception($"No tree spec found in '{fileName}' to add");
            return found;
        }

        private static XElement GetActiveSkillSet(XDocument doc, string fileName)
        {
            return GetChildByActiveIndex(doc, fileName, Skills);
        }

        private static XElement GetActiveItemSet(XDocument doc, string fileName)
        {
            return GetChildByActiveIndex(doc, fileName, Items);
        }

        private static XElement GetActiveConfigSet(XDocument doc, string fileName)
        {
            return GetChildByActiveIndex(doc, fileName, Config);
        }

        private static void AddSkillSet(XDocument doc, string fileName, XElement destinationNode, string newLoadoutName, Dictionary<int, int> idDictionary)
        {
            AddNodeWithUniqueId(doc, fileName, destinationNode, newLoadoutName, idDictionary, Skills);
        }

        private static void AddItemSet(XDocument doc, string fileName, XElement destinationNode, string newLoadoutName, Dictionary<int, int> idDictionary)
        {
            AddNodeWithUniqueId(doc, fileName, destinationNode, newLoadoutName, idDictionary, Items);
        }

        private static void AddConfigSet(XDocument doc, string fileName, XElement destinationNode, string newLoadoutName, Dictionary<int, int> idDictionary)
        {
            AddNodeWithUniqueId(doc, fileName, destinationNode, newLoadoutName, idDictionary, Config);
        }

        private static void AddNodeWithUniqueId(XDocument doc, string fileName, XElement destinationNode, string newLoadoutName, Dictionary<int, int> idDictionary, SetType setType)
        {
            var result = GetChildByActiveIndex(doc, fileName, setType);
            UpdateItemIds(result, idDictionary);
            result.SetAttributeValue("title", newLoadoutName);
            var newId = GetFirstAvailableId(destinationNode, setType.Element);
            result.SetAttributeValue("id", newId);
            destinationNode.Add(result);
        }

        private static string GetFirstAvailableId(XElement node, string elementName)
        {
            var nodes = node.Elements(elementName);
            var usedIds = nodes.Select(x => (int?)x.Attribute("id") ?? 0).Where(x => x > 0).ToList();
            var newId = 1;
            while (usedIds.Contains(newId)) ++newId;
            return newId.ToString();
        }

        private static XElement GetChildByActiveIndex(XDocument doc, string fileName, SetType setType)
        {
            var node = GetRootNode(doc, fileName, setType.Node);
            var activeId = (string?)node.Attribute(setType.Active);
            var found = node.Elements(setType.Element).Where(e => String.Equals((string?)e.Attribute("id"), activeId)).FirstOrDefault();
            found ??= node.Elements(setType.Element).FirstOrDefault();
            return found ?? throw new Exception($"Could not find a '{setType.Element}' in '{fileName}'");
        }

        private static void RemoveChildByTitle(XElement parent, string nameToRemove, SetType setType)
        {
            var found = parent.Elements(setType.Element).Where(e => String.Equals((string?)e.Attribute("title"), nameToRemove));
            if (found.Any())
            {
                found.Remove();
                var newActive = (string?)parent.Elements(setType.Element).FirstOrDefault()?.Attribute("id");
                parent.SetAttributeValue(setType.Active, newActive);
            }
        }

        private static void RemoveItemSet(XElement itemsNode, string nameToRemove)
        {
            RemoveChildByTitle(itemsNode, nameToRemove, Items);
        }

        private static void RemoveConfigSet(XElement configNode, string nameToRemove)
        {
            RemoveChildByTitle(configNode, nameToRemove, Config);
        }

        private static void RemoveSkillSet(XElement skillsNode, string nameToRemove)
        {
            RemoveChildByTitle(skillsNode, nameToRemove, Skills);
        }

        private static void RemoveTreeSpec(XElement treeNode, string nameToRemove)
        {
            var found = treeNode.Elements(Tree.Element).Where(e => String.Equals((string?)e.Attribute("title"), nameToRemove));
            if (found.Any())
            {
                found.Remove();
                treeNode.SetAttributeValue(Tree.Active, "1");
            }
        }

        private static XElement GetRootNode(XDocument doc, string fileName, string nodeName)
        {
            var node = doc.Root?.Element(nodeName);
            return node ?? throw new Exception($"Docuemnt '{fileName}' does not contain node '{nodeName}'");
        }
    }
}