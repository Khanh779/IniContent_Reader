using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IniContent_Reader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fileReader = new IniContent_Lib.IniContentReader();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProcessIniContent(richTextBox1.Text);
        }


        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "INI files|*.ini|All files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                richTextBox1.Text = File.ReadAllText(filePath);
            }
        }

        IniContent_Lib.IniContentReader fileReader;

        private void buttonProcessFile_Click(object sender, EventArgs e)
        {
            ProcessIniContent(richTextBox1.Text);
        }

        private void ProcessIniContent(string iniContent)
        {
            var data = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
          

            fileReader.LoadContent(iniContent);

            foreach(string section in fileReader.GetSections())
            {
                data[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string key in fileReader.GetKeys(section))
                {
                    data[section][key] = fileReader.GetValue(section, key);
                }
            }


            PopulateTreeView(data);
        }

        private void PopulateTreeView(Dictionary<string, Dictionary<string, string>> data)
        {
            treeView1.Nodes.Clear();
            foreach (var section in data)
            {
                TreeNode sectionNode = new TreeNode(section.Key +" <sector>");
                foreach (var keyValue in section.Value)
                {
                    TreeNode keyNode = new TreeNode(keyValue.Key +" <property>");
                    TreeNode valueNode = new TreeNode(keyValue.Value + " <value>");
                    keyNode.Nodes.Add(valueNode);
                    sectionNode.Nodes.Add(keyNode);
                }
                treeView1.Nodes.Add(sectionNode);
            }
          
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            ProcessIniContent(richTextBox1.Text);
           
        }
    }
}
