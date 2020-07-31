using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AppTools.autoupdate
{
    public partial class DownloadConfirm2 : Form
    {
        List<DownloadFileInfo> downloadFileList = null;

        public DownloadConfirm2(List<DownloadFileInfo> dfl)
        {
            InitializeComponent();

            downloadFileList = dfl;
        }

        private void DownloadConfirm2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //private void OnLoad(object sender, EventArgs e)
        //{
        //    foreach (DownloadFileInfo file in this.downloadFileList)
        //    {
        //        ListViewItem item = new ListViewItem(new string[] { file.FileName, file.LastVer, file.Size.ToString() });
        //        this.listDownloadFile.Items.Add(item);
        //    }

        //    this.Activate();
        //    this.Focus();
        //}
    }
}