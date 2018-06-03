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
    public enum Direction
    {
        UP,
        DOWN
    }

    //TODO: Fix print so that it prints multiple pages.
    public partial class Main : Form
    {
        private string filePath;
        private string fileName;
        private Boolean fileOpen;

        private string findString;
        public Direction Direction { get; set; }
        private Boolean matchCase;

        private PageSetupDialog psd = new PageSetupDialog();
        private PrintDocument pdo = new PrintDocument();
        private PrintDialog ptd = new PrintDialog();

        public Main()
        {
            InitializeComponent();

            newFile();

            findString = "";

            psd.Document = pdo;

            //findToolStripMenuItem.Enabled = false;
            replaceToolStripMenuItem.Enabled = false;
            fontToolStripMenuItem.Enabled = false;
            statusBarToolStripMenuItem.Checked = true;

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
            fileOpen = false;
            textBox1.Modified = false;
            this.Text = "TextEditor - " + fileName + "*";
            toolStripStatusLabel1.Text = "Line: " + 0 + ", Col: " + 0 + ", Char: " + 0 + " | Lines: " + 0 + " Chars: " + 0;
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
                StreamReader sr = new StreamReader(filePath);
                textBox1.Text = sr.ReadToEnd();
                sr.Dispose();
                textBox1.Modified = false;
                this.Text = "TextEditor - " + fileName;
            }
        }

        private void saveFile()
        {
            if (fileOpen == false)
            {
                saveAsFile();
            }
            else
            {
                StreamWriter sw = new StreamWriter(File.Create(filePath));
                sw.Write(textBox1.Text);
                sw.Dispose();
                textBox1.Modified = false;
                this.Text = "TextEditor - " + fileName;
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
                StreamWriter sw = new StreamWriter(File.Create(filePath));
                sw.Write(textBox1.Text);
                sw.Dispose();
                textBox1.Modified = false;
                this.Text = "TextEditor - " + fileName;
                fileOpen = true;
            }
        }

        private void updateStatusBar()
        {
            int currentIndex = textBox1.GetFirstCharIndexOfCurrentLine();
            int lineNum = textBox1.GetLineFromCharIndex(currentIndex);
            int colNum = textBox1.SelectionStart - currentIndex;
            int currentChar = colNum + currentIndex;
            int numChar = textBox1.TextLength + 1;
            int numLines = textBox1.GetLineFromCharIndex(numChar) + 1;
            toolStripStatusLabel1.Text = "Line: " + lineNum + ", Col: " + colNum + ", Char: " + currentChar + " | Lines: " + numLines + " Chars: " + numChar;
        }

        public void findTextUp(string query)
        {
            //TODO: Find a non-insane way to do this.
            int currentPosition = textBox1.SelectionStart;
            int length = textBox1.SelectionLength;
            if (textBox1.Text == "" || textBox1.Text == null || currentPosition == textBox1.TextLength)
            {
                MessageBox.Show("Could not find " + "\"" + query + "\"", "Find");
                textBox1.ScrollToCaret();
                updateStatusBar();
                return;
            }

            char c = textBox1.Text[currentPosition];
            int i = length - 1;
            do
            {
                if (query[i].ToString() == c.ToString())
                {
                    i--;
                }
                else
                {
                    i = length - 1;
                }
                if (currentPosition != textBox1.TextLength)
                {
                    c = textBox1.Text[currentPosition];
                }
            }
            while (currentPosition-- > 0 && i > 0);

            if (i == 0)
            {
                textBox1.SelectionStart = currentPosition - 1;
                textBox1.SelectionLength = length;
            }
            else
            {
                MessageBox.Show("Could not find " + "\"" + query + "\"", "Find");
            }
            textBox1.ScrollToCaret();
            updateStatusBar();
        }

        public void findTextDown(string query)
        {
            //TODO: Find a non-insane way to do this.
            int currentPosition = textBox1.SelectionStart;
            int length = textBox1.SelectionLength;
            if(textBox1.Text == "" || textBox1.Text == null || currentPosition == textBox1.TextLength)
            {
                MessageBox.Show("Could not find " + "\"" + query + "\"", "Find");
                textBox1.ScrollToCaret();
                updateStatusBar();
                return;
            }

            char c = textBox1.Text[currentPosition];
            int i = 0;
            do
            {
                if (query[i].ToString() == c.ToString())
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
                if (currentPosition != textBox1.TextLength)
                {
                    c = textBox1.Text[currentPosition];
                }
            }
            while (currentPosition++ < textBox1.TextLength && i < query.Length);
            
            if (i == query.Length)
            {
                textBox1.SelectionStart = currentPosition - i - 1;
                textBox1.SelectionLength = i;
            }
            else
            {
                MessageBox.Show("Could not find " + "\"" + query + "\"", "Find");
            }
            textBox1.ScrollToCaret();
            updateStatusBar();
        }

        private void timeDate()
        {
            //TODO: Make this go faster!!!
            DateTime currentTime = DateTime.Now;
            int currentPosition = textBox1.SelectionStart;
            string line = currentTime.ToString("h:mm tt, MMMM d, yyyy");
            string before = "";
            string after = "";
            for (int i = 0; i < currentPosition; i++)
            {
                before += textBox1.Text[i];
            }
            for (int i = currentPosition; i < textBox1.TextLength; i++)
            {
                after += textBox1.Text[i];
            }
            textBox1.Text = before + line + after;
            textBox1.SelectionStart = currentPosition;
            textBox1.ScrollToCaret();
            updateStatusBar();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = "TextEditor - " + fileName + "*";
            if (textBox1.SelectionStart != 0)
            {
                char c = textBox1.Text[textBox1.SelectionStart - 1];
                if (c == 0x20 || c == 0x0A)
                {
                    textBox1.ClearUndo();
                }
            }
            updateStatusBar();
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
            wordWrapToolStripMenuItem1.Checked = !wordWrapToolStripMenuItem1.Checked;
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
            statusBarToolStripMenuItem.Checked = !statusBarToolStripMenuItem.Checked;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timeDate();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            updateStatusBar();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            textBox1.ScrollToCaret();
            updateStatusBar();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoTo gt = new GoTo();
            if (gt.ShowDialog() == DialogResult.OK)
            {
                textBox1.SelectionStart = textBox1.GetFirstCharIndexFromLine(gt.ReturnValue);
                textBox1.ScrollToCaret();
                updateStatusBar();
            }
            gt.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
            about.Dispose();  
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find find = new Find();
            find.findString = findString;
            if(find.ShowDialog() == DialogResult.Cancel)
            {
                findString = find.findString;
                Direction = find.Direction;
                matchCase = find.matchCase;
            }
            find.Dispose();
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(findString == "" || findString == null)
            {
                Find find = new Find();
                find.findString = "";
                if (find.ShowDialog() == DialogResult.Cancel)
                {
                    findString = find.findString;
                    Direction = find.Direction;
                    matchCase = find.matchCase;
                }
                find.Dispose();
            }
            else
            {
                if(this.Direction == Direction.DOWN)
                {
                    findTextDown(findString);
                }
                else
                {
                    findTextUp(findString);
                }
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Replace replace = new Replace();
            replace.ShowDialog();
            replace.Dispose();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            updateStatusBar();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            updateStatusBar();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            /* if (Control.ModifierKeys.HasFlag(Keys.Control))
             {
                 switch (e.KeyCode)
                 {
                     case Keys.N:
                         newFile();
                         break;
                     case Keys.O:
                         openFile();
                         break;
                     case Keys.S:
                         saveFile();
                         break;
                     case Keys.P:
                         if (ptd.ShowDialog() == DialogResult.OK)
                         {
                             pdo.Print();
                         }
                         break;
                     case Keys.F:
                         textBox1.Undo();
                         break;
                     case Keys.H:
                         break;
                     case Keys.G:
                         GoTo gt = new GoTo();
                         if (gt.ShowDialog() == DialogResult.OK)
                         {
                             textBox1.SelectionStart = textBox1.GetFirstCharIndexFromLine(gt.ReturnValue);
                             textBox1.ScrollToCaret();
                             updateStatusBar();
                         }
                         gt.Dispose();
                         break;
                     case Keys.A:
                         textBox1.SelectAll();
                         break;
                     default:
                         break;
                 }
             }
             switch (e.KeyCode)
             {
                 case Keys.F3:
                     if (findString == "" || findString == null)
                     {
                         Find find = new Find();
                         find.findString = "";
                         if (find.ShowDialog() == DialogResult.Cancel)
                         {
                             findString = find.findString;
                             Direction = find.Direction;
                             matchCase = find.matchCase;
                         }
                         find.Dispose();
                     }
                     else
                     {
                         if (this.Direction == Direction.DOWN)
                         {
                             findTextDown(findString);
                         }
                         else
                         {
                             findTextUp(findString);
                         }
                     }
                     break;
                 case Keys.F5:
                     timeDate();
                     break;
                 default:
                     break;
             }*/
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            

            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}
