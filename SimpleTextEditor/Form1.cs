using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SimpleTextEditor
{
    public partial class Form1 : Form
    {
        private string path;
        private Boolean fileOpen; //Not yet used. Will use an event to active buttons when this becomes true.

        public Form1()
        {
            InitializeComponent();
            this.Text = "Notepad";
            fileOpen = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            sfd.Title = "New";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button2.Enabled = true;
                path = sfd.FileName;
                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Dispose();
                textBox1.Text = "";
            }
        }

        private void openFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button2.Enabled = true;
                path = ofd.FileName;
                StreamReader sr = new StreamReader(path);
                textBox1.Text = sr.ReadToEnd();
                sr.Dispose();
            }
        }

        private void saveFile()
        {
            StreamWriter sw = new StreamWriter(File.Create(path));
            sw.Write(textBox1.Text);
            sw.Dispose();
        }

        private void saveAsFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button2.Enabled = true;
                path = sfd.FileName;
                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Write(textBox1.Text);
                sw.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }
    }
}
