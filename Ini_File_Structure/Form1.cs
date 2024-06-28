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

namespace Ini_File_Structure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void buttonProcessFile_Click(object sender, EventArgs e)
        {
            //ProcessIniContent(richTextBox1.Text);
        }

        private void ProcessIniContent(string iniContent)
        {
            var data = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            string currentSection = null;
            var lines = iniContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";"))
                    continue;

                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    if (!data.ContainsKey(currentSection))
                        data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                else if (trimmedLine.Contains("="))
                {
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    if (currentSection != null)
                        data[currentSection][key] = value;
                }
            }

            PopulateTreeView(data);
        }

        private void PopulateTreeView(Dictionary<string, Dictionary<string, string>> data)
        {
            treeView1.Nodes.Clear();
            foreach (var section in data)
            {
                TreeNode sectionNode = new TreeNode(section.Key);
                foreach (var keyValue in section.Value)
                {
                    TreeNode keyNode = new TreeNode(keyValue.Key);
                    TreeNode valueNode = new TreeNode(keyValue.Value);
                    keyNode.Nodes.Add(valueNode);
                    sectionNode.Nodes.Add(keyNode);
                }
                treeView1.Nodes.Add(sectionNode);
            }
            //treeView1.ExpandAll();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            ProcessIniContent(richTextBox1.Text);
        }
    }
}
