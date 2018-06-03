using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class Find : Form
    {
        public Direction Direction { get; set; }
        public Boolean matchCase { get; set; }
        public string findString { get; set; }

        //TODO: allow this control to perform a search of text in the Main form.
        public Find()
        {
            InitializeComponent();

            Direction = Direction.DOWN;
            radioButton2.Select();

            checkBox1.Enabled = false;
            matchCase = false;
        }

        private void Find_Load(object sender, EventArgs e)
        {
            if (findString != null)
            {
                textBox1.Text = findString;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Parent..findTextUp(findString);
            findString = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Direction = Direction.UP;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Direction = Direction.DOWN;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            matchCase = !matchCase;
        }
    }
}
