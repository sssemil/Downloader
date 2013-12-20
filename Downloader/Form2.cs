using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Downloader
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).FullName + "\\Downloads";
            }
            StreamWriter sw = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini");
            sw.WriteLine(textBox1.Text);
            sw.WriteLine(comboBox1.Text);
            sw.Close();
            MessageBox.Show("Saved!");
            Application.Restart();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dlg.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini");
            MessageBox.Show("Reset finished!");
            Application.Restart();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 fh = new Form2();
            fh.Hide();
            this.Hide();
        }

        private string ln1;
        private string ln2;
        private void Form2_Load(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini") == true)
            {
                using (StreamReader reader = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini"))
                {
                    ln1 = reader.ReadLine();
                    ln2 = reader.ReadLine();
                }
            }
            textBox1.Text = ln1;
            comboBox1.Text = ln2;
        }
    }
}
