using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SimpleTextEditor
{
    public partial class Form1 : Form
    {
        private string filePath;
        private string fileName;
        private Boolean fileOpen;

        private PageSetupDialog psd = new PageSetupDialog();
        private PrintDocument pdo = new PrintDocument();
        private PrintDialog ptd = new PrintDialog();

        public Form1()
        {
            InitializeComponent();
            filePath = "";
            fileName = "untitled";
            fileOpen = false;
            this.Text = "Notepad - " + fileName;

            psd.Document = pdo;
            pdo.DocumentName = fileName;
            pdo.PrintPage += new PrintPageEventHandler(this.pd_Print);
            ptd.AllowSelection = true;
            ptd.AllowSomePages = true;
        }

        private void pd_Print(object sender, PrintPageEventArgs ppeArgs)
        {
            DrawGraphicsItem(ppeArgs.Graphics);
        }

        private void DrawGraphicsItem(Graphics gobj)
        {
            gobj.TextRenderingHint = TextRenderingHint.AntiAlias;
            gobj.DrawString(textBox1.Text, new Font("Monospace", 14), new SolidBrush(Color.Blue), 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newFile()
        {
            textBox1.Text = "";
            filePath = "";
            fileName = "untitled";
            pdo.DocumentName = fileName;
            this.Text = "Notepad - " + fileName;
            fileOpen = false;
        }

        private void openFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileOpen = true;
                filePath = ofd.FileName;
                fileName = ofd.SafeFileName;
                pdo.DocumentName = fileName;
                this.Text = "Notepad - " + fileName;
                StreamReader sr = new StreamReader(filePath);
                textBox1.Text = sr.ReadToEnd();
                sr.Dispose();
            }
        }

        private void saveFile()
        {
            if (fileOpen == false)
            {
                saveAsFile();
                fileOpen = true;
            }
            else
            {
                StreamWriter sw = new StreamWriter(File.Create(filePath));
                sw.Write(textBox1.Text);
                sw.Dispose();
            }
        }

        private void saveAsFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = sfd.FileName;
                fileName = Path.GetFileName(filePath);
                pdo.DocumentName = fileName;
                this.Text = "Notepad - " + fileName;
                StreamWriter sw = new StreamWriter(File.Create(filePath));
                sw.Write(textBox1.Text);
                sw.Dispose();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void wordWrapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.WordWrap = !textBox1.WordWrap;
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(psd.ShowDialog() == DialogResult.OK)
            {
                pdo.DefaultPageSettings = psd.PageSettings;
                pdo.PrinterSettings = psd.PrinterSettings;
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ptd.ShowDialog() == DialogResult.OK)
            {
                pdo.Print();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
