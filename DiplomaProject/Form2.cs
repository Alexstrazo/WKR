using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace DiplomaProject
{
    public partial class Form2 : Form
    {
        int fontstylebold = 0;
        int fontstyleitalic = 0;
        int fontstyleunderline = 0;
        string font;
        float sizeStyle;
        int swindex = 0;
        OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = DPDataBase.accdb");


        public Form2()
        {
            InitializeComponent();
            this.topicsTableAdapter.Fill(this.dPDataBaseDataSet.Topics);
            comboBoxT.SelectedIndex = -1;
            Form Form1 = new Form1();
            if (listBox1.SelectedIndex == -1)
            {
               panel1.Visible = false;
            }
            panel2.Visible = false;
            panel4.Visible = false;
            openFileDialog1.Filter = "Картинки|*.jpeg;*.jpg;*.png;*.ico;*.bmp;*.emp;*..wmf;*.tiff";
            openFileDialog2.Filter = "Документы|*.rtf;";

            comboBoxFontSize.SelectedItem = "8";

           
         
            if (Directory.Exists(@"./text/LC")) { }
            else
            {
                Directory.CreateDirectory(@"./text/LC");
            }
                comboBoxFont.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxFont.DrawItem += comboBoxFont_DrawItem;
            
            foreach (FontFamily fontFamily in FontFamily.Families)
            {
                comboBoxFont.Items.Add(fontFamily.Name);
            }
            comboBoxFont.SelectedItem = "Arial";
        }

        private void comboBoxFont_DrawItem(object sender, DrawItemEventArgs e)
        {
            var fontFamily = (String)comboBoxFont.Items[e.Index];
            var font = new Font(fontFamily, comboBoxFont.Font.SizeInPoints);
            e.DrawBackground();
            string FontName = comboBoxFont.Items[e.Index].ToString();
            e.Graphics.DrawString(font.Name, font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }

        private void Form2_FormClosed(Object sender, FormClosedEventArgs e)
        {
            Form Form1 = new Form1();
            Form1.Show();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {  }
            else
            {
                panel1.Visible = true;
                string n = listBox1.SelectedItem.ToString();
                if (File.Exists(@"./text/LC/" + n + ".rtf"))
                {
                    richTextBox1.LoadFile(@"./text/LC/" + n + ".rtf");

                }
                else
                {
                    richTextBox1.Clear();
                    panel1.Visible = false;
                    MessageBox.Show("Файл отсутствует");
                }
            }
        }

        private bool CheckIfImage(string filename)
        {
            if (filename.EndsWith(".jpeg")) { return true; }
            else if (filename.EndsWith(".jpg")) { return true; }
            else if (filename.EndsWith(".png")) { return true; }
            else if (filename.EndsWith(".ico")) { return true; }
            else if (filename.EndsWith(".bmp")) { return true; }
            else if (filename.EndsWith(".emp")) { return true; }
            else if (filename.EndsWith(".wmf")) { return true; }
            else if (filename.EndsWith(".tiff")) { return true; }
            else { return false; }
        }
        private bool CheckIfDocument(string filename)
        {
            if (filename.EndsWith(".rtf")) { return true; }
            
            else { return false; }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (CheckIfImage(openFileDialog1.FileName.ToLower()) == true)
            {
                
                Image img = Image.FromFile(openFileDialog1.FileName);
                string setData = (String)Clipboard.GetData(DataFormats.Rtf);

                Clipboard.SetImage(img);
                richTextBox1.Paste();

                Clipboard.SetData(DataFormats.Rtf, setData);
            }
            else
            {
                MessageBox.Show("Неверный формат файла");
            }
        }
        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            if (CheckIfDocument(openFileDialog2.FileName.ToLower()) == true)
            {
               
            }
            else
            {
                MessageBox.Show("Неверный формат файла");
            }
        }

        private void изображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) ;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { }
            else
            {
                string n = listBox1.SelectedItem.ToString();
                richTextBox1.SaveFile("./text/LC/" + n + ".rtf");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
             font = (String)(comboBoxFont.SelectedItem);
            Int32 size = Convert.ToInt32(comboBoxFontSize.Text);
            
            richTextBox1.SelectionFont = new Font(font, size);
            fontstyle();
            comboBoxFont.Font = new Font(font, comboBoxFont.Font.SizeInPoints);
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                label3.Text = comboBoxT.Text;
                string cmdstr = "SELECT Lecture FROM Lectures WHERE Topic = '" + comboBoxT.Text + "'";
                OleDbCommand command = new OleDbCommand(cmdstr, conn);
                IList<string> lc = new List<string>();
                conn.Open();
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lc.Add(reader[0].ToString());
                }
                conn.Close();
                listBox1.DataSource = lc;
                listBox1.DisplayMember = "Lecture";
                panel1.Visible = false;
                listBox1.SelectedIndex = -1;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 size = Convert.ToInt32(comboBoxFontSize.Text);

            richTextBox1.SelectionFont = new Font(font, size);
            sizeStyle = float.Parse(comboBoxFontSize.Text);
            fontstyle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (fontstylebold == 0)
            {
                fontstylebold = 1;
                fontstyle();
            }
            else
            {
                fontstylebold = 0;
                fontstyle();
            }
                        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (fontstyleitalic == 0)
            {
                fontstyleitalic = 1;
                fontstyle();
            }
            else
            {
                fontstyleitalic = 0;
                fontstyle();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (fontstyleunderline == 0)
            {
                fontstyleunderline = 1;
                fontstyle();
            }
            else
            {
                fontstyleunderline = 0;
                fontstyle();
            }
        }

        private void fontstyle()
        {
            int caseswith = 0;

            if (fontstylebold == 1)
            {
                caseswith += 1;
            }
            if (fontstyleitalic == 1)
            {
                caseswith += 3;
            }
            if (fontstyleunderline == 1)
            {
                caseswith += 5;
            }

            switch(caseswith)
            {
                case 1:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Bold);
                    break;
                case 3:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Italic);
                    break;
                case 4:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Bold|FontStyle.Italic);
                    break;
                case 5:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Underline);
                    break;
                case 6:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Bold|FontStyle.Underline);
                    break;
                case 8:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Italic|FontStyle.Underline);
                    break;
                case 9:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Bold|FontStyle.Italic|FontStyle.Underline);
                    break;
                default:
                    richTextBox1.SelectionFont = new Font(font, sizeStyle, FontStyle.Regular);
                    break;
            }
               
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent += 8;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent -= 8;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            richTextBox1.SelectionColor = colorDialog1.Color;
        }

        private void темуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            textBox1.Clear();
            panel2.Visible = true;
            swindex = 1;
        }

        private void createitem(int swithindex)
        {
            switch (swithindex)
            {
                case 1:
                    {
                        if (textBox1.Text == "") { MessageBox.Show("Введите название"); }
                        else
                        {
                            string cmdstr = "SELECT COUNT(*) FROM Topics";
                            OleDbCommand command = new OleDbCommand(cmdstr,conn);
                            conn.Open();
                            int count = (int)command.ExecuteScalar();
                            conn.Close();
                            DPDataBaseDataSetTableAdapters.TopicsTableAdapter topicsTableAdapter = new DPDataBaseDataSetTableAdapters.TopicsTableAdapter();
                            topicsTableAdapter.Insert(count,textBox1.Text);
                            this.topicsTableAdapter.Fill(this.dPDataBaseDataSet.Topics);
                            string Ttext = textBox1.Text;
                            comboBoxT.SelectedIndex = count;

                        }
                       
                    }
                    break;
                case 2:
                        if (textBox1.Text == "") { MessageBox.Show("Введите название"); }
                    else
                    {
                        string cmdstr = "SELECT COUNT(*) FROM Lectures";
                        string type = "Lecture";
                        OleDbCommand command = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        int count = (int)command.ExecuteScalar();
                        conn.Close();
                        DPDataBaseDataSet1TableAdapters.LecturesTableAdapter lecturesTableAdapter = new DPDataBaseDataSet1TableAdapters.LecturesTableAdapter();
                        lecturesTableAdapter.Insert(count, textBox1.Text, comboBoxT.Text,type);
                            File.AppendAllText("./text/LC/" + textBox1.Text + ".rtf", @"{\rtf}");
                        cmdstr = "SELECT Lecture FROM Lectures WHERE Topic = '" + comboBoxT.Text + "'";
                        command = new OleDbCommand(cmdstr, conn);
                        IList<string> lc = new List<string>();
                        conn.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            lc.Add(reader[0].ToString());
                        }
                        conn.Close();
                        listBox1.DataSource = lc;
                        listBox1.DisplayMember = "Lecture";
                        listBox1.SelectedItem = textBox1.Text;
                    }
                    this.lecturesTableAdapter.Fill(this.dPDataBaseDataSet1.Lectures);
                    break;
                case 3:
                    if (textBox1.Text == "") { MessageBox.Show("Введите название"); }
                    else
                    {
                        string cmdstr = "SELECT COUNT(*) FROM Lectures";
                        string type = "Lecture";
                        OleDbCommand command = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        int count = (int)command.ExecuteScalar();
                        conn.Close();
                        DPDataBaseDataSet1TableAdapters.LecturesTableAdapter lecturesTableAdapter = new DPDataBaseDataSet1TableAdapters.LecturesTableAdapter();
                        lecturesTableAdapter.Insert(count, textBox1.Text, comboBoxT.Text,type);
                        richTextBox1.SaveFile("./text/LC/" + textBox1.Text + ".rtf");
                        richTextBox1.Clear();
                        this.lecturesTableAdapter.Fill(this.dPDataBaseDataSet1.Lectures);
                        cmdstr = "SELECT Lecture FROM Lectures WHERE Topic = '" + comboBoxT.Text + "'";
                        command = new OleDbCommand(cmdstr, conn);
                        IList<string> lc = new List<string>();
                        conn.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            lc.Add(reader[0].ToString());
                        }
                        conn.Close();
                        listBox1.DataSource = lc;
                        listBox1.DisplayMember = "Lecture";
                        listBox1.SelectedItem = textBox1.Text;
                    }
                    break;
            }
        
        }

        private void button12_Click(object sender, EventArgs e)
        {

            createitem(swindex);
            panel2.Visible = false;
            if (listBox1.SelectedIndex == -1) { }
                else { panel1.Visible = true; }

        }

        private void новуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                if (Directory.Exists(@"./text/LC")) { }
                else
                {
                    Directory.CreateDirectory(@"./text/LC");
                }
                if (panel1.Visible == true)
                {
                    panel1.Visible = false;
                }
                textBox1.Clear();
                panel2.Visible = true;
                swindex = 2;
            }
            else
            {
                MessageBox.Show("Выберите или создайте тему");
            }
        }

        private void изФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.LoadFile(openFileDialog2.FileName);
                    if (Directory.Exists(@"./text/LC")) { }
                    else
                    {
                        Directory.CreateDirectory(@"./text/LC");
                    }
                    if (panel1.Visible == true)
                    {
                        panel1.Visible = false;
                    }
                    textBox1.Clear();
                    panel2.Visible = true;
                    swindex = 3;
                }
            }
            else
            {
                MessageBox.Show("Выберите или создайте тему");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void тестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
        }

    }
}
