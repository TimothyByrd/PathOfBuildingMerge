using System.Windows.Forms;

namespace PathOfBuildingMerge
{
    public partial class Form1 : Form
    {
        private string _pobPath;

        public Form1()
        {
            InitializeComponent();

            var fileFilter = "PoB files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.Filter = fileFilter;
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _pobPath = Path.Combine(documentsPath, "Path of Building\\Builds");

            saveFileDialog1.Filter = fileFilter;
            saveFileDialog1.Title = "Select the name of the PoB file to write";
            saveFileDialog1.RestoreDirectory = true;

            CheckEnableMergeButton();
        }

        private void CheckEnableMergeButton()
        {
            var enable = File.Exists(textBoxMainPobFile.Text) && File.Exists(textBoxPobFileToMerge.Text);
            buttonMerge.Enabled = enable;
        }

        private void ShowOpenFileDialog(string title, TextBox textBox, bool mustExist = true)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(_pobPath))
                Directory.SetCurrentDirectory(_pobPath);
            openFileDialog1.Title = title;
            openFileDialog1.FileName = textBox.Text;
            openFileDialog1.CheckFileExists = mustExist;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox.Text = openFileDialog1.FileName;
            Directory.SetCurrentDirectory(currentDirectory);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void buttonBrowseMainPobFile_Click(object sender, EventArgs e)
        {
            ShowOpenFileDialog("Select the main PoB file", textBoxMainPobFile);
        }

        private void buttonBrowsePobFileToMerge_Click(object sender, EventArgs e)
        {
            ShowOpenFileDialog("Select the PoB file to merge in", textBoxPobFileToMerge);
        }

        private void buttonBrowseOutputPob_Click(object sender, EventArgs e)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(_pobPath))
                Directory.SetCurrentDirectory(_pobPath);
            saveFileDialog1.FileName = textBoxOutputPob.Text;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                textBoxOutputPob.Text = saveFileDialog1.FileName;
            Directory.SetCurrentDirectory(currentDirectory);
        }

        private void textBoxMainPobFile_TextChanged(object sender, EventArgs e)
        {
            CheckEnableMergeButton();
            if (File.Exists(textBoxMainPobFile.Text))
                labelMainPobFile.Text = $"Main PoB file - '{Path.GetFileNameWithoutExtension(textBoxMainPobFile.Text)}'";
            else
                labelMainPobFile.Text = "Main PoB file (required)";
        }

        private void textBoxPobFileToMerge_TextChanged(object sender, EventArgs e)
        {
            CheckEnableMergeButton();
            if (File.Exists(textBoxPobFileToMerge.Text))
                labelPobToMerge.Text = $"PoB file to merge in - '{Path.GetFileNameWithoutExtension(textBoxPobFileToMerge.Text)}'";
            else
                labelPobToMerge.Text = "PoB file to merge in (required)";
        }

        private void buttonMerge_Click(object sender, EventArgs e)
        {
            var mainPob = textBoxMainPobFile.Text;
            var pobToMerge = textBoxPobFileToMerge.Text;
            var newLoadoutName = textBoxNewLoadoutName.Text;
            if (string.IsNullOrWhiteSpace(newLoadoutName))
                newLoadoutName = Path.GetFileNameWithoutExtension(pobToMerge);

            var outputPob = textBoxOutputPob.Text;
            if (string.IsNullOrWhiteSpace(outputPob))
                outputPob = mainPob;

            try
            {
                PobMergeUtils.Merge(mainPob, pobToMerge, newLoadoutName, outputPob);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(this, ex.Message, "Error");
                return;
            }

            var msg = $"Merged PoB '{Path.GetFileNameWithoutExtension(pobToMerge)}' into '{Path.GetFileNameWithoutExtension(mainPob)}' as loadout {newLoadoutName}, saving result to {Path.GetFileNameWithoutExtension(outputPob)}";
            MessageBox.Show(this, msg, "Success");
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
