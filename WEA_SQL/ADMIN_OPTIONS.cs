using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEA_SQL
{
    public partial class ADMIN_OPTIONS : Form
    {
        System.Drawing.Point Last_point;
        DataTable dt;
        int form_num;
        bool newrouadding = false;
        public ADMIN_OPTIONS(int _form_num)
        {
            form_num = _form_num;
            InitializeComponent();
        }


        private void ADMIN_OPTIONS_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            this.textBox1.Text= (string)GloBal.Conf_oll.adm.Server;
            this.textBox2.Text= (string)GloBal.Conf_oll.adm.DataBase;
            this.textBox3.Text= (string)GloBal.Conf_oll.adm.Login;
            this.textBox4.Text= (string)GloBal.Conf_oll.adm.Passwword;
            this.textBox5.Text = (string)GloBal.SQL.LOg_base;

            panel1.Size = new Size(panel1.Width, 35);
            panel3.Size = new Size(panel3.Width, 35);

            textBox6.Enabled = textBox7.Enabled = textBox8.Enabled = textBox9.Enabled = true;
            textBox14.Enabled = textBox15.Enabled = false;
            textBox16.Text = Properties.Resources.create_base;
            try
            {
                if (GloBal.SQL.sqlConnection!=null)
                {
                GloBal.SQL.connect();
                dt = GloBal.SQL.Admin_oll_user();
                DataTable dtt = GloBal.SQL.select__($"SELECT * FROM INFORMATION_SCHEMA.TABLES ");
                dataGridView2.DataSource = dtt;
                GloBal.SQL.clouse();
                dataGridView1.DataSource = dt;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                    DataGridViewLinkCell Lincell_2 = new DataGridViewLinkCell();
                    dataGridView1[0, i] = Lincell;
                    dataGridView1[0, i] = Lincell_2;
                }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Ошибка  загрузкb пользователей ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void label4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void label4_MouseEnter_1(object sender, EventArgs e)
        {
            this.panel2.BackColor = System.Drawing.Color.Red;

        }

        private void label4_MouseLeave_1(object sender, EventArgs e)
        {
            this.panel2.BackColor = System.Drawing.Color.DimGray;

        }

        private void ADMIN_OPTIONS_MouseDown_1(object sender, MouseEventArgs e)
        {
            Last_point = e.Location;
        }

        private void ADMIN_OPTIONS_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - Last_point.X;
                this.Top += e.Y - Last_point.Y;
            }
        }

     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex==0) 
            {
                this.label1.Enabled= this.label2.Enabled = this.label3.Enabled = this.label5.Enabled = false;
                this.textBox1.Enabled= this.textBox2.Enabled = this.textBox3.Enabled = this.textBox4.Enabled = false;
                this.button3.Enabled = false;
                this.label6.Enabled = this.textBox5.Enabled = true;

            }
            if (this.comboBox1.SelectedIndex == 1)
            {
                this.label1.Enabled = this.label2.Enabled = this.label3.Enabled = this.label5.Enabled = true;
                this.textBox1.Enabled = this.textBox2.Enabled = this.textBox3.Enabled = this.textBox4.Enabled = true;
                this.button3.Enabled = true;

                this.label6.Enabled = this.textBox5.Enabled = false;
            }
        }

       

        private void panel1_Click(object sender, EventArgs e)
        {
            if (this.panel1.Size.Height==339) 
            {
                 panel1.Size = new Size(panel1.Width, 35); 
            }
            else 
            {
                panel1.Size = new Size(panel1.Width, 339);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (form_num)
            {
                case 1:
                    this.Hide();
                    if (GloBal.Logining==null)
                    {
                        GloBal.Logining = new Form1();
                    }
                    GloBal.Logining.Show();
                    break;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            switch (this.panel3.Size.Height)
            {
                case 35:
                    panel3.Size = new Size(panel3.Width, 296);
                    break;
                case 296:
                    panel3.Size = new Size(panel3.Width, 458);
                    break;
                case 458:
                    panel3.Size = new Size(panel3.Width, 295);
                    break;
                case 295:
                    panel3.Size = new Size(panel3.Width, 35);
                    break;
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newrouadding == false) 
                {
                    newrouadding=true;
                    DataGridViewRow row=this.dataGridView1.Rows[dataGridView1.Rows.Count-2];
                    DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                    this.dataGridView1[0, dataGridView1.Rows.Count - 2] = Lincell;
                    row.Cells["WOC"].Value = "INSERT";
                }
                

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0 )
                {
                    GloBal.SQL.connect();
                    string task = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    switch (task)
                    {
                        case "DEL":
                            if (MessageBox.Show("Удалить таблицу", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                GloBal.SQL.Admin_del_user(Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
                            }
                            break;
                        case "INSERT":
                            try
                            {
                            DataRow row=dt.NewRow();
                            foreach (var item in row.Table.Columns)
                            {
                                row[item.ToString()] = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[item.ToString()].Value;
                            }
                            GloBal.SQL.Admin_insert_user(row);
                            }
                            catch (Exception)
                            {
                                dt.Rows.RemoveAt(dt.Rows.Count - 1);
                                MessageBox.Show("регистрация пользователя прошла НЕ УСПЕШНО", "Insert user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            newrouadding = false;
                            break;
                        case "Update":
                            try
                            {
                                DataRow row = dt.NewRow();
                                foreach (var item in row.Table.Columns)
                                {
                                    row[item.ToString()] = dataGridView1.Rows[e.RowIndex].Cells[item.ToString()].Value;
                                }

                                GloBal.SQL.Admin_update_user(row);
                            }
                            catch (Exception)
                            {
                                dt.Rows.RemoveAt(dt.Rows.Count - 1);
                                MessageBox.Show("обновление пользователя прошла НЕ УСПЕШНО", "Insert user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                    }
                    dataGridView1.DataSource = dt = GloBal.SQL.Admin_oll_user();
                    GloBal.SQL.clouse();
                   
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                        dataGridView1[0, i] = Lincell;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cst = this.textBox5.Text = $"Data Source={textBox1.Text};Initial Catalog={textBox2.Text};User Id={textBox3.Text};Password={textBox4.Text};";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите сохранить строку подключени и другие параметры", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (textBox5.Text!=null)
                {
                GloBal.Conf_oll.adm.Sql_db_serv=textBox5.Text;
                GloBal.SQL = null;
                GloBal.SQL = new SQL_DB_WOC(textBox5.Text);
                }
                GloBal.Conf_oll.adm.Server=textBox1.Text;
                GloBal.Conf_oll.adm.DataBase=textBox2.Text;
                GloBal.Conf_oll.adm.Login=textBox3.Text;
                GloBal.Conf_oll.adm.Passwword=textBox4.Text;
                GloBal.load_Conf.seri_s_oll(GloBal.Conf_oll);

            }
            try
            {
                GloBal.SQL.connect();
                dt = GloBal.SQL.Admin_oll_user();
                DataTable dtt = GloBal.SQL.select__($"SELECT * FROM INFORMATION_SCHEMA.TABLES ");
                dataGridView2.DataSource = dtt;
                GloBal.SQL.clouse();
                dataGridView1.DataSource = dt;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                    DataGridViewLinkCell Lincell_2 = new DataGridViewLinkCell();
                    dataGridView1[0, i] = Lincell;
                    dataGridView1[0, i] = Lincell_2;
                }

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

      
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newrouadding == false)
                {
                    DataGridViewRow row = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex];
                    DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                    dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex] = Lincell;
                    row.Cells["WOC"].Value = "Update";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string id = null, Name = null, Login = null, Passwword = null;
            DataTable dd=new DataTable();
            GloBal.SQL.connect();
            if (button6.BackColor == Color.Brown)
            {
                if (textBox6.Text != "")
                {
                    id = textBox6.Text;
                   
                        if (textBox7.Text != "")
                        {
                            Name = textBox7.Text;
                        }
                        if (textBox8.Text != "")
                        {
                            Login = textBox8.Text;
                        }
                        if (textBox9.Text != "")
                        {
                            Passwword = textBox9.Text;
                        }
                    dd=GloBal.SQL.Admin_serch_user_2(id, Name, Login, Passwword);
                   
                }
                else
                {
                    MessageBox.Show("Нет данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (textBox14.Text != "" && textBox15.Text != "")
                {
                    dd=GloBal.SQL.Admin_serch_user_For_id(textBox14.Text, textBox15.Text);
                }
                else
                {
                    MessageBox.Show("Нет данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            dataGridView1.DataSource = dd;
            GloBal.SQL.clouse();
        }

       
        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.BackColor == Color.Brown)
            {
                button6.BackColor = Color.Green;
                textBox6.Enabled = textBox7.Enabled = textBox8.Enabled = textBox9.Enabled = false;
                textBox14.Enabled = textBox15.Enabled = true;
            }
            else
            {
                button6.BackColor = Color.Brown;
                textBox6.Enabled = textBox7.Enabled = textBox8.Enabled = textBox9.Enabled = true;
                textBox14.Enabled = textBox15.Enabled = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
            GloBal.SQL.connect();
            dataGridView1.DataSource = dt = GloBal.SQL.Admin_oll_user();
            GloBal.SQL.clouse();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                dataGridView1[0, i] = Lincell;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GloBal.SQL.connect();
            if (button6.BackColor == Color.Brown)
            {
                string id = null, Name = null, Login = null, Passwword = null;
                if (textBox6.Text!="")
                {
                    id = textBox6.Text;
                    if (MessageBox.Show("Удалить Пользователя?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (textBox7.Text!="")
                        {
                            Name = textBox7.Text;
                        }
                        if (textBox8.Text != "")
                        {
                            Login = textBox8.Text;
                        }
                        if (textBox9.Text != "")
                        {
                            Passwword = textBox9.Text;
                        }
                        GloBal.SQL.Admin_del_user_2(id, Name, Login, Passwword);
                    }
                }
                else
                {
                    MessageBox.Show("Нет данных", "Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                if (textBox14.Text != "" && textBox15.Text != "")
                {
                    GloBal.SQL.Admin_del_user_For_id(textBox14.Text, textBox15.Text);
                }
                else 
                {
                    MessageBox.Show("Нет данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            dataGridView1.DataSource = dt = GloBal.SQL.Admin_oll_user();
            GloBal.SQL.clouse();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                dataGridView1[0, i] = Lincell;
            }
        }
               
        private void panel3_Click(object sender, EventArgs e)
        {
            switch (this.panel3.Size.Height)
            {
                case 35:
                    panel3.Size = new Size(panel3.Width, 296);
                    break;
                case 296:
                    panel3.Size = new Size(panel3.Width, 458);
                    break;
                case 458:
                    panel3.Size = new Size(panel3.Width, 295);
                    break;
                case 295:
                    panel3.Size = new Size(panel3.Width, 35);
                    break;
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            switch (this.panel3.Size.Height)
            {
                case 296:
                    panel3.Size = new Size(panel3.Width, 458);
                    break;
                case 458:
                    panel3.Size = new Size(panel3.Width, 295);
                    break;
                case 295:
                    panel3.Size = new Size(panel3.Width, 35);
                    break;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            switch (this.panel3.Size.Height)
            {
                case 35:
                    panel3.Size = new Size(panel3.Width, 296);
                    break;
                case 296:
                    panel3.Size = new Size(panel3.Width, 458);
                    break;
                case 458:
                    panel3.Size = new Size(panel3.Width, 295);
                    break;
                case 295:
                    panel3.Size = new Size(panel3.Width, 35);
                    break;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        int cc = 0;
        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.BackColor == Color.DimGray)
            {
                button8.BackColor = Color.Red;
                textBox10.Visible = textBox11.Visible = textBox12.Visible = textBox13.Visible = true;
                label15.Visible = label16.Visible = label17.Visible = label18.Visible = true;

            }
            else if (button8.BackColor == Color.Green)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("WOC");
                    dt.Columns.Add("Id");
                    dt.Columns.Add("nik_name");
                    dt.Columns.Add("Login");
                    dt.Columns.Add("Password");
                    dt.Rows.Add(dt.NewRow());

                    dt.Rows[0][0] = "";
                    dt.Rows[0][1] = textBox12.Text;
                    dt.Rows[0][2] = textBox13.Text;
                    dt.Rows[0][3] = textBox11.Text;
                    dt.Rows[0][4] = textBox10.Text;
                    cc = 0;
                    button8.BackColor = Color.DimGray;
                    GloBal.SQL.connect();
                    GloBal.SQL.Admin_insert_user(dt.Rows[0]);
                    GloBal.SQL.clouse();
                    textBox10.Visible = textBox11.Visible = textBox12.Visible = textBox13.Visible = false;
                    label15.Visible = label16.Visible = label17.Visible = label18.Visible = false;
                    button7_Click(button7, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (button8.BackColor == Color.Red) 
            {
                button8.BackColor = Color.DimGray;
                textBox10.Visible = textBox11.Visible = textBox12.Visible = textBox13.Visible = false;
                label15.Visible = label16.Visible = label17.Visible = label18.Visible = false;
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            cc += 1;
            if (cc==4)
            {
                button8.BackColor = Color.Green;
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            cc += 1;
            if (cc == 4)
            {
                button8.BackColor = Color.Green;
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            cc += 1;
            if (cc == 4)
            {
                button8.BackColor = Color.Green;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            cc+=1;
            if (cc == 4)
            {
                button8.BackColor = Color.Green;
            }
        }

        private void button9_Click(object sender, EventArgs e) /*открыть локальную базу данных*/
        {
            try
            {
            string filePat = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePat = openFileDialog.FileName;

                }
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                textBox5.Text=$@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='{openFileDialog.FileName}';Integrated Security=True";
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                GloBal.SQL.connect();
                GloBal.SQL.select__(textBox16.Text); 
                GloBal.SQL.clouse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
