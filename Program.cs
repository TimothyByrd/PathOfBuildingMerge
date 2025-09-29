namespace PathOfBuildingMerge
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

            //var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var pobPath = Path.Combine(documentsPath, "Path of Building\\Builds");
            //var savePath = Path.Combine(pobPath, "3.26 Secrets");
            //var mainPob = Path.Combine(savePath, "Sanavixx ignite arc.xml");
            //var pobToAdd = Path.Combine(savePath, "SSFHCMaerNotApprovedBuild.xml");
            //var newLoadoutName = "MaerNotApprovedBuild";
            //var pobResult = Path.Combine(savePath, "result.xml");
            
            //mainPob = Path.Combine(savePath, "result.xml");
            //pobResult = Path.Combine(savePath, "result2.xml");
            //newLoadoutName = "MaerNotApprovedBuild2";

            //try
            //{
            //    PobMergeUtils.Merge(mainPob, pobToAdd, newLoadoutName, pobResult);
            //}
            //catch (Exception ex)
            //{ 
            //    Console.WriteLine(ex.ToString());
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}