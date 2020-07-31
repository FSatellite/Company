using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MessagePrinter
{
    public partial class MessagePrinter : UserControl
    {
        private  MessagePrinter()
        {
            InitializeComponent();
            this.richTextBox1.Text = "->ToolResolver 初始化"+System.DateTime .Now ;
           // Control.CheckForIllegalCrossThreadCalls = false;
        }
        public void PrintMessage(string content)

       {
           try
           {
               if (this.richTextBox1.TextLength > 1000)
               {
                   this.richTextBox1.Clear();
               }
               this.richTextBox1.AppendText("->" + "\r\n" + content);
               this.richTextBox1.ScrollToCaret();
           }
           catch (Exception ee)
           {

           }
       }
        public void Clear()
        {
            try
            {
                this.richTextBox1.Clear();
                this.richTextBox1.ScrollToCaret();
            }
            catch (Exception ee)
            {

            }
        }
      
        private static MessagePrinter instance;
        public static MessagePrinter GetInstance()
        {
            if (instance == null)
            {
                instance = new MessagePrinter();
            }
            return instance;
        }
    }
}
