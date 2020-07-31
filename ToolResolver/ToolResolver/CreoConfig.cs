using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ToolResolver
{
    public partial class CreoConfig : Form
    {
        public CreoConfig()
        {
            InitializeComponent();
        }

        private void button_Sel_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择Creo启动文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_CreoPath.Text = dialog.FileName;
            }
        }

        private void button_Confirm_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\CreoConfig.txt"))
            {
                File.Delete(Application.StartupPath + "\\CreoConfig.txt");
                FileStream fs = new FileStream(Application.StartupPath + "\\CreoConfig.txt",FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(this.textBox_CreoPath.Text);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\CreoConfig.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(this.textBox_CreoPath.Text);
                sw.Close();
                fs.Close();
            }

            this.Close();
        }
    }
}
