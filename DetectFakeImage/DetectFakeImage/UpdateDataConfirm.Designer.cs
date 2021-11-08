
namespace TLI.FakeImage
{
    partial class UpdateDataConfirm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioFake = new System.Windows.Forms.RadioButton();
            this.radioReal = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCance = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioFake);
            this.groupBox1.Controls.Add(this.radioReal);
            this.groupBox1.Location = new System.Drawing.Point(18, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(358, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // radioFake
            // 
            this.radioFake.AutoSize = true;
            this.radioFake.Location = new System.Drawing.Point(148, 19);
            this.radioFake.Name = "radioFake";
            this.radioFake.Size = new System.Drawing.Size(61, 17);
            this.radioFake.TabIndex = 1;
            this.radioFake.TabStop = true;
            this.radioFake.Text = "Ảnh giả";
            this.radioFake.UseVisualStyleBackColor = true;
            // 
            // radioReal
            // 
            this.radioReal.AutoSize = true;
            this.radioReal.Location = new System.Drawing.Point(6, 19);
            this.radioReal.Name = "radioReal";
            this.radioReal.Size = new System.Drawing.Size(65, 17);
            this.radioReal.TabIndex = 0;
            this.radioReal.TabStop = true;
            this.radioReal.Text = "Ảnh thật";
            this.radioReal.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(166, 138);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCance
            // 
            this.btnCance.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCance.Location = new System.Drawing.Point(281, 138);
            this.btnCance.Name = "btnCance";
            this.btnCance.Size = new System.Drawing.Size(95, 30);
            this.btnCance.TabIndex = 2;
            this.btnCance.Text = "Cancel";
            this.btnCance.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Các ảnh trên đều là:";
            // 
            // UpdateDataConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(401, 180);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCance);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UpdateDataConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cập nhật dữ liệu huấn luyện";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioFake;
        private System.Windows.Forms.RadioButton radioReal;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCance;
        private System.Windows.Forms.Label label1;
    }
}
