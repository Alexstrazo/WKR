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
        int questionsave=0;
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
            panel5.Visible = false;
            сохранитьToolStripMenuItem.Visible = false;
            вставитьToolStripMenuItem.Visible = false;
            openFileDialog1.Filter = "Картинки|*.jpeg;*.jpg;*.png;*.ico;*.bmp;*.emp;*..wmf;*.tiff";
            openFileDialog2.Filter = "Документы|*.rtf;";

            comboBoxFontSize.SelectedItem = "8";
           
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
            {
                panel1.Visible = false;
                panel4.Visible = false;
            }
            else
            {
                string cmdstr = "SELECT Type FROM Lectures WHERE Lecture = '" + listBox1.SelectedItem.ToString() + "'";
                OleDbCommand command = new OleDbCommand(cmdstr, conn);
                conn.Open();
                string listtype = command.ExecuteScalar().ToString();
                conn.Close();
                if (listtype == "Lecture")
                {
                    panel1.Visible = true;
                    сохранитьToolStripMenuItem.Visible = true;
                    вставитьToolStripMenuItem.Visible = true;
                    panel4.Visible = false;
                    string n = listBox1.SelectedItem.ToString();
                    if (File.Exists("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem + ".rtf"))
                    {
                        richTextBox1.LoadFile("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem + ".rtf");

                    }
                    else
                    {
                        richTextBox1.Clear();
                        panel1.Visible = false;
                        сохранитьToolStripMenuItem.Visible = false;
                        вставитьToolStripMenuItem.Visible = false;
                        MessageBox.Show("Файл отсутствует", "Ошибка");
                    }
                }
                else
                {
                    testload();
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
                MessageBox.Show("Неверный формат файла","Ошибка");
            }
        }
        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            if (CheckIfDocument(openFileDialog2.FileName.ToLower()) == true)
            {
               
            }
            else
            {
                MessageBox.Show("Неверный формат файла","Ошибка");
            }
        }

        private void изображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) ;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                if (listBox1.SelectedIndex == -1) { }
                else
                {
                    string n = listBox1.SelectedItem.ToString();
                    richTextBox1.SaveFile("./text/" + comboBoxT.Text + "/" + n + ".rtf");
                }
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
                loadlecture();
            }
        }

        private void loadlecture()
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
            сохранитьToolStripMenuItem.Visible = false;
            вставитьToolStripMenuItem.Visible = false;
            listBox1.SelectedIndex = -1;
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
                сохранитьToolStripMenuItem.Visible = false;
                вставитьToolStripMenuItem.Visible = false;
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
                        if (textBox1.Text == "") { MessageBox.Show("Введите название", "Создание темы"); }
                        else
                        {
                            string cmdstr = "SELECT COUNT(*) FROM Topics";
                            OleDbCommand command = new OleDbCommand(cmdstr,conn);
                            conn.Open();
                            int count = (int)command.ExecuteScalar();
                            conn.Close();
                            DPDataBaseDataSetTableAdapters.TopicsTableAdapter topicsTableAdapter = new DPDataBaseDataSetTableAdapters.TopicsTableAdapter();
                            topicsTableAdapter.Insert(textBox1.Text);
                            this.topicsTableAdapter.Fill(this.dPDataBaseDataSet.Topics);
                            comboBoxT.SelectedIndex = count;
                            if (Directory.Exists("./text/" + textBox1.Text)) { }
                            else
                            {
                                Directory.CreateDirectory("./text/" + textBox1.Text);
                            }

                        }
                       
                    }
                    break;
                case 2:
                        if (textBox1.Text == "") { MessageBox.Show("Введите название","Создание лекции"); }
                    else
                    {
                       
                        string cmdstr = "SELECT COUNT(*) FROM Lectures";
                        string type = "Lecture";
                        OleDbCommand command = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        int count = (int)command.ExecuteScalar();
                        conn.Close();
                        DPDataBaseDataSet1TableAdapters.LecturesTableAdapter lecturesTableAdapter = new DPDataBaseDataSet1TableAdapters.LecturesTableAdapter();
                        lecturesTableAdapter.Insert(textBox1.Text, comboBoxT.Text,type);
                            File.AppendAllText("./text/"+comboBoxT.Text+"/" + textBox1.Text + ".rtf", @"{\rtf}");
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
                    if (textBox1.Text == "") { MessageBox.Show("Введите название", "Создание лекции"); }
                    else
                    {
                        string cmdstr = "SELECT COUNT(*) FROM Lectures";
                        string type = "Lecture";
                        OleDbCommand command = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        int count = (int)command.ExecuteScalar();
                        conn.Close();
                        DPDataBaseDataSet1TableAdapters.LecturesTableAdapter lecturesTableAdapter = new DPDataBaseDataSet1TableAdapters.LecturesTableAdapter();
                        lecturesTableAdapter.Insert(textBox1.Text, comboBoxT.Text,type);
                        richTextBox1.SaveFile("./text/" + comboBoxT.Text + "/" + textBox1.Text + ".rtf");
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
                case 4:
                    if (textBox1.Text == "") { MessageBox.Show("Введите название", "Создание теста"); }
                    else
                    {
                        string cmdstr = "SELECT COUNT(*) FROM Lectures";
                        string type = "Test";
                        OleDbCommand command = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        int count = (int)command.ExecuteScalar();
                        conn.Close();
                        DPDataBaseDataSet1TableAdapters.LecturesTableAdapter lecturesTableAdapter = new DPDataBaseDataSet1TableAdapters.LecturesTableAdapter();
                        lecturesTableAdapter.Insert(textBox1.Text, comboBoxT.Text, type);
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
                        
                    }
                    this.lecturesTableAdapter.Fill(this.dPDataBaseDataSet1.Lectures);
                    listBox1.SelectedItem = textBox1.Text;
                    break;
            }
        
        }

        private void button12_Click(object sender, EventArgs e)
        {

            createitem(swindex);
            panel2.Visible = false;
            if (listBox1.SelectedIndex == -1) { }
                else
            {
                panel1.Visible = true;
                сохранитьToolStripMenuItem.Visible = true;
                вставитьToolStripMenuItem.Visible = true;
            }

        }

        private void новуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                if (panel1.Visible == true)
                {
                    panel1.Visible = false;
                    сохранитьToolStripMenuItem.Visible = false;
                    вставитьToolStripMenuItem.Visible = false;
                }
                textBox1.Clear();
                panel2.Visible = true;
                swindex = 2;
            }
            else
            {
                MessageBox.Show("Выберите или создайте тему", "Создание лекции");
            }
        }

        private void изФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.LoadFile(openFileDialog2.FileName);
                    if (panel1.Visible == true)
                    {
                        panel1.Visible = false;
                        сохранитьToolStripMenuItem.Visible = false;
                        вставитьToolStripMenuItem.Visible = false;
                    }
                    textBox1.Clear();
                    panel2.Visible = true;
                    swindex = 3;
                }
            }
            else
            {
                MessageBox.Show("Выберите или создайте тему","Создание лекции");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            int select = listBox1.SelectedIndex;
            listBox1.SelectedIndex = -1;
            listBox1.SelectedIndex = select;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cmdstr = "SELECT Type FROM Lectures WHERE Lecture = '" + listBox1.SelectedItem + "'";
            OleDbCommand command = new OleDbCommand(cmdstr, conn);
            conn.Open();
            string type = (string)command.ExecuteScalar();
            conn.Close();
            if (listBox1.SelectedIndex != -1)
            {
                if (type == "Lecture")
                {
                    File.Delete("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem + ".rtf");
                    cmdstr = "DELETE Lecture FROM Lectures WHERE Lecture ='" + listBox1.SelectedItem + "'";
                    OleDbCommand lecturesdelete = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    lecturesdelete.ExecuteNonQuery();
                    conn.Close();
                }
                if (type == "Test")
                {
                    cmdstr = "DELETE question FROM Tests WHERE Test ='" + listBox1.SelectedItem + "'";
                    OleDbCommand testsdelete = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    testsdelete.ExecuteNonQuery();
                    conn.Close();
                    cmdstr = "DELETE question FROM Qustion WHERE Test ='" + listBox1.SelectedItem + "'";
                    OleDbCommand deletequstion = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    deletequstion.ExecuteNonQuery();
                    conn.Close();
                    cmdstr = "DELETE Lecture FROM Lectures WHERE Lecture ='" + listBox1.SelectedItem + "'";
                    OleDbCommand Testdelete = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    Testdelete.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Выберите лекцию или тест для удаления", "Ошибка удаления");
            }
            loadlecture();
            listBox1.SelectedIndex = -1;
        }

        private void тестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                if (panel1.Visible == true)
                {
                    panel1.Visible = false;
                    сохранитьToolStripMenuItem.Visible = false;
                    вставитьToolStripMenuItem.Visible = false;
                }
                if (panel4.Visible == true)
                {
                    panel4.Visible = false;
                }
                textBox1.Clear();
                panel2.Visible = true;
                swindex = 4;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(textBox3.Text);
            DPDataBaseDataSet3TableAdapters.QustionTableAdapter qustionTableAdapter = new DPDataBaseDataSet3TableAdapters.QustionTableAdapter();
            qustionTableAdapter.Insert(listBox1.SelectedItem.ToString(), textBox2.Text, textBox3.Text);
            questionsave = 1;
            panel5.Visible = false;
            textBox3.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex != -1)
            {
                string cmdstr = "DELETE question FROM Qustion WHERE anwoption ='" + checkedListBox1.SelectedItem.ToString() + "'";
                OleDbCommand anwdelete = new OleDbCommand(cmdstr, conn);
                conn.Open();
                anwdelete.ExecuteNonQuery();
                conn.Close();
                string sitem = checkedListBox1.SelectedItem.ToString();
                checkedListBox1.Items.Remove(sitem);
                
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления","Ошибка удаления");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Сохранить вопрос?", "Сохранить", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Нет вопроса", "Ошибка");
                }
                else
                {
                    savequestion();
                    testload();
                    listBox2.SelectedItem = textBox2.Text;
                    questionsave = 0;
                }
            }
            if (result == DialogResult.No)
            {
                int n = checkedListBox1.Items.Count;
                int i = 0;
                while (i<n)
                {
                    string cmdstr = "DELETE question FROM Qustion WHERE question = '" + textBox2.Text + "'";
                    OleDbCommand command = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    questionsave = 0;
                }
                textBox2.Clear();
                checkedListBox1.Items.Clear();

            }
        }

        private void savequestion()
        {
            int i = 0;
            int n = checkedListBox1.Items.Count;
            if (listBox2.SelectedIndex == -1)
            {
                while (i < n)
                {
                    if (checkedListBox1.GetItemChecked(i) == true)
                    {
                        DPDataBaseDataSet2TableAdapters.TestsTableAdapter testsTableAdapter = new DPDataBaseDataSet2TableAdapters.TestsTableAdapter();
                        testsTableAdapter.Insert(listBox1.SelectedItem.ToString(), textBox2.Text, checkedListBox1.Items[i].ToString());
                        break;
                    }
                    else
                    {
                        if (i == n - 1)
                        {
                            MessageBox.Show("Выберите верный ответ", "Не выбран ответ");
                            break;
                        }
                        i++;
                    }
                }


            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            { checkedListBox1.Items.Clear();
                textBox2.Text = "";

            }
            else
            {
                checkedListBox1.Items.Clear();
                textBox2.Text = listBox2.SelectedItem.ToString();
                string cmdstr = "SELECT anwoption FROM Qustion WHERE question = '" + textBox2.Text + "'";
                OleDbCommand command = new OleDbCommand(cmdstr, conn);
                IList<string> lc = new List<string>();
                conn.Open();
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    checkedListBox1.Items.Add(reader[0].ToString());
                }
                conn.Close();
                cmdstr = "SELECT answer FROM Tests WHERE question = '" + listBox2.SelectedItem + "'";
                OleDbCommand commandans = new OleDbCommand(cmdstr, conn);
                conn.Open();
                string answer = (string)commandans.ExecuteScalar();
                conn.Close();
                checkedListBox1.SelectedItem = answer;
                checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
                checkedListBox1.SelectedIndex = -1;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
           
            if (questionsave == 1)
            {
                var result = MessageBox.Show("Сохранить вопрос?", "Сохранить", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.No)
                {
                    checkedListBox1.Items.Clear();
                    textBox2.Clear();
                    listBox2.SelectedIndex = -1;
                    questionsave = 0;
                }
                if (result == DialogResult.Yes)
                {
                    savequestion();
                    checkedListBox1.Items.Clear();
                    textBox2.Clear();
                    listBox2.SelectedIndex = -1;
                    questionsave = 0;
                }

                
            }
            
            
        }   

        private void button19_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            textBox2.Clear();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Удалить вопрос?", "Удалить?", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                if (listBox2.SelectedIndex != -1)
                {
                    string cmdstr = "SELECT question FROM Tests WHERE question = '" + listBox2.SelectedItem + "'";
                    OleDbCommand command = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    string deletequestion = (string)command.ExecuteScalar();
                    conn.Close();
                    cmdstr = "DELETE question FROM Tests WHERE question ='" + deletequestion + "'";
                    OleDbCommand testsdelete = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    testsdelete.ExecuteNonQuery();
                    conn.Close();
                    cmdstr = "DELETE question FROM Qustion WHERE question ='" + deletequestion + "'";
                    OleDbCommand deletequstion = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    deletequstion.ExecuteNonQuery();
                    conn.Close();
                    testload();
                }
                else
                {
                    MessageBox.Show("Выберите вопрос для удаления", "Ошибка удаления");
                }
            }

        }
        private void testload()
        {
            string  cmdstr = "SELECT question FROM Tests WHERE Test ='" + listBox1.SelectedItem + "'";
            OleDbCommand commandtest = new OleDbCommand(cmdstr, conn);
            IList<string> lc = new List<string>();
            conn.Open();
            OleDbDataReader reader = commandtest.ExecuteReader();
            while (reader.Read())
            {
                lc.Add(reader[0].ToString());
            }
            conn.Close();
            listBox2.DataSource = null;
            listBox2.DataSource = lc;
            listBox2.DisplayMember = "question";
            listBox2.SelectedIndex = -1;
            panel1.Visible = false;
            сохранитьToolStripMenuItem.Visible = false;
            вставитьToolStripMenuItem.Visible = false;
            panel4.Visible = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int n = listBox1.Items.Count;
            int i = 0;
            if (n != 0)
            {
                while (i < n)
                {
                    string cmdstr = "SELECT Type FROM Lectures WHERE Lecture = '" + listBox1.Items[i] + "'";
                    OleDbCommand command = new OleDbCommand(cmdstr, conn);
                    conn.Open();
                    string type = (string)command.ExecuteScalar();
                    conn.Close();
                    if (type == "Lecture")
                    {
                        File.Delete("./text/" + comboBoxT.Text + "/" + listBox1.Items[i] + ".rtf");
                        cmdstr = "DELETE Lecture FROM Lectures WHERE Lecture ='" + listBox1.Items[i] + "'";
                        OleDbCommand lecturesdelete = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        lecturesdelete.ExecuteNonQuery();
                        conn.Close();
                    }
                    if (type == "Test")
                    {
                        cmdstr = "DELETE question FROM Tests WHERE Test ='" + listBox1.Items[i] + "'";
                        OleDbCommand testsdelete = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        testsdelete.ExecuteNonQuery();
                        conn.Close();
                        cmdstr = "DELETE question FROM Qustion WHERE Test ='" + listBox1.Items[i] + "'";
                        OleDbCommand deletequstion = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        deletequstion.ExecuteNonQuery();
                        conn.Close();
                        cmdstr = "DELETE Lecture FROM Lectures WHERE Lecture ='" + listBox1.Items[i] + "'";
                        OleDbCommand Testdelete = new OleDbCommand(cmdstr, conn);
                        conn.Open();
                        Testdelete.ExecuteNonQuery();
                        conn.Close();
                    }

                    i++;
                }
            }
            if (comboBoxT.SelectedIndex != -1)
            {
                int deleteindex = comboBoxT.SelectedIndex;
                string cmdstr = "DELETE Topic FROM Topics WHERE Topic ='" + comboBoxT.Text + "'";
                OleDbCommand topicdelete = new OleDbCommand(cmdstr, conn);
                conn.Open();
                int f = topicdelete.ExecuteNonQuery();
                conn.Close();
                comboBoxT.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Выберите тему для удаления", "Ошибка удаления");
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { }
            else
            {
                string n = listBox1.SelectedItem.ToString();
                richTextBox1.SaveFile("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem + ".rtf");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string item = (string)listBox1.SelectedItem;
                label3.Text = item;
                richTextBox1.SaveFile("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem.ToString() + ".rtf");
            }
            catch
            {

            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                foreach (int index in list.CheckedIndices)
                    if (index != e.Index)
                        list.SetItemChecked(index, false);

        }
    }
}
