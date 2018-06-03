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
        private Boolean fileIO;

        private PageSetupDialog psd = new PageSetupDialog();
        private PrintDocument pdo = new PrintDocument();
        private PrintDialog ptd = new PrintDialog();

        public Form1()
        {
            InitializeComponent();
            fileIO = false;
            newFile();

            psd.Document = pdo;

            pdo.PrintPage += new PrintPageEventHandler(this.pdo_Print);
            //ptd.AllowSelection = false;
            //ptd.AllowSomePages = false;
        }

        private void pdo_Print(object sender, PrintPageEventArgs ppeArgs)
        {
            Graphics gobj = ppeArgs.Graphics;
            gobj.TextRenderingHint = TextRenderingHint.AntiAlias;
            gobj.DrawString(textBox1.Text, new Font("Monospace", 14), new SolidBrush(Color.Black), 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newFile()
        {
            textBox1.Clear();
            textBox1.ClearUndo();
            filePath = "";
            fileName = "untitled";
            pdo.DocumentName = fileName;
            this.Text = "Notepad - " + fileName + "*";
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
                fileIO = true;
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
                fileIO = true;
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
                fileIO = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //TODO: Find a way to add a star to title only when the user edits the text.
            if(textBox1.SelectionStart != 0)
            {
                char c = textBox1.Text[textBox1.SelectionStart - 1];
                if (c == 0x20 || c == 0x0A)
                {
                    textBox1.ClearUndo();
                }
            }
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
                //pdo.PrinterSettings = psd.PrinterSettings;
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

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size tempSize = textBox1.Size;
            if (statusStrip1.Visible)
            {
                tempSize.Height += 22;
                textBox1.Size = tempSize;
            }
            else
            {
                tempSize.Height -= 22;
                textBox1.Size = tempSize;
            }
            statusStrip1.Visible = !statusStrip1.Visible;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Make this go faster!!!
            DateTime currentTime = DateTime.Now;
            int currentPosition = textBox1.SelectionStart;
            string line = currentTime.ToString("h:mm tt, MMMM d, yyyy");
            string before = "";
            string after = "";
            for(int i = 0; i < currentPosition; i++)
            {
                before += textBox1.Text[i];
            }
            for(int i = currentPosition; i < textBox1.TextLength; i++)
            {
                after += textBox1.Text[i];
            }
            textBox1.Text = before + line + after;
            textBox1.SelectionStart = currentPosition;
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            //TODO: Make this automatically update. Add word count and character count.
            int currentIndex = textBox1.GetFirstCharIndexOfCurrentLine();
            int lineNum = textBox1.GetLineFromCharIndex(currentIndex);
            int colNum = textBox1.SelectionStart - currentIndex;
            int numChar = textBox1.TextLength;
            toolStripStatusLabel1.Text = "Line: " + lineNum + ", Col: " + colNum + ", Char: " + numChar;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Clear undo whenever a space or newline is entered.
            textBox1.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Make this go faster!!! Make it work with undo.
            int currentPosition = textBox1.SelectionStart;
            int length = textBox1.SelectionLength;
            string before = "";
            string after = "";
            for (int i = 0; i < currentPosition; i++)
            {
                before += textBox1.Text[i];
            }
            for (int i = currentPosition + length; i < textBox1.TextLength; i++)
            {
                after += textBox1.Text[i];
            }
            textBox1.Text = before + after;
            textBox1.SelectionStart = currentPosition;
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: I have decided whether to do this yet, but lines and columns start with zero
            // and maybe in the program one should be added to hide reality.
            GoTo gt = new GoTo();
            if(gt.ShowDialog() == DialogResult.OK)
            {
                textBox1.SelectionStart = textBox1.GetFirstCharIndexFromLine(gt.ReturnValue);
            }
            gt.Dispose();
        }
    }
}
