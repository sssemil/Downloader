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
using System.Net;
using System.Reflection;
using System.Security.Permissions;
using System.Diagnostics;
using System.Media;

namespace Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public WebClient webClient = new WebClient();

        private void ReportStatus(string statusMessage)
        {
            // If the caller passed in a message... 
            if ((statusMessage != null) && (statusMessage != String.Empty))
            {
                // ...post the caller's message to the status bar. 
                toolStripStatusLabel1.Text = statusMessage;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = false;
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please fill in all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
                ReportStatus("Please fill in all fields!");
            }
            else
            {
                try
                {
                    Uri uri = new Uri(textBox1.Text);
                    string filename = System.IO.Path.GetFileName(uri.LocalPath);
                    string outf = textBox2.Text + "\\" + filename; 
                    ReportStatus("Starting...");
                    webClient.DownloadFileAsync(new Uri(textBox1.Text), outf);
                }
                catch(Exception ex)
                {
                    ReportStatus(ex.Message);
                    button1.Enabled = true;
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label4.Text = e.ProgressPercentage + "%";
            ReportStatus("Downloading..." + e.ProgressPercentage + "%");
        }

        private SoundPlayer Player = new SoundPlayer();

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = 0;
            label4.Text = "0%";
            button1.Enabled = true;
            ReportStatus("Done!");
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
            Player.SoundLocation = "file:///C:/Windows/Media/Windows%20Notify%20System%20Generic.wav";
            Player.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dlg.SelectedPath;
                }
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            MessageBox.Show(" Desined and created by sssemil inc.©\n Downloader by sssemil: " + version);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webClient.CancelAsync();
            progressBar1.Value = 0;
            ReportStatus("Stoped!");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 mdiChild = new Form2();
            mdiChild.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini") == true)
            {
                using (StreamReader reader = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini"))
                {
                    textBox2.Text = reader.ReadLine();
                }
            }
        }
    }
}
