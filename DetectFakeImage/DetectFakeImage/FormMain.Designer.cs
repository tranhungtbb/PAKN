
namespace FakeImageDetection
{
    partial class FormMain
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
            this.btnBrowserFolder = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.btnResetCache = new System.Windows.Forms.Button();
            this.btnCreateCache = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFake = new System.Windows.Forms.Label();
            this.lbReal = new System.Windows.Forms.Label();
            this.btnLearn = new System.Windows.Forms.Button();
            this.btnBrowseImage = new System.Windows.Forms.Button();
            this.btnCheckImage = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowserFolder
            // 
            this.btnBrowserFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowserFolder.Location = new System.Drawing.Point(6, 86);
            this.btnBrowserFolder.Name = "btnBrowserFolder";
            this.btnBrowserFolder.Size = new System.Drawing.Size(153, 36);
            this.btnBrowserFolder.TabIndex = 1;
            this.btnBrowserFolder.Text = "Chọn thư mục...";
            this.btnBrowserFolder.UseVisualStyleBackColor = true;
            this.btnBrowserFolder.Click += new System.EventHandler(this.btnBrowserFolder_Click);
            // 
            // buttonPanel
            // 
            this.buttonPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonPanel.Controls.Add(this.btnResetCache);
            this.buttonPanel.Controls.Add(this.btnCreateCache);
            this.buttonPanel.Controls.Add(this.label1);
            this.buttonPanel.Controls.Add(this.label2);
            this.buttonPanel.Controls.Add(this.lbFake);
            this.buttonPanel.Controls.Add(this.lbReal);
            this.buttonPanel.Controls.Add(this.btnLearn);
            this.buttonPanel.Controls.Add(this.btnBrowseImage);
            this.buttonPanel.Controls.Add(this.btnCheckImage);
            this.buttonPanel.Controls.Add(this.btnBrowserFolder);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonPanel.Location = new System.Drawing.Point(750, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(171, 492);
            this.buttonPanel.TabIndex = 3;
            // 
            // btnResetCache
            // 
            this.btnResetCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetCache.Location = new System.Drawing.Point(9, 380);
            this.btnResetCache.Name = "btnResetCache";
            this.btnResetCache.Size = new System.Drawing.Size(153, 36);
            this.btnResetCache.TabIndex = 9;
            this.btnResetCache.Text = "Reset Simple Data";
            this.btnResetCache.UseVisualStyleBackColor = true;
            this.btnResetCache.Click += new System.EventHandler(this.btnResetCache_Click);
            // 
            // btnCreateCache
            // 
            this.btnCreateCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateCache.Location = new System.Drawing.Point(9, 326);
            this.btnCreateCache.Name = "btnCreateCache";
            this.btnCreateCache.Size = new System.Drawing.Size(153, 36);
            this.btnCreateCache.TabIndex = 9;
            this.btnCreateCache.Text = "Simple Data";
            this.btnCreateCache.UseVisualStyleBackColor = true;
            this.btnCreateCache.Click += new System.EventHandler(this.btnCreateCache_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ảnh giả:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ảnh thật:";
            // 
            // lbFake
            // 
            this.lbFake.AutoSize = true;
            this.lbFake.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFake.Location = new System.Drawing.Point(63, 223);
            this.lbFake.Name = "lbFake";
            this.lbFake.Size = new System.Drawing.Size(15, 15);
            this.lbFake.TabIndex = 5;
            this.lbFake.Text = "0";
            // 
            // lbReal
            // 
            this.lbReal.AutoSize = true;
            this.lbReal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbReal.Location = new System.Drawing.Point(63, 197);
            this.lbReal.Name = "lbReal";
            this.lbReal.Size = new System.Drawing.Size(15, 15);
            this.lbReal.TabIndex = 4;
            this.lbReal.Text = "0";
            // 
            // btnLearn
            // 
            this.btnLearn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLearn.Location = new System.Drawing.Point(9, 254);
            this.btnLearn.Name = "btnLearn";
            this.btnLearn.Size = new System.Drawing.Size(153, 56);
            this.btnLearn.TabIndex = 3;
            this.btnLearn.Text = "Cập nhật dữ liệu huấn luyện...";
            this.btnLearn.UseVisualStyleBackColor = true;
            this.btnLearn.Click += new System.EventHandler(this.btnLearnFake_Click);
            // 
            // btnBrowseImage
            // 
            this.btnBrowseImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseImage.Location = new System.Drawing.Point(6, 32);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(153, 36);
            this.btnBrowseImage.TabIndex = 2;
            this.btnBrowseImage.Text = "Chọn ảnh...";
            this.btnBrowseImage.UseVisualStyleBackColor = true;
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            // 
            // btnCheckImage
            // 
            this.btnCheckImage.Enabled = false;
            this.btnCheckImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckImage.Location = new System.Drawing.Point(6, 152);
            this.btnCheckImage.Name = "btnCheckImage";
            this.btnCheckImage.Size = new System.Drawing.Size(153, 36);
            this.btnCheckImage.TabIndex = 1;
            this.btnCheckImage.Text = "Check thật/giả";
            this.btnCheckImage.UseVisualStyleBackColor = true;
            this.btnCheckImage.Click += new System.EventHandler(this.btnCheckImage_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(750, 492);
            this.webBrowser1.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 492);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.buttonPanel);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phát hiện ảnh giả";
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnBrowserFolder;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.Button btnLearn;
        private System.Windows.Forms.Label lbReal;
        private System.Windows.Forms.Label lbFake;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreateCache;
        private System.Windows.Forms.Button btnCheckImage;
        private System.Windows.Forms.Button btnResetCache;
    }
}

