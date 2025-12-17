namespace PathOfBuildingMerge
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            labelMainPobFile = new Label();
            buttonBrowseMainPobFile = new Button();
            textBoxMainPobFile = new TextBox();
            textBoxPobFileToMerge = new TextBox();
            buttonBrowsePobFileToMerge = new Button();
            labelPobToMerge = new Label();
            textBoxNewLoadoutName = new TextBox();
            label3 = new Label();
            textBoxOutputPob = new TextBox();
            buttonBrowseOutputPob = new Button();
            label4 = new Label();
            buttonMerge = new Button();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            checkBoxOnlyAddUsedItems = new CheckBox();
            checkBoxReuseExisitngItems = new CheckBox();
            toolTip1 = new ToolTip(components);
            buttonMultiMerge = new Button();
            SuspendLayout();
            // 
            // labelMainPobFile
            // 
            labelMainPobFile.AutoSize = true;
            labelMainPobFile.Location = new Point(12, 9);
            labelMainPobFile.Name = "labelMainPobFile";
            labelMainPobFile.Size = new Size(77, 15);
            labelMainPobFile.TabIndex = 0;
            labelMainPobFile.Text = "Main PoB file";
            toolTip1.SetToolTip(labelMainPobFile, "The PoB file to add/replace a loadout in");
            // 
            // buttonBrowseMainPobFile
            // 
            buttonBrowseMainPobFile.Location = new Point(12, 27);
            buttonBrowseMainPobFile.Name = "buttonBrowseMainPobFile";
            buttonBrowseMainPobFile.Size = new Size(75, 23);
            buttonBrowseMainPobFile.TabIndex = 1;
            buttonBrowseMainPobFile.Text = "Browse";
            buttonBrowseMainPobFile.UseVisualStyleBackColor = true;
            buttonBrowseMainPobFile.Click += buttonBrowseMainPobFile_Click;
            // 
            // textBoxMainPobFile
            // 
            textBoxMainPobFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxMainPobFile.Location = new Point(93, 27);
            textBoxMainPobFile.Name = "textBoxMainPobFile";
            textBoxMainPobFile.PlaceholderText = "<not specified - an empty PoB will be used>";
            textBoxMainPobFile.Size = new Size(434, 23);
            textBoxMainPobFile.TabIndex = 2;
            textBoxMainPobFile.TextChanged += textBoxMainPobFile_TextChanged;
            // 
            // textBoxPobFileToMerge
            // 
            textBoxPobFileToMerge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPobFileToMerge.Location = new Point(93, 83);
            textBoxPobFileToMerge.Name = "textBoxPobFileToMerge";
            textBoxPobFileToMerge.Size = new Size(434, 23);
            textBoxPobFileToMerge.TabIndex = 5;
            textBoxPobFileToMerge.TextChanged += textBoxPobFileToMerge_TextChanged;
            // 
            // buttonBrowsePobFileToMerge
            // 
            buttonBrowsePobFileToMerge.Location = new Point(12, 83);
            buttonBrowsePobFileToMerge.Name = "buttonBrowsePobFileToMerge";
            buttonBrowsePobFileToMerge.Size = new Size(75, 23);
            buttonBrowsePobFileToMerge.TabIndex = 4;
            buttonBrowsePobFileToMerge.Text = "Browse";
            buttonBrowsePobFileToMerge.UseVisualStyleBackColor = true;
            buttonBrowsePobFileToMerge.Click += buttonBrowsePobFileToMerge_Click;
            // 
            // labelPobToMerge
            // 
            labelPobToMerge.AutoSize = true;
            labelPobToMerge.Location = new Point(12, 65);
            labelPobToMerge.Name = "labelPobToMerge";
            labelPobToMerge.Size = new Size(111, 15);
            labelPobToMerge.TabIndex = 3;
            labelPobToMerge.Text = "PoB file to merge in";
            toolTip1.SetToolTip(labelPobToMerge, "The PoB file to copy the active loadout from. If this PoB has multiple loadouts, make the desired loadout active and save the PoB first.");
            // 
            // textBoxNewLoadoutName
            // 
            textBoxNewLoadoutName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxNewLoadoutName.Location = new Point(12, 168);
            textBoxNewLoadoutName.Name = "textBoxNewLoadoutName";
            textBoxNewLoadoutName.PlaceholderText = "<the merge file name will be used as the loadout name>";
            textBoxNewLoadoutName.Size = new Size(515, 23);
            textBoxNewLoadoutName.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 150);
            label3.Name = "label3";
            label3.Size = new Size(275, 15);
            label3.TabIndex = 6;
            label3.Text = "New Loadout name (blank to use merge file name)";
            toolTip1.SetToolTip(label3, resources.GetString("label3.ToolTip"));
            // 
            // textBoxOutputPob
            // 
            textBoxOutputPob.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxOutputPob.Location = new Point(93, 232);
            textBoxOutputPob.Name = "textBoxOutputPob";
            textBoxOutputPob.PlaceholderText = "<results will be saved back to the Main PoB file>";
            textBoxOutputPob.Size = new Size(434, 23);
            textBoxOutputPob.TabIndex = 10;
            textBoxOutputPob.TextChanged += textBoxOutputPob_TextChanged;
            // 
            // buttonBrowseOutputPob
            // 
            buttonBrowseOutputPob.Location = new Point(12, 232);
            buttonBrowseOutputPob.Name = "buttonBrowseOutputPob";
            buttonBrowseOutputPob.Size = new Size(75, 23);
            buttonBrowseOutputPob.TabIndex = 9;
            buttonBrowseOutputPob.Text = "Browse";
            buttonBrowseOutputPob.UseVisualStyleBackColor = true;
            buttonBrowseOutputPob.Click += buttonBrowseOutputPob_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 214);
            label4.Name = "label4";
            label4.Size = new Size(259, 15);
            label4.TabIndex = 8;
            label4.Text = "Output PoB file (blank to save back to main file)";
            toolTip1.SetToolTip(label4, "Where to save the resulting merged PoB file. If left blank, the main PoB file above will be replaced.");
            // 
            // buttonMerge
            // 
            buttonMerge.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonMerge.Location = new Point(452, 304);
            buttonMerge.Name = "buttonMerge";
            buttonMerge.Size = new Size(75, 23);
            buttonMerge.TabIndex = 11;
            buttonMerge.Text = "Merge";
            buttonMerge.UseVisualStyleBackColor = true;
            buttonMerge.Click += buttonMerge_Click;
            // 
            // checkBoxOnlyAddUsedItems
            // 
            checkBoxOnlyAddUsedItems.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBoxOnlyAddUsedItems.AutoSize = true;
            checkBoxOnlyAddUsedItems.Checked = true;
            checkBoxOnlyAddUsedItems.CheckState = CheckState.Checked;
            checkBoxOnlyAddUsedItems.Location = new Point(12, 279);
            checkBoxOnlyAddUsedItems.Name = "checkBoxOnlyAddUsedItems";
            checkBoxOnlyAddUsedItems.Size = new Size(183, 19);
            checkBoxOnlyAddUsedItems.TabIndex = 12;
            checkBoxOnlyAddUsedItems.Text = "Only add items that are in use";
            toolTip1.SetToolTip(checkBoxOnlyAddUsedItems, "Only copy items from the merge PoB that are used in the loadout being copied. (Should not matter for PoBs from poe.,ninja)");
            checkBoxOnlyAddUsedItems.UseVisualStyleBackColor = true;
            // 
            // checkBoxReuseExisitngItems
            // 
            checkBoxReuseExisitngItems.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBoxReuseExisitngItems.AutoSize = true;
            checkBoxReuseExisitngItems.Checked = true;
            checkBoxReuseExisitngItems.CheckState = CheckState.Checked;
            checkBoxReuseExisitngItems.Location = new Point(12, 304);
            checkBoxReuseExisitngItems.Name = "checkBoxReuseExisitngItems";
            checkBoxReuseExisitngItems.Size = new Size(132, 19);
            checkBoxReuseExisitngItems.TabIndex = 13;
            checkBoxReuseExisitngItems.Text = "Reuse existing items";
            toolTip1.SetToolTip(checkBoxReuseExisitngItems, "If an item is already in the main PoB for an existing snapshot, reuse that item rather than making another copy of it in the output PoB.");
            checkBoxReuseExisitngItems.UseVisualStyleBackColor = true;
            // 
            // buttonMultiMerge
            // 
            buttonMultiMerge.Location = new Point(12, 112);
            buttonMultiMerge.Name = "buttonMultiMerge";
            buttonMultiMerge.Size = new Size(111, 23);
            buttonMultiMerge.TabIndex = 14;
            buttonMultiMerge.Text = "Multi-merge";
            toolTip1.SetToolTip(buttonMultiMerge, "Select several files to merge active snapshots from at once. Either a Main PoB file or an Output PoB file must also be selected. The file anmes will be used for the loadout names.");
            buttonMultiMerge.UseVisualStyleBackColor = true;
            buttonMultiMerge.Click += buttonMultiMerge_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(539, 338);
            Controls.Add(buttonMultiMerge);
            Controls.Add(checkBoxReuseExisitngItems);
            Controls.Add(checkBoxOnlyAddUsedItems);
            Controls.Add(buttonMerge);
            Controls.Add(textBoxOutputPob);
            Controls.Add(buttonBrowseOutputPob);
            Controls.Add(label4);
            Controls.Add(textBoxNewLoadoutName);
            Controls.Add(label3);
            Controls.Add(textBoxPobFileToMerge);
            Controls.Add(buttonBrowsePobFileToMerge);
            Controls.Add(labelPobToMerge);
            Controls.Add(textBoxMainPobFile);
            Controls.Add(buttonBrowseMainPobFile);
            Controls.Add(labelMainPobFile);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(555, 353);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Path of Building Merge Tool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelMainPobFile;
        private Button buttonBrowseMainPobFile;
        private TextBox textBoxMainPobFile;
        private TextBox textBoxPobFileToMerge;
        private Button buttonBrowsePobFileToMerge;
        private Label labelPobToMerge;
        private TextBox textBoxNewLoadoutName;
        private Label label3;
        private TextBox textBoxOutputPob;
        private Button buttonBrowseOutputPob;
        private Label label4;
        private Button buttonMerge;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private CheckBox checkBoxOnlyAddUsedItems;
        private CheckBox checkBoxReuseExisitngItems;
        private ToolTip toolTip1;
        private Button buttonMultiMerge;
    }
}
