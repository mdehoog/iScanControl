namespace Profiler
{
    partial class SaveColor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveColor));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.XYZRadio = new System.Windows.Forms.RadioButton();
            this.xyYRadio = new System.Windows.Forms.RadioButton();
            this.RGBRadio = new System.Windows.Forms.RadioButton();
            this.nameText = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.XYZRadio);
            this.groupBox1.Controls.Add(this.xyYRadio);
            this.groupBox1.Controls.Add(this.RGBRadio);
            this.groupBox1.Controls.Add(this.nameText);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Save:";
            // 
            // XYZRadio
            // 
            this.XYZRadio.AutoSize = true;
            this.XYZRadio.Location = new System.Drawing.Point(50, 68);
            this.XYZRadio.Name = "XYZRadio";
            this.XYZRadio.Size = new System.Drawing.Size(46, 17);
            this.XYZRadio.TabIndex = 3;
            this.XYZRadio.Text = "XYZ";
            this.XYZRadio.UseVisualStyleBackColor = true;
            this.XYZRadio.CheckedChanged += new System.EventHandler(this.XYZRadio_CheckedChanged);
            // 
            // xyYRadio
            // 
            this.xyYRadio.AutoSize = true;
            this.xyYRadio.Location = new System.Drawing.Point(50, 45);
            this.xyYRadio.Name = "xyYRadio";
            this.xyYRadio.Size = new System.Drawing.Size(42, 17);
            this.xyYRadio.TabIndex = 2;
            this.xyYRadio.Text = "xyY";
            this.xyYRadio.UseVisualStyleBackColor = true;
            this.xyYRadio.CheckedChanged += new System.EventHandler(this.xyYRadio_CheckedChanged);
            // 
            // RGBRadio
            // 
            this.RGBRadio.AutoSize = true;
            this.RGBRadio.Checked = true;
            this.RGBRadio.Location = new System.Drawing.Point(50, 91);
            this.RGBRadio.Name = "RGBRadio";
            this.RGBRadio.Size = new System.Drawing.Size(48, 17);
            this.RGBRadio.TabIndex = 4;
            this.RGBRadio.TabStop = true;
            this.RGBRadio.Text = "RGB";
            this.RGBRadio.UseVisualStyleBackColor = true;
            this.RGBRadio.CheckedChanged += new System.EventHandler(this.RGBRadio_CheckedChanged);
            // 
            // nameText
            // 
            this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameText.Location = new System.Drawing.Point(50, 19);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(252, 20);
            this.nameText.TabIndex = 1;
            this.nameText.TextChanged += new System.EventHandler(this.name_TextChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(165, 135);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(246, 135);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // SaveColor
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(333, 166);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveColor";
            this.Text = "Save new color";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.TextBox nameText;
        public System.Windows.Forms.RadioButton xyYRadio;
        public System.Windows.Forms.RadioButton RGBRadio;
        public System.Windows.Forms.RadioButton XYZRadio;
        private System.Windows.Forms.Label label2;
    }
}