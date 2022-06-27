using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WEA_SQL
{
    
    public partial class Form1 : Form
    {
        System.Drawing.Point Last_point;private string adm1 = "";
        public Form1()
        {
            InitializeComponent();
        }

       
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                this.Left += e.X-Last_point.X;
                this.Top += e.Y-Last_point.Y ;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Last_point=e.Location;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            this.panel1.BackColor = System.Drawing.Color.Red;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            this.panel1.BackColor = System.Drawing.Color.DimGray;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            this.button1.BackColor = System.Drawing.Color.Green;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.button1.BackColor = System.Drawing.Color.DimGray;
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            //настройки графики окна
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new Font(GloBal.myFont.Families[0], 24);
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new Font(GloBal.myFont.Families[0], 16);
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new Font(GloBal.myFont.Families[0], 16);
            this.button1.Font = new Font(GloBal.myFont.Families[0], 24);
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Font = new Font(GloBal.myFont.Families[0], 12);
            checkBox1.Checked = true;
            //настройки графики окна

            if (GloBal.Conf_oll.us.Login!=null) /*Авто заполнение данных логина и пароля*/
            {
                this.textBox1.Text = GloBal.Conf_oll.us.Login;
                if (GloBal.Conf_oll.us.Passwword != null)
                {
                    this.textBox2.Text = GloBal.Conf_oll.us.Passwword;
                }
            }
            if (GloBal.Conf_oll.adm.Auto_login==true) /*Автоматичесский вход*/
            {
                button1_Click(this.button1,null);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)/*для входа в форму настроек*/
        {
            //adm1+enter
            if (e.KeyValue==(char)Keys.A || e.KeyValue == (char)Keys.D || e.KeyValue == (char)Keys.M || e.KeyValue == (char)Keys.D1 )
            {
                adm1 += e.KeyValue;
            }
            if (e.KeyValue == (char)Keys.Enter && adm1== "65687749")
            {
                if (GloBal.admin==null)
                {
                    GloBal.admin= new ADMIN_OPTIONS(1);
                }
                GloBal.admin.Show();
                this.Hide();
            }
            if (e.KeyValue == (char)Keys.Back || e.KeyValue == (char)Keys.Delete)
            {
                adm1 = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Login=textBox1.Text, password= textBox2.Text;
            GloBal.SQL.connect();
            if (GloBal.SQL.LOgininng(Login, password) == true) 
            {
                if (this.checkBox1.Checked==true)
                {
                    GloBal.load_Conf.seri_s_oll(GloBal.Conf_oll);
                }
                if (GloBal.main_Worc==null) { GloBal.main_Worc = new Main_worc(); }
                this.Hide();
                GloBal.main_Worc.Show();
            }
            else 
            {
                MessageBox.Show("Ошибка в логине или пароле","ошибка пользователя",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            GloBal.SQL.clouse();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Help_Click(object sender, EventArgs e)
        {
            string fn = @"Help_WORC_BLOC_ver_0.4.pdf";
            var proc = new Process();
            proc.StartInfo.FileName = fn;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
    }
}
