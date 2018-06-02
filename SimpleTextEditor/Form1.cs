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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
                
                /* sr.BaseStream.Position = 0x0C;
                byte[] buffer = new byte[3];
                sr.BaseStream.Read(buffer, 0, 3);
                foreach (byte myByte in buffer)
                {
                    textBox1.Text += myByte.ToString("X") + " ";
                }
                sr.Dispose(); */
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(File.Create(path));
            sw.Write(textBox1.Text);
            sw.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = sfd.FileName;
                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Write(textBox1.Text);
                sw.Dispose();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
