using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Xml;

namespace xml
{
    public partial class Form1 : Form
    {       
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
            isTargetFile(openFileDialog1.FileName);
        }

        private bool isGrouped(string id) 
        {
            foreach (ListViewGroup group in listView1.Groups)
            {
                if (group.Name == id) return true;
            }
            return false;
        }

        private bool isTargetFile(string filename)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(filename);

                XmlElement root = doc.DocumentElement;
                if( !root.Name.Equals("table") ) return false;

                XmlNode title_row = root.FirstChild;    // 標題行
                if( !title_row.Name.Equals("row") ) return false;

                XmlNodeList titles = title_row.ChildNodes;    // 標題的項目
                if( titles.Count != 11 ) return false;
                if( !titles.Item(0).Name.Equals("column") ) return false;

                // 插入標題
                listView1.Columns.Clear();
                for (int i = 1; i < 11; ++i)
                {
                    listView1.Columns.Add(titles.Item(i).InnerText);
                }

                // 插入每一行
                listView1.Items.Clear();
                XmlNodeList entries = root.ChildNodes;    // 很多行
                for (int i = 1; i < entries.Count; ++i)
                {
                    XmlNode one_row = entries.Item(i);    // 一行項目
                    XmlNodeList row_entries = one_row.ChildNodes;    // 一行中的很多項目

                    ListViewItem it = new ListViewItem();
                    it.Text = row_entries.Item(1).InnerText.Trim();  // 流水號
                    for (int j = 2; j < 11; ++j)
                    {
                        it.SubItems.Add(row_entries.Item(j).InnerText.Trim());   // 後面剩下的項目
                    }
                    
                    // 依照流水號分組
                    if ( !isGrouped(it.Text) ) listView1.Groups.Add(it.Text, it.Text);
                    foreach (ListViewGroup group in listView1.Groups)
                    {
                        if (group.Name == it.Text)
                        {
                            it.Group = group;
                            break;
                        }
                    }
                    
                    listView1.Items.Add(it);   // 插入這行
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "錯誤Orz", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            string id = e.Item.Group.Name;
            bool check = e.Item.Checked;

            foreach (ListViewItem item in e.Item.Group.Items)
            {
                item.Checked = check;
            }
        }
    }
}
