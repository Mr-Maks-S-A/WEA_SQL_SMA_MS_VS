using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using Excel=Microsoft.Office.Interop.Excel;

namespace WEA_SQL
{
    public partial class Main_worc : Form
    {
        int id_table,value_toinsert;
        System.Drawing.Point Last_point;
        string last_table_box = "";
        DataSet ds;
        DataSet ds2=new DataSet();
        DataSet ds3=new DataSet();
        DataSet ds_save=new DataSet();
        DataTable dt;
        bool newrouadding = false;

        List <string> gotovo = new List<string> ();
        public Main_worc()
        {
            InitializeComponent();
        }


        private void label4_MouseEnter(object sender, EventArgs e)
        {
            this.panel2.BackColor = System.Drawing.Color.Red;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            this.panel2.BackColor = System.Drawing.Color.DimGray;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Main_worc_Load(object sender, EventArgs e)
        {
            panel7.Size = new System.Drawing.Size(1218, 189);
            dataGridView3.AllowUserToAddRows = false;
            label1.Text = $"Выбор таблици для работы  " +
                $"Ваше имя: {GloBal.Conf_oll.us.Name.TrimEnd(' ')} " +
                $"\nВаш ID=> {GloBal.Conf_oll.us.ID} ";
            panel1.Size = new Size(panel1.Width, 581);
            panel3.Size = new System.Drawing.Size(1226, 538);
            panel4.Size = new System.Drawing.Size(200, 44);
            panel4.Enabled = false;
            panel7.Size = new System.Drawing.Size(1164, 137);
            panel8.Size = new System.Drawing.Size(1130, 138);
            panel9.Size = new System.Drawing.Size(1090, 528);
            panel15.Size = new System.Drawing.Size(536, 477);
            
            comboBox17.Items.Clear();
            if (GloBal.Conf_oll.us.requests != null && GloBal.Conf_oll.us.requests.Count!=0)
            {
                for (int i = 0; i < GloBal.Conf_oll.us.requests.Count; i++)
                {
                    comboBox17.Items.Add(GloBal.Conf_oll.us.requests[i][0]);
                }
                comboBox17.SelectedIndex = 0;
            }
            try
            {
                GloBal.SQL.connect();
                dataGridView1.DataSource =dt=  GloBal.SQL.TO_greadviu_1(GloBal.Conf_oll.us.ID);
                GloBal.SQL.clouse();
                

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                    DataGridViewLinkCell Lincell_2 = new DataGridViewLinkCell();
                    dataGridView1[0, i] = Lincell;
                    dataGridView1[1, i] = Lincell_2;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка в загрузку таблиц дел", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Main_worc_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - Last_point.X;
                this.Top += e.Y - Last_point.Y;
            }
        }

        private void Main_worc_MouseDown_1(object sender, MouseEventArgs e)
        {
            Last_point = e.Location;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    GloBal.SQL.connect();
                    string task = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    switch (task)
                    {
                        case "Delete":
                            if (MessageBox.Show("Удалить таблицу", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                GloBal.SQL.DEL_user_table(
                                    GloBal.Conf_oll.us.ID, Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString())
                                    );
                                id_table = 0;
                            }
                            break;
                        case "Woc":
                            id_table = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                            ds = GloBal.SQL.TO_greadviu_2(GloBal.Conf_oll.us.ID, id_table); 
                            foreach (DataTable item in ds.Tables)
                            {
                                this.comboBox1.Items.Add(item.TableName);
                            }
                            comboBox1.SelectedIndex = 0;
                            dataGridView2.Columns["Conf_id"].ReadOnly = dataGridView2.Columns["Таблица"].ReadOnly = true;
                            if (dataGridView1.Columns.Equals("User") == true)
                            {
                                dataGridView2.Columns["User"].ReadOnly = true;
                            }
                            switch (this.panel1.Size.Height)
                            {
                                case 581:
                                    panel1.Size = new Size(panel1.Width, 44);
                                    break;
                                case 44:
                                    panel1.Size = new Size(panel1.Width, 581);
                                    break;
                            }
                            panel5.Enabled=panel4.Enabled=button3.Enabled = button4.Enabled = button8.Enabled = button9.Enabled = true;
                            break;
                    }
                    button3_Click(this.button3,null);
                    button2_Click(this.button2, null);
                    this.label7.Text = "таблица :" + id_table.ToString();
                    dataGridView1.DataSource =dt= GloBal.SQL.TO_greadviu_1(GloBal.Conf_oll.us.ID);
                    GloBal.SQL.clouse();

                     
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                        DataGridViewLinkCell Lincell_2 = new DataGridViewLinkCell();

                        dataGridView1[0, i] = Lincell;
                        dataGridView1[1, i] = Lincell_2;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0 )
                {
                    GloBal.SQL.connect();
                    string task = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    switch (task)
                    {
                        case "DELETE":
                            if (MessageBox.Show("Удалить дело", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                GloBal.SQL.DEL_Дело(
                                    GloBal.Conf_oll.us.ID, Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString()),
                                    Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString())
                                    ); 
                            }
                            break;
                        case "Update":
                            try
                            {
                                if (MessageBox.Show("ОБНОВИТЬ ДЕЛО ?", "ОБНОВЛЕНИЕ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    DataRow row = ds.Tables[this.comboBox1.Text].NewRow();
                                    last_table_box=this.comboBox1.Text;
                                    foreach (var item in row.Table.Columns)
                                    {
                                        row[item.ToString()] = dataGridView2.Rows[e.RowIndex].Cells[item.ToString()].Value;
                                    }
                                    if(GloBal.SQL.Update_Дело(this.comboBox1.Text, Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString()), row)==1) 
                                    {
                                        MessageBox.Show("обновление пользователя прошла  УСПЕШНО", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("обновление пользователя прошла НЕ УСПЕШНО", "Insert user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            
                            break;
                    }
                    this.label7.Text = "таблица :" + id_table.ToString();
                    ds = GloBal.SQL.TO_greadviu_2(GloBal.Conf_oll.us.ID, id_table);
                    button3_Click(this.button3, null);
                    this.comboBox1.SelectedItem = last_table_box;
                    GloBal.SQL.clouse();


                    for (int i = 0; i < ds.Tables[comboBox1.Text].Rows.Count; i++)
                    {
                        DataGridViewLinkCell Lincell = new DataGridViewLinkCell();

                        dataGridView2[0, i] = Lincell;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)/*табоица программы обновление*/
        {
            try
            {
                if (id_table != 0)
                {

                    ds = GloBal.SQL.TO_greadviu_2(GloBal.Conf_oll.us.ID,id_table);
                    comboBox1.Items.Clear();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        comboBox1.Items.Add((string)$"{ds.Tables[i].TableName}");
                    }

                    for (int i = 0; i < ds.Tables[comboBox1.Text].Rows.Count; i++)
                    {
                        DataGridViewLinkCell Lincell = new DataGridViewLinkCell();

                        dataGridView2[0, i] = Lincell;
                    }
                    
                }
                else 
                {
                    dataGridView2.DataSource = ds = new DataSet();
                    this.comboBox1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = ds.Tables[this.comboBox1.Text];
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                dataGridView2[0, i] = Lincell;
            }
            dataGridView2.Columns["Conf_id"].ReadOnly = dataGridView2.Columns["Таблица"].ReadOnly  = true;
            if (dataGridView2.Columns.Contains("User")==true)
            {
                dataGridView2.Columns["User"].ReadOnly = true;
            }
        }



        private void panel1_Click(object sender, EventArgs e)
        {
            switch (this.panel1.Size.Height)
            {
                case 581:
                    panel1.Size = new Size(panel1.Width, 44);
                    break;
                case 44:
                    panel1.Size = new Size(panel1.Width, 581);
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GloBal.SQL.connect();
            dataGridView1.DataSource = GloBal.SQL.TO_greadviu_1(GloBal.Conf_oll.us.ID);
            GloBal.SQL.clouse();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                DataGridViewLinkCell Lincell_2 = new DataGridViewLinkCell();

                dataGridView1[0, i] = Lincell;
                dataGridView1[1, i] = Lincell_2;
            }
            if (comboBox4.Items.Count>0)
            {
            comboBox4_SelectedIndexChanged(comboBox4,null);
            }
        }


        private void button1_Click(object sender, EventArgs e) /*новая таблица дел*/
        {
            try
            {
            string filePat ="";
            GloBal.SQL.connect();
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePat = openFileDialog.FileName;

                    }
                }
                if (filePat != "")
                {
                    panel1.Size = new Size(panel1.Width, 44);
                    

                    panel6.Enabled= panel6.Visible = true;
                    panel1.Enabled = panel3.Enabled =  false;
                    DataSet dss = open_exel(filePat);
                    this.progressBar1.Value = GloBal.loat_line;
                    id_table = GloBal.SQL.Save_new_exel(dss, GloBal.Conf_oll.us.ID,0);
                    this.progressBar1.Value = GloBal.loat_line;
                    ds =GloBal.SQL.TO_greadviu_2( GloBal.Conf_oll.us.ID, id_table);
                    comboBox1.Items.Clear();
                    foreach (DataTable item in ds.Tables)
                    {
                        this.comboBox1.Items.Add(item.TableName);
                    }
                    this.label7.Text = "таблица :"+ id_table.ToString();
                    this.comboBox1.SelectedIndex = 0;
                    GloBal.loat_line = 100;
                    this.progressBar1.Value = GloBal.loat_line;
                    button2_Click(this.button2,null);
                    dataGridView2.Columns["Conf_id"].ReadOnly = dataGridView2.Columns["Таблица"].ReadOnly = true;
                panel5.Enabled=panel4.Enabled=this.Enabled = panel1.Enabled = panel3.Enabled =  true;
            panel6.Visible = false;
            button3.Enabled = button4.Enabled = button8.Enabled = button9.Enabled = true;
                }
                GloBal.SQL.clouse();
            GloBal.loat_line = progressBar1.Value = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataSet open_exel(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            IExcelDataReader reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
            GloBal.loat_line = 20;
            var conf = new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader => {
                        rowReader.Read();
                        rowReader.Read();
                    }

                }
            };
            GloBal.loat_line = 30;
            DataSet ds = reader.AsDataSet(conf);
            DataSet cc = new DataSet();
            string[] arr_table = new string[] { "Общие_данные", "Процесс окозания услуги", "Данные_услуги",
                                                "Данные ФЛ/ИП", "Данные ЮЛ", "Сроки и даты", "Сведения по обьектам недвижимости" };
            int[] arr = new int[] { 7, 11, 10, 18, 8, 6, 4 };
            foreach (DataTable item in ds.Tables)
            {
                int con = 0;
                for (int i = 0; i < 7; i++)
                {
                    DataTable tb = new DataTable(arr_table[i]);
                    for (int j = 0; j < arr[i]; j++)
                    {
                        tb.Columns.Add(item.Columns[con].ColumnName);
                        con++;
                    }
                    for (int b = 0; b < item.Rows.Count; b++)
                    {
                        con = con - arr[i];
                        DataRow dataRow = tb.NewRow();
                        for (int j = 0; j < arr[i]; j++)
                        {
                            dataRow[item.Columns[con].ColumnName] = item.Rows[b][con].ToString();
                            con++;
                        }
                        tb.Rows.Add(dataRow);
                    }
                    cc.Tables.Add(tb);
                }
            }
            GloBal.loat_line = 40;

            return cc;

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newrouadding == false)
                {
                    DataGridViewRow row = dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex];
                    DataGridViewLinkCell Lincell = new DataGridViewLinkCell();
                    dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex] = Lincell;
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
            this.ds2=new DataSet();
            button5.BackColor = Color.Red;
            button5.Enabled = false;
            value_toinsert = 100 / (ds.Tables.Count-1) ;
            this.panel7.Visible = true;
            DataTable dtt2 ;
            gotovo.Clear();
            comboBox2.Items.Clear();
            foreach (DataTable item in ds.Tables)
            {
                dtt2 = new DataTable(item.TableName);
                if (item.TableName != "Дела")
                {
                this.comboBox2.Items.Add(item.TableName);
                }
                    for (int i = 0; i < item.Columns.Count; i++)
                    {
                        if (item.Columns[i].ColumnName != "WOC")
                        {
                        
                            dtt2.Columns.Add(item.Columns[i].ColumnName);
                            if (item.Columns[i].ColumnName=="Conf_id" || item.Columns[i].ColumnName == "Таблица")
                            {
                                switch (item.Columns[i].ColumnName)
                                {
                                    case "Conf_id":

                                        break;
                                    case "Таблица":
                                           break ;
                                }
                            }
                        }
                    }
                dtt2.Rows.Add(dtt2.NewRow());
                    this.ds2.Tables.Add(dtt2);
            }
            this.comboBox2.SelectedIndex = 0;
        }

        private void panel7_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView3.DataSource = ds2.Tables[comboBox2.Text];

            if (dataGridView3.Columns.Contains("Conf_id") == true)
            {
                dataGridView3.Columns["Conf_id"].Visible = dataGridView3.Columns["Таблица"].Visible = false;
            }
            if (dataGridView3.Columns.Contains("User") == true)
            {
                dataGridView3.Columns["User"].Visible = false;
            }
            if (gotovo.Contains(comboBox2.Text) == true)
            {
                dataGridView3.Enabled = false;
            }
            else
            {
                dataGridView3.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
            GloBal.SQL.connect();
                int cc = GloBal.SQL.Save_new_Дел(ds2, GloBal.Conf_oll.us.ID, id_table);
                if (cc > 0)
                {
                    MessageBox.Show("Добовление дела прошло успешно его id = "+cc, "Добовление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.progressBar2.Value = 0;
                button5.BackColor = System.Drawing.Color.Red;
                button5.Enabled=false;
                ds2=new DataSet();
                button4_Click(button4,null);
                panel7.Visible=false;
                button3_Click(button3,null);
            GloBal.SQL.clouse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
        }



        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string filePat = "";
                GloBal.SQL.connect();
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePat = openFileDialog.FileName;

                    }
                }
                if (filePat != "")
                {
                    panel1.Size = new Size(panel1.Width, 44);
                    progressBar1.Visible = true;

                    panel7.Visible=panel8.Visible=false;
                    panel6.Enabled = panel6.Visible = true;
                    panel1.Enabled = panel3.Enabled = false;
                    DataSet dss = open_exel(filePat);
                    this.progressBar1.Value = GloBal.loat_line;
                    id_table = GloBal.SQL.Save_new_exel(dss, GloBal.Conf_oll.us.ID,id_table);
                    this.progressBar1.Value = GloBal.loat_line;
                    ds = GloBal.SQL.TO_greadviu_2(GloBal.Conf_oll.us.ID, id_table);
                    comboBox1.Items.Clear();
                    foreach (DataTable item in ds.Tables)
                    {
                        this.comboBox1.Items.Add(item.TableName);
                    }
                    this.label7.Text = "таблица :" + id_table.ToString();
                    this.comboBox1.SelectedIndex = 0;
                    
                    button2_Click(this.button2, null);
                    progressBar1.Visible = false;
                    dataGridView2.Columns["Conf_id"].ReadOnly = dataGridView2.Columns["Таблица"].ReadOnly = true;
                }
                GloBal.SQL.clouse();
                panel6.Visible = false;
                this.Enabled = panel1.Enabled = panel3.Enabled = true;
                GloBal.loat_line = progressBar1.Value = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void button9_Click(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            ds2=new DataSet();
            foreach (DataTable item in ds.Tables)
            {
            dt=new DataTable(item.TableName);
            comboBox3.Items.Add(item.TableName);
                
                for (int c = 0; c < item.Columns.Count; c++ )
                {
                    if ($"{item.Columns[c]}" != "WOC")
                    {
                    dt.Columns.Add($"{item.Columns[c]}");
                    }
                }
                dt.Rows.Add(dt.NewRow());
                ds2.Tables.Add(dt);
            }
            dataGridView4.AllowUserToAddRows = false;
            this.panel8.Visible = true;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView4.DataSource = ds2.Tables[comboBox3.Text];
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                ds2.Tables.Clear();
                ds2.Tables .Add((DataTable)dataGridView4.DataSource);
                int cc = 0;
                foreach (DataTable item in ds2.Tables)
                {
                    for (int i = 0; i < item.Columns.Count; i++)
                    {
                        if ($"{item.Rows[0][i]}" != "")
                            cc += 1;

                    }
                }
                if (cc>0)
                {
                    GloBal.SQL.connect();
                    ds=GloBal.SQL.serch_Дело(ds2.Tables[comboBox3.Text],GloBal.Conf_oll.us.ID,id_table);
                    GloBal.SQL.clouse();
                    comboBox1.Items.Clear();
                    if (ds.Tables.Count!=0)
                    {

                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            comboBox1.Items.Add((string)$"{ds.Tables[i].TableName}");
                        }
                        comboBox1.SelectedIndex = 0;
                        dataGridView2.DataSource = ds.Tables[comboBox1.Text];
                        button10_Click(button10,null);
                        ds2.Tables.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Нет значений");
                    }
                }
                else
                {
                    MessageBox.Show("Нет значений");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
       

        string[] arr_Table = new string[] { "Процесс окозания услуги",
            "Общие_данные" };
        string[] arr_colom = new string[] { "Текущий статус услуги",/*0*/ 
            "МФЦ, в котором зарегистрировано дело","ФИО оператора внесшего последние изменения в дело","ФИО оператора создавшего дело","ФИО оператора зарегистрировавшего дело","ФИО оператора выдавшего дело"/*1-5*/
        };

        private void button13_Click(object sender, EventArgs e)
        {
            if (GloBal.Logining == null)
            {
                GloBal.Logining = new Form1();
            }
            GloBal.Logining.Show();
            GloBal.main_Worc.Hide();
            GloBal.main_Worc = null;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }



        private void panel4_Click(object sender, EventArgs e)
        {
            button2_Click(button2,null);
            switch (panel4.Size.Height)
            {
                case 44:
                    if ( ds != null && ds.Tables.Count>0 )
                    {
                        panel4.Size = new Size(panel4.Size.Width,75);
                        panel9.Visible = true;
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                        this.comboBox4.Items.Add((string)$"{ds.Tables[i]}");
                        }
                        comboBox4.SelectedIndex = 0;
                    }
                    break;
                case 75:
                    panel4.Size = new Size(panel4.Size.Width, 44);
                    panel9.Visible = false;
                    comboBox4.Items.Clear();
                    break;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //колонки таблици     значение из колонки       доп значения
            //comboBox11          comboBox10 
            //combobox9           combobox12                combobox15 ровно или нет
            //combobox6           combobox7 combobox8
            //combobox13          combobox14                combobox16 список значений
            comboBox11.Items.Clear();
            comboBox9.Items.Clear();
            comboBox6.Items.Clear();
            comboBox13.Items.Clear();

            //comboBox11.SelectedIndex = 0; /*comboBox10.SelectedIndex = 0;*/
            //comboBox9.SelectedIndex = 0;/* comboBox12.SelectedIndex = comboBox15.SelectedIndex = 0;*/
            //comboBox6.SelectedIndex = 0; /*comboBox7.SelectedIndex= comboBox8.SelectedIndex = 0;*/
            //comboBox13.SelectedIndex = 0; /*comboBox14.SelectedIndex = comboBox16.SelectedIndex= 0;*/
            for (int i = 0; i < ds.Tables[comboBox4.Text].Columns.Count; i++)
            {

                if ($"{ds.Tables[comboBox4.Text].Columns[i].ColumnName}" != "WOC" && $"{ds.Tables[comboBox4.Text].Columns[i].ColumnName}" != "")
                {
                comboBox11.Items.Add(ds.Tables[comboBox4.Text].Columns[i].ColumnName);
                comboBox9.Items.Add(ds.Tables[comboBox4.Text].Columns[i].ColumnName);
                comboBox6.Items.Add(ds.Tables[comboBox4.Text].Columns[i].ColumnName);
                comboBox13.Items.Add(ds.Tables[comboBox4.Text].Columns[i].ColumnName);
                }

            }
            comboBox11.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox13.SelectedIndex = 0;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button16_Click(button6,null);
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            //колонки таблици     значение из колонки       доп значения
            //combobox9           combobox12                combobox15 ровно или нет
            comboBox12.Items.Clear();
            for (int i = 0; i < ds.Tables[comboBox4.Text].Rows.Count; i++)
            {
                string str = $"{ds.Tables[comboBox4.Text].Rows[i][comboBox9.Text]}";
                str.TrimEnd(' ');
                if (comboBox12.Items.Contains(str) !=true)
                {
                comboBox12.Items.Add(str);

                }
            }
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            //колонки таблици     значение из колонки       доп значения
            //comboBox11          comboBox10 

            comboBox10.Items.Clear();
            for (int i = 0; i < ds.Tables[comboBox4.Text].Rows.Count; i++)
            {
                string str = $"{ds.Tables[comboBox4.Text].Rows[i][comboBox11.Text]}";
                str.TrimEnd(' ');
                if (comboBox10.Items.Contains(str) !=true)
                {
                comboBox10.Items.Add(str);

                }
            }
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox15.Items.Clear();
            comboBox15.Items.Add($"<");
            comboBox15.Items.Add($">");
            comboBox15.Items.Add($"=");
            comboBox15.Items.Add($"<=");
            comboBox15.Items.Add($">=");
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //колонки таблици     значение из колонки       доп значения
            //combobox6           combobox7 combobox8

            comboBox7.Items.Clear();
            comboBox8.Items.Clear();
            for (int i = 0; i < ds.Tables[comboBox4.Text].Rows.Count; i++)
            {
                string str = $"{ds.Tables[comboBox4.Text].Rows[i][comboBox6.Text]}";
                str=str.TrimEnd(' ');
                if (comboBox7.Items.Contains(str) !=true)
                {
                comboBox7.Items.Add(str);
                comboBox8.Items.Add(str);
                }
            }

        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox14.Items.Clear();
            for (int i = 0; i < ds.Tables[comboBox4.Text].Rows.Count; i++)
            {
                string str = $"{ds.Tables[comboBox4.Text].Rows[i][comboBox13.Text]}";
                str = str.TrimEnd(' ');
                if (comboBox14.Items.Contains(str)!=true)
                {
                comboBox14.Items.Add(str);
                }
            }
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                string select = $"select ";
                string select2 = $" FROM [{comboBox4.Text}] ";
                string select3 = $" where  ";
                List<string> table_colum_list = new List<string>();
                int c=0;
                int c1=0;
                if (this.checkBox1.Checked == true) /*выбрать всё*/
                {
                    c++; 
                    select += " * ";
                }
               if (this.checkBox2.Checked == true) /*сумма*/
               {
                   
                    if (checkBox1.Checked == true || c>0 )
                        {
                        select += ", ";
                        }
                    if (checkBox6.Checked == true)
                    {
                        select += $" count([{comboBox11.Text}]) AS [SUM_{comboBox11.Text}]";

                    }
                    else 
                    {
                    select += $" count( distinct [{comboBox11.Text}]) AS [SUM_{comboBox11.Text}]";
                    }

                    if (comboBox10.Text!="")
                    {
                        c1++;
                        select3 += $"[{comboBox11.Text}]=N'{comboBox10.Text}'";
                    }
                }
               if (this.checkBox3.Checked == true ) /*соответствие*/
               {
                    if  (c1 >0 )
                        {
                        select3 += "AND ";
                        }
                    c1++;
                    select3 += $"[{comboBox9.Text}]{comboBox15.Text}N'{comboBox12.Text}'";
               }
               if (this.checkBox5.Checked == true ) /*промежуток*/
               {
                    if (comboBox10.Text != "" || c1>0)
                    {
                        select3 += " AND ";
                    }
                    c1++;
                    select3 += $"[{comboBox6.Text}]>=N'{comboBox7.Text}' and [{comboBox6.Text}] <= N'{comboBox8.Text}'";
                }
               if (this.checkBox4.Checked == true ) /*список*/
               {
                        if (c1>0)
                        {
                        select3 += " AND ";
                        }
                    c1++;
                    string ar="";
                    foreach (string item in comboBox16.Items)
                    {

                        ar += $" N'{item}' ,";
                    }
                    ar = ar.TrimEnd(',');
                    select3 += $"[{comboBox13.Text}] IN ({ar})";
               }
                if (select3== " where  ")
                {
                    select3 = null;
                }
                textBox1.Text=select + select2 + select3;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            button16_Click(button6, null);

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            button16_Click(button6, null);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            button16_Click(button6, null);

        }

        private void button15_Click(object sender, EventArgs e)
        {
            GloBal.SQL.connect();

            dt = GloBal.SQL.select__(this.textBox1.Text);
            dataGridView5.DataSource  = dt;
            GloBal.SQL.clouse();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (comboBox14.Text!="")
            {
             comboBox16.Items.Add(comboBox14.Text);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (comboBox16.Text=="")
            {
                comboBox16.Items.Clear();
            }
            comboBox16.Items.Remove(comboBox16.Text);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button16_Click(button6, null);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            ds_save.Tables.Add(dt);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Click(object sender, EventArgs e)
        {
            switch (panel5.Height)
            {
                case 44:
                    panel5.Size = new System.Drawing.Size(panel5.Width,70);
                    panel15.Visible = true;
                    for (int i = 0; i < ds3.Tables.Count; i++)
                    {
                        comboBox5.Items.Add(ds3.Tables[i].TableName);
                    }
                    if (comboBox5.Items.Count!=0)
                    {
                    comboBox5.SelectedIndex = 0;
                    }
                    break;
                case 70:
                    panel15.Visible = false;
                    panel5.Size = new System.Drawing.Size(panel5.Width, 44);
                    break;
            }
        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GloBal.Conf_oll.us.requests.Count!=0)
            {
                for (int i = 0; i < GloBal.Conf_oll.us.requests.Count; i++)
                {
                    if (comboBox17.Text== GloBal.Conf_oll.us.requests[i][0])
                    {
                        textBox1.Text = GloBal.Conf_oll.us.requests[i][1];
                    }
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text!=null)
            {
                this.button18.BackColor = System.Drawing.Color.Green;
                this.button18.Enabled=true;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.button18.BackColor == System.Drawing.Color.Green)
                {
                GloBal.Conf_oll.us.requests.Add(new string[]{textBox3.Text,textBox1.Text });
                this.button18.Enabled = false;
                }
                else
                {
                this.button18.BackColor = System.Drawing.Color.Red;
                }
                comboBox17.Items.Clear();
                if (GloBal.Conf_oll.us.requests.Count != 0)
                {
                    for (int i = 0; i < GloBal.Conf_oll.us.requests.Count; i++)
                    {
                        comboBox17.Items.Add(GloBal.Conf_oll.us.requests[i][0]);
                    }
                    comboBox17.SelectedIndex = 0;
                }
                GloBal.load_Conf.seri_s_oll(GloBal.Conf_oll);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message); 
            }
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            if (dataGridView5.Rows.Count!=0 && dt.Rows.Count!=0) 
            {
                dt.TableName = textBox4.Text;
                ds3.Tables.Add(dt);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView6.DataSource = ds3.Tables[comboBox5.Text];
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(ds3.Tables[comboBox5.Text].Rows.Count != 0 && textBox2.Text!=null)
            {
                button19.Enabled = true;
                button19.BackColor = Color.Green;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (ds3.Tables[comboBox5.Text].Rows.Count != 0 && textBox2.Text != null)
            {
                SaveFileDialog SFD = new SaveFileDialog();
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    // Get DataTable that is data bound to DataGridView.
                    DataTable table = (DataTable)dataGridView6.DataSource;

                    //Creae an Excel application instance
                    Excel.Application excelApp = new Excel.Application();

                    //Create an Excel workbook instance and open it from the predefined location
                    //ERROR




                    Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(1);
                    panel5_Click(panel5,null);
                    panel6.Visible = true;
                    int cc =  65/ table.Rows.Count;

                    //Add a new worksheet to workbook with the Datatable name
                    Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                    progressBar1.Value = 25;
                    if (textBox2.Text.Length <= 30)
                    {
                    excelWorkSheet.Name = textBox2.Text;
                    }
                    else
                    {
                        MessageBox.Show("Превышен размер имени выгрузка назавётся Выгрузка");
                    excelWorkSheet.Name = "Выгрузка";
                    }

                    for (int i = 1; i < table.Columns.Count + 1; i++)
                    {
                        excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                    }
                    progressBar1.Value = 35;

                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        for (int k = 0; k < table.Columns.Count; k++)
                        {
                            excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                        }
                        progressBar1.Value += cc;
                    }


                    excelWorkBook.SaveAs(SFD.FileName);
                    excelWorkBook.Close();

                    excelApp.Quit();
                    progressBar1.Value = 100;
                    panel6.Visible = false;
                    ds3 = new DataSet();
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text!=null)
            {
                button11.Enabled = true;
                button11.BackColor = Color.Green;
            }
            else
            {
                button11.Enabled = false;
                button11.BackColor = Color.Red;
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (gotovo.Contains(comboBox2.Text) ==false) 
            {
            gotovo.Add(comboBox2.Text);
                if (progressBar2.Value!=100)
                {
                progressBar2.Value += value_toinsert;
                }
                if (comboBox2.SelectedIndex < comboBox2.Items.Count-1)
                {
                this.comboBox2.SelectedIndex = comboBox2.SelectedIndex+1;
                }
            }
            if(progressBar2.Value < 100 && progressBar2.Value > 95)
            {
                progressBar2.Value = 100;
                button5.BackColor = Color.LightGreen;
                button5.Enabled = true;
                gotovo.Clear();
            }
        }
    }
}
