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
using System.Data.OleDb;

namespace DiplomaProject
{
    public partial class Form3 : Form
    {
        float testresult = 0;
        int countoprion;
        OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = DPDataBase.accdb; Jet OLEDB:Database Password=ЦЛКМЬ51;");
        public Form3()
        {
            InitializeComponent();
            this.topicsTableAdapter.Fill(this.dPDataBaseDataSet.Topics);
            comboBoxT.SelectedIndex = -1;
            panel1.Visible = false;
            panel4.Visible = false;

        }

        private void comboBoxT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxT.SelectedIndex != -1)
            {
                loadlecture();
            }
        }

        private void loadlecture()
        {

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
                string listtype = (string)command.ExecuteScalar();
                conn.Close();
                if (listtype == "Lecture")
                {
                    panel1.Visible = true;
                    panel4.Visible = false;
                    string n = listBox1.SelectedItem.ToString();
                    if (File.Exists("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem + ".rtf"))
                    {
                        richTextBox1.LoadFile("./text/" + comboBoxT.Text + "/" + listBox1.SelectedItem + ".rtf");
                        label3.Text = listBox1.SelectedItem.ToString();
                    }
                    else
                    {
                        richTextBox1.Clear();
                        panel1.Visible = false;
                        MessageBox.Show("Файл отсутствует", "Ошибка");
                    }
                }
                else
                {
                    testresult = 0;
                    testload();
                    countoprion = listBox2.Items.Count;
                    button20.Visible = false;
                    button17.Visible = true;
                }
            }
        }

        private void testload()
        {
            string cmdstr = "SELECT question FROM Tests WHERE Test ='" + listBox1.SelectedItem + "'";
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
            panel1.Visible = false;
            panel4.Visible = true;
            label6.Text = "";
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                checkedListBox1.Items.Clear();
                label4.Text = "";

            }
            else
            {
                checkedListBox1.SelectionMode = SelectionMode.One;
                checkedListBox1.Items.Clear();
                label4.Text = listBox2.SelectedItem.ToString();
                string cmdstr = "SELECT anwoption FROM Qustion WHERE question = '" + label4.Text + "'";
                OleDbCommand command = new OleDbCommand(cmdstr, conn);
                IList<string> lc = new List<string>();
                conn.Open();
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    checkedListBox1.Items.Add(reader[0].ToString());
                }
                conn.Close();
                checkedListBox1.SelectedIndex = -1;



            }
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form Form1 = new Form1();
            Form1.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < countoprion)
            {
                string cmdstr = "SELECT answer FROM Tests WHERE question = '" + label4.Text + "'";
                OleDbCommand command = new OleDbCommand(cmdstr, conn);
                conn.Open();
                string answ = (string)command.ExecuteScalar();
                conn.Close();
                int i = 0;
                string answopt;
                while (i < countoprion)
                {
                    if (checkedListBox1.GetItemChecked(i) == true)
                    {
                        answopt = checkedListBox1.Items[i].ToString();
                        if (answ == answopt)
                        {
                            testresult += 1;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                if (listBox2.SelectedIndex < countoprion - 1)
                {
                    listBox2.SelectedIndex += 1;
                    
                }
                else
                {
                    button20.Visible = true;
                    button17.Visible = false;
                }
            }
            
            
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (testresult != 0)
            {
                double result = (testresult/countoprion)*100;
                result = Math.Round(result, 1);
                label6.Text = "Ваш результат: " + result.ToString();
            }
            else
            {
                label6.Text = "Ваш результат: " + testresult.ToString();
            }
            button20.Visible = false;
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
