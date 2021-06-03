
namespace FontSpacingEditor
{
    partial class MainForm
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadFontButton = new System.Windows.Forms.Button();
            this.loadTableButton = new System.Windows.Forms.Button();
            this.fontPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveTableButton = new System.Windows.Forms.Button();
            this.charPictureBox = new System.Windows.Forms.PictureBox();
            this.topLeftNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.middleLeftNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.bottomLeftNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.topRightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.middleRightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.bottomRightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.automaticValuesButton = new System.Windows.Forms.Button();
            this.marginNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.fontPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.charPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.topLeftNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.middleLeftNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.bottomLeftNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.topRightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.middleRightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.bottomRightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.marginNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // loadFontButton
            // 
            this.loadFontButton.Location = new System.Drawing.Point(10, 10);
            this.loadFontButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadFontButton.Name = "loadFontButton";
            this.loadFontButton.Size = new System.Drawing.Size(200, 23);
            this.loadFontButton.TabIndex = 0;
            this.loadFontButton.Text = "1. Load font image";
            this.loadFontButton.UseVisualStyleBackColor = true;
            this.loadFontButton.Click += new System.EventHandler(this.LoadFontButtonClick);
            // 
            // loadTableButton
            // 
            this.loadTableButton.Location = new System.Drawing.Point(219, 10);
            this.loadTableButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadTableButton.Name = "loadTableButton";
            this.loadTableButton.Size = new System.Drawing.Size(200, 23);
            this.loadTableButton.TabIndex = 1;
            this.loadTableButton.Text = "2. Load spacing table";
            this.loadTableButton.UseVisualStyleBackColor = true;
            this.loadTableButton.Click += new System.EventHandler(this.LoadTableButtonClick);
            // 
            // fontPictureBox
            // 
            this.fontPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fontPictureBox.Location = new System.Drawing.Point(10, 64);
            this.fontPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.fontPictureBox.Name = "fontPictureBox";
            this.fontPictureBox.Size = new System.Drawing.Size(512, 512);
            this.fontPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.fontPictureBox.TabIndex = 2;
            this.fontPictureBox.TabStop = false;
            this.fontPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.FontPicturePaint);
            this.fontPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FontPictureMouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "3. Click on a character to edit";
            // 
            // saveTableButton
            // 
            this.saveTableButton.Location = new System.Drawing.Point(10, 580);
            this.saveTableButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveTableButton.Name = "saveTableButton";
            this.saveTableButton.Size = new System.Drawing.Size(202, 23);
            this.saveTableButton.TabIndex = 11;
            this.saveTableButton.Text = "5. Save spacing table";
            this.saveTableButton.UseVisualStyleBackColor = true;
            this.saveTableButton.Click += new System.EventHandler(this.SaveTableButtonClick);
            // 
            // charPictureBox
            // 
            this.charPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.charPictureBox.Location = new System.Drawing.Point(602, 63);
            this.charPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.charPictureBox.Name = "charPictureBox";
            this.charPictureBox.Size = new System.Drawing.Size(512, 512);
            this.charPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.charPictureBox.TabIndex = 8;
            this.charPictureBox.TabStop = false;
            this.charPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CharPicturePaint);
            // 
            // topLeftNumericUpDown
            // 
            this.topLeftNumericUpDown.DecimalPlaces = 3;
            this.topLeftNumericUpDown.Enabled = false;
            this.topLeftNumericUpDown.Increment = new decimal(new int[] {5, 0, 0, 131072});
            this.topLeftNumericUpDown.Location = new System.Drawing.Point(540, 64);
            this.topLeftNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.topLeftNumericUpDown.Maximum = new decimal(new int[] {2, 0, 0, 0});
            this.topLeftNumericUpDown.Minimum = new decimal(new int[] {2, 0, 0, -2147483648});
            this.topLeftNumericUpDown.Name = "topLeftNumericUpDown";
            this.topLeftNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.topLeftNumericUpDown.TabIndex = 3;
            this.topLeftNumericUpDown.Tag = "TopLeft";
            this.topLeftNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.topLeftNumericUpDown.ValueChanged += new System.EventHandler(this.MarginValueChanged);
            // 
            // middleLeftNumericUpDown
            // 
            this.middleLeftNumericUpDown.DecimalPlaces = 3;
            this.middleLeftNumericUpDown.Enabled = false;
            this.middleLeftNumericUpDown.Increment = new decimal(new int[] {5, 0, 0, 131072});
            this.middleLeftNumericUpDown.Location = new System.Drawing.Point(540, 258);
            this.middleLeftNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.middleLeftNumericUpDown.Maximum = new decimal(new int[] {2, 0, 0, 0});
            this.middleLeftNumericUpDown.Minimum = new decimal(new int[] {2, 0, 0, -2147483648});
            this.middleLeftNumericUpDown.Name = "middleLeftNumericUpDown";
            this.middleLeftNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.middleLeftNumericUpDown.TabIndex = 5;
            this.middleLeftNumericUpDown.Tag = "MiddleLeft";
            this.middleLeftNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.middleLeftNumericUpDown.ValueChanged += new System.EventHandler(this.MarginValueChanged);
            // 
            // bottomLeftNumericUpDown
            // 
            this.bottomLeftNumericUpDown.DecimalPlaces = 3;
            this.bottomLeftNumericUpDown.Enabled = false;
            this.bottomLeftNumericUpDown.Increment = new decimal(new int[] {5, 0, 0, 131072});
            this.bottomLeftNumericUpDown.Location = new System.Drawing.Point(540, 452);
            this.bottomLeftNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.bottomLeftNumericUpDown.Maximum = new decimal(new int[] {2, 0, 0, 0});
            this.bottomLeftNumericUpDown.Minimum = new decimal(new int[] {2, 0, 0, -2147483648});
            this.bottomLeftNumericUpDown.Name = "bottomLeftNumericUpDown";
            this.bottomLeftNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.bottomLeftNumericUpDown.TabIndex = 7;
            this.bottomLeftNumericUpDown.Tag = "BottomLeft";
            this.bottomLeftNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bottomLeftNumericUpDown.ValueChanged += new System.EventHandler(this.MarginValueChanged);
            // 
            // topRightNumericUpDown
            // 
            this.topRightNumericUpDown.DecimalPlaces = 3;
            this.topRightNumericUpDown.Enabled = false;
            this.topRightNumericUpDown.Increment = new decimal(new int[] {5, 0, 0, 131072});
            this.topRightNumericUpDown.Location = new System.Drawing.Point(1118, 64);
            this.topRightNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.topRightNumericUpDown.Maximum = new decimal(new int[] {2, 0, 0, 0});
            this.topRightNumericUpDown.Minimum = new decimal(new int[] {2, 0, 0, -2147483648});
            this.topRightNumericUpDown.Name = "topRightNumericUpDown";
            this.topRightNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.topRightNumericUpDown.TabIndex = 4;
            this.topRightNumericUpDown.Tag = "TopRight";
            this.topRightNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.topRightNumericUpDown.ValueChanged += new System.EventHandler(this.MarginValueChanged);
            // 
            // middleRightNumericUpDown
            // 
            this.middleRightNumericUpDown.DecimalPlaces = 3;
            this.middleRightNumericUpDown.Enabled = false;
            this.middleRightNumericUpDown.Increment = new decimal(new int[] {5, 0, 0, 131072});
            this.middleRightNumericUpDown.Location = new System.Drawing.Point(1118, 258);
            this.middleRightNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.middleRightNumericUpDown.Maximum = new decimal(new int[] {2, 0, 0, 0});
            this.middleRightNumericUpDown.Minimum = new decimal(new int[] {2, 0, 0, -2147483648});
            this.middleRightNumericUpDown.Name = "middleRightNumericUpDown";
            this.middleRightNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.middleRightNumericUpDown.TabIndex = 6;
            this.middleRightNumericUpDown.Tag = "MiddleRight";
            this.middleRightNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.middleRightNumericUpDown.ValueChanged += new System.EventHandler(this.MarginValueChanged);
            // 
            // bottomRightNumericUpDown
            // 
            this.bottomRightNumericUpDown.DecimalPlaces = 3;
            this.bottomRightNumericUpDown.Enabled = false;
            this.bottomRightNumericUpDown.Increment = new decimal(new int[] {5, 0, 0, 131072});
            this.bottomRightNumericUpDown.Location = new System.Drawing.Point(1118, 452);
            this.bottomRightNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.bottomRightNumericUpDown.Maximum = new decimal(new int[] {2, 0, 0, 0});
            this.bottomRightNumericUpDown.Minimum = new decimal(new int[] {2, 0, 0, -2147483648});
            this.bottomRightNumericUpDown.Name = "bottomRightNumericUpDown";
            this.bottomRightNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.bottomRightNumericUpDown.TabIndex = 8;
            this.bottomRightNumericUpDown.Tag = "BottomRight";
            this.bottomRightNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bottomRightNumericUpDown.ValueChanged += new System.EventHandler(this.MarginValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(602, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "4. Edit character margins";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Yakuza font spacing table (*.txt)|*.txt";
            // 
            // automaticValuesButton
            // 
            this.automaticValuesButton.Location = new System.Drawing.Point(748, 581);
            this.automaticValuesButton.Margin = new System.Windows.Forms.Padding(2);
            this.automaticValuesButton.Name = "automaticValuesButton";
            this.automaticValuesButton.Size = new System.Drawing.Size(67, 23);
            this.automaticValuesButton.TabIndex = 10;
            this.automaticValuesButton.Text = "Calculate";
            this.automaticValuesButton.UseVisualStyleBackColor = true;
            this.automaticValuesButton.Click += new System.EventHandler(this.AutomaticValuesButtonClick);
            // 
            // marginNumericUpDown
            // 
            this.marginNumericUpDown.Location = new System.Drawing.Point(708, 581);
            this.marginNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.marginNumericUpDown.Maximum = new decimal(new int[] {10, 0, 0, 0});
            this.marginNumericUpDown.Minimum = new decimal(new int[] {1, 0, 0, -2147483648});
            this.marginNumericUpDown.Name = "marginNumericUpDown";
            this.marginNumericUpDown.Size = new System.Drawing.Size(36, 20);
            this.marginNumericUpDown.TabIndex = 16;
            this.marginNumericUpDown.Tag = "Margin";
            this.marginNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.marginNumericUpDown.Value = new decimal(new int[] {1, 0, 0, 0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(602, 583);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Margin (in pixels):";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1184, 611);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.marginNumericUpDown);
            this.Controls.Add(this.automaticValuesButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bottomRightNumericUpDown);
            this.Controls.Add(this.middleRightNumericUpDown);
            this.Controls.Add(this.topRightNumericUpDown);
            this.Controls.Add(this.bottomLeftNumericUpDown);
            this.Controls.Add(this.middleLeftNumericUpDown);
            this.Controls.Add(this.topLeftNumericUpDown);
            this.Controls.Add(this.charPictureBox);
            this.Controls.Add(this.saveTableButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fontPictureBox);
            this.Controls.Add(this.loadTableButton);
            this.Controls.Add(this.loadFontButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 650);
            this.MinimumSize = new System.Drawing.Size(1200, 650);
            this.Name = "MainForm";
            this.Text = "Yakuza Font Spacing Editor";
            ((System.ComponentModel.ISupportInitialize) (this.fontPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.charPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.topLeftNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.middleLeftNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.bottomLeftNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.topRightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.middleRightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.bottomRightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.marginNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.NumericUpDown marginNumericUpDown;
        private System.Windows.Forms.Label label3;

        #endregion

        private System.Windows.Forms.Button loadFontButton;
        private System.Windows.Forms.Button loadTableButton;
        private System.Windows.Forms.PictureBox fontPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveTableButton;
        private System.Windows.Forms.PictureBox charPictureBox;
        private System.Windows.Forms.NumericUpDown topLeftNumericUpDown;
        private System.Windows.Forms.NumericUpDown middleLeftNumericUpDown;
        private System.Windows.Forms.NumericUpDown bottomLeftNumericUpDown;
        private System.Windows.Forms.NumericUpDown topRightNumericUpDown;
        private System.Windows.Forms.NumericUpDown middleRightNumericUpDown;
        private System.Windows.Forms.NumericUpDown bottomRightNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button automaticValuesButton;
    }
}

