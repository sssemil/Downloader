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
        private string filename;
        private void button1_Click_1(object sender, EventArgs e)
        {
            int xx = 1;
            button1.Enabled = false;
            bool fs = freespace(ln1);
            if (fs == true)
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please fill in all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button1.Enabled = true;
                    ReportStatus("Please fill in all fields!");
                    this.WindowState = FormWindowState.Minimized;
                    this.WindowState = FormWindowState.Normal;
                    Player.SoundLocation = "file:///C:/Windows/Media/Windows%20Notify%20System%20Generic.wav";
                    Player.Play();
                }
                else
                {
                    try
                    {
                        Uri uri = new Uri(textBox1.Text);
                        filename = System.IO.Path.GetFileName(uri.LocalPath);
                        string outf = textBox2.Text + "\\" + filename;
                        if (File.Exists(outf) == true)
                        {
                            DialogResult result = MessageBox.Show("File with the same name already exists, would you like to overwrite it?",
        "Warning.",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                            {
                                xx = 0;
                            }
                            else if (result == DialogResult.Yes)
                            {
                                xx = 1;
                            }

                        }
                        else{xx = 1;}
                        if (xx == 0)
                        {
                            ReportStatus("Canceled!");
                            button1.Enabled = true;
                        }
                        else if (xx == 1)
                        {
                            ReportStatus("Starting...");
                            webClient.DownloadFileAsync(new Uri(textBox1.Text), outf);
                        }
                    }
                    catch (Exception ex)
                    {
                        ReportStatus(ex.Message);
                        button1.Enabled = true;
                    }
                }
            }
            else if (fs == false)
            {
                ReportStatus("There is insufficient space at the chosen location!");
                this.WindowState = FormWindowState.Minimized;
                this.WindowState = FormWindowState.Normal;
                Player.SoundLocation = "file:///C:/Windows/Media/Windows%20Notify%20System%20Generic.wav";
                Player.Play();
                button1.Enabled = true;
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
        private string ln1;
        private string ln2;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini") == true)
            {
                using (StreamReader reader = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\downloader.ini"))
                {
                    ln1 = reader.ReadLine();
                    ln2 = reader.ReadLine();
                }
            }
            else
            {
                ln1 = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).FullName + "\\Downloads";
                ln2 = "Auto";
            }
            textBox2.Text = ln1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Net.WebRequest req = System.Net.HttpWebRequest.Create(textBox1.Text);
                req.Method = "HEAD";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    int ContentLength;
                    if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                    {
                        Uri uri = new Uri(textBox1.Text);
                        filename = System.IO.Path.GetFileName(uri.LocalPath);
                        if (ln2 == "Auto")
                        {
                            if (ContentLength >= 1000000000)
                            {
                                ln2 = "GB";
                            }
                            else if (ContentLength >= 1000000 && ContentLength <= 1000000000)
                            {
                                ln2 = "MB";
                            }
                            else if (ContentLength >= 1000 && ContentLength <= 1000000)
                            {
                                ln2 = "KB";
                            }
                            else
                            {
                                ln2 = "Bytes";
                            }
                        }
                        if (ln2 == "Bytes")
                        {
                            ReportStatus(ContentLength.ToString() + " Bytes - " + filename);
                        }
                        else if (ln2 == "KB")
                        {
                            ContentLength = ContentLength/1000;
                            ReportStatus(ContentLength.ToString() + " KB - " + filename);
                        }
                        else if (ln2 == "MB")
                        {
                            ContentLength = ContentLength/1000000;
                            ReportStatus(ContentLength.ToString() + " MB - " + filename);
                        }
                        else if (ln2 == "GB")
                        {
                            ContentLength = ContentLength/100000000;
                            ReportStatus(ContentLength.ToString() + " GB - " + filename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportStatus(ex.Message);
                button1.Enabled = true;
            }
        }

        private bool freespace(string path)
        {
            int ContentLength;
            long fs = 0;
            try
            {
                System.Net.WebRequest req = System.Net.HttpWebRequest.Create(textBox1.Text);
                req.Method = "HEAD";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                    {
                        fs = ContentLength;
                    }
                }
                var drive = new DriveInfo(Path.GetPathRoot(path));
                long freeSpaceInBytes = drive.AvailableFreeSpace;if (fs >= freeSpaceInBytes)
                {
                    return false;
                }
                else { return true; }
            }
            catch (Exception ex)
            {
                ReportStatus(ex.Message);
                return false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ln1 = textBox2.Text;
        }

        private void exityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
