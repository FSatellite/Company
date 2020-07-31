namespace ToolResolver
{
    partial class CreoConfig
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
            this.button_Sel = new System.Windows.Forms.Button();
            this.Creo软件路径 = new System.Windows.Forms.Label();
            this.textBox_CreoPath = new System.Windows.Forms.TextBox();
            this.button_Confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Sel
            // 
            this.button_Sel.Location = new System.Drawing.Point(355, 26);
            this.button_Sel.Name = "button_Sel";
            this.button_Sel.Size = new System.Drawing.Size(75, 23);
            this.button_Sel.TabIndex = 0;
            this.button_Sel.Text = "选择";
            this.button_Sel.UseVisualStyleBackColor = true;
            this.button_Sel.Click += new System.EventHandler(this.button_Sel_Click);
            // 
            // Creo软件路径
            // 
            this.Creo软件路径.AutoSize = true;
            this.Creo软件路径.Location = new System.Drawing.Point(20, 29);
            this.Creo软件路径.Name = "Creo软件路径";
            this.Creo软件路径.Size = new System.Drawing.Size(77, 12);
            this.Creo软件路径.TabIndex = 1;
            this.Creo软件路径.Text = "Creo软件路径";
            // 
            // textBox_CreoPath
            // 
            this.textBox_CreoPath.Location = new System.Drawing.Point(103, 26);
            this.textBox_CreoPath.Name = "textBox_CreoPath";
            this.textBox_CreoPath.Size = new System.Drawing.Size(233, 21);
            this.textBox_CreoPath.TabIndex = 2;
            // 
            // button_Confirm
            // 
            this.button_Confirm.Location = new System.Drawing.Point(186, 62);
            this.button_Confirm.Name = "button_Confirm";
            this.button_Confirm.Size = new System.Drawing.Size(75, 23);
            this.button_Confirm.TabIndex = 3;
            this.button_Confirm.Text = "确定";
            this.button_Confirm.UseVisualStyleBackColor = true;
            this.button_Confirm.Click += new System.EventHandler(this.button_Confirm_Click);
            // 
            // CreoConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 94);
            this.Controls.Add(this.button_Confirm);
            this.Controls.Add(this.textBox_CreoPath);
            this.Controls.Add(this.Creo软件路径);
            this.Controls.Add(this.button_Sel);
            this.Name = "CreoConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreoConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Sel;
        private System.Windows.Forms.Label Creo软件路径;
        private System.Windows.Forms.TextBox textBox_CreoPath;
        private System.Windows.Forms.Button button_Confirm;
    }
}