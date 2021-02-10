using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DiplomaProject
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"./text/comboitem.txt";
            string TC = "";
            if (textBox1.Text == "") { MessageBox.Show("Введите название"); }
            else 
            {
             TC = textBox1.Text + "\r\n";
                File.AppendAllText(path, TC);
                File.Create("./text/TM" + textBox1.Text + ".txt");
             this.Close();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
