﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OddyseyUI
{
    public partial class Form4 : Form
    {
        public string name;
        public string author;
        public string album;
        public string score;


        public Form4()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox1.Text.Trim() == string.Empty || textBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("You must fill all the spaces");
            }
            else
            {
                name = textBox1.Text;
                author = textBox2.Text;
                album = textBox3.Text;
                score = textBox4.Text;

                Close();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
