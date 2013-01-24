using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace xml
{
    public partial class Form1 : Form
    {
        public string xmlFileName = null;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            xmlFileName = openFileDialog1.FileName;
        }
    }
}
