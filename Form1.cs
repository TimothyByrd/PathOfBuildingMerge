namespace PathOfBuildingMerge
{
    public partial class Form1 : Form
    {
        private string _pobPath;
        private string[] _multiMergeFiles = [];

        public Form1()
        {
            InitializeComponent();

            var fileFilter = "PoB files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.Filter = fileFilter;
            openFileDialog1.CheckFileExists = true;
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
            var haveMainOrOutput = File.Exists(textBoxMainPobFile.Text) || !string.IsNullOrWhiteSpace(textBoxOutputPob.Text);
            var haveMerge = File.Exists(textBoxPobFileToMerge.Text) || _multiMergeFiles.Length > 1;
            buttonMerge.Enabled = haveMainOrOutput && haveMerge;
        }

        private void ShowOpenFileDialog(string title, TextBox textBox)
        {
            var result = ShowOpenFileDialog(title, textBox.Text, false);
            if (result.Length == 1)
                textBox.Text = result[0];
        }

        private string[] ShowOpenFileDialog(string title, string initialText, bool multiSelect = false)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(_pobPath))
                Directory.SetCurrentDirectory(_pobPath);
            openFileDialog1.Title = title;
            openFileDialog1.FileName = initialText;
            openFileDialog1.Multiselect = multiSelect;
            string[] result = openFileDialog1.ShowDialog() == DialogResult.OK ? openFileDialog1.FileNames : [];
            Directory.SetCurrentDirectory(currentDirectory);
            return result;
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

        private void buttonMulitiMerge_Click(object sender, EventArgs e)
        {
            var result = ShowOpenFileDialog("Select multiple PoB snapshots to merge together", String.Empty, true);
            if (result.Length > 1)
            {
                _multiMergeFiles = result;
                textBoxPobFileToMerge.Text = "<multiple>";
            }
            else if (result.Length == 1)
            {
                textBoxPobFileToMerge.Text = result[0];
                _multiMergeFiles = [];
            }
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
            if (_multiMergeFiles.Length > 1)
                labelPobToMerge.Text = "PoB file to merge in - multiple files selected, loadout name will be ignored";
            else if (File.Exists(textBoxPobFileToMerge.Text))
                labelPobToMerge.Text = $"PoB file to merge in - '{Path.GetFileNameWithoutExtension(textBoxPobFileToMerge.Text)}'";
            else
                labelPobToMerge.Text = "PoB file to merge in (required)";
        }

        private void textBoxOutputPob_TextChanged(object sender, EventArgs e)
        {
            CheckEnableMergeButton();
        }

        private void buttonMerge_Click(object sender, EventArgs e)
        {
            var startingWithEmptyPoB = false;

            var mainPob = textBoxMainPobFile.Text;
            if (string.IsNullOrWhiteSpace(mainPob))
            {
                var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (exePath != null)
                {
                    mainPob = Path.Combine(exePath, "empty.xml");
                    startingWithEmptyPoB = true;
                }
            }

            var outputPob = textBoxOutputPob.Text;
            if (string.IsNullOrWhiteSpace(outputPob))
            {
                if (!startingWithEmptyPoB)
                    outputPob = mainPob;
                else
                {
                    MessageBox.Show(this, "Need to specify a main file or an output file", "Error");
                    return;
                }
            }

            bool onlyAddUsedItems = checkBoxOnlyAddUsedItems.Checked;
            bool reuseExistingItems = checkBoxReuseExisitngItems.Checked;

            if (_multiMergeFiles.Length > 1)
            {
                foreach (string file in _multiMergeFiles)
                {
                    var loadout = Path.GetFileNameWithoutExtension(file);
                    try
                    {
                        PobMergeUtils.Merge(mainPob, file, loadout, outputPob, onlyAddUsedItems: onlyAddUsedItems, reuseExistingItems: reuseExistingItems);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show(this, ex.Message, "Error");
                        return;
                    }
                    mainPob = outputPob;
                }
                MessageBox.Show(this, $"Merged {_multiMergeFiles.Length} PoBs into '{Path.GetFileName(outputPob)}'", "Success");
                return;
            }

            var pobToMerge = textBoxPobFileToMerge.Text;
            var newLoadoutName = textBoxNewLoadoutName.Text;
            if (string.IsNullOrWhiteSpace(newLoadoutName))
                newLoadoutName = Path.GetFileNameWithoutExtension(pobToMerge);

            try
            {
                PobMergeUtils.Merge(mainPob, pobToMerge, newLoadoutName, outputPob, onlyAddUsedItems: onlyAddUsedItems, reuseExistingItems: reuseExistingItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(this, ex.Message, "Error");
                return;
            }

            var msg = $"Merged PoB '{Path.GetFileName(pobToMerge)}' into '{Path.GetFileName(mainPob)}' as loadout '{newLoadoutName}', saving result to '{Path.GetFileName(outputPob)}'";
            MessageBox.Show(this, msg, "Success");
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
