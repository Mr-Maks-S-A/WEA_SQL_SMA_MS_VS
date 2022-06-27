using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WEA_SQL
{
    internal class SQL_DB_WOC
    {
        public  string LOg_base = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename= |DataDirectory|\Lokal_test.mdf;Integrated Security=True";
        public SqlConnection sqlConnection ;
        public void connect() {
            try
            {

            if (sqlConnection == null)
            {
                    sqlConnection = new SqlConnection(LOg_base);
            }
            if (this.sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                this.sqlConnection.Open();
            }
            }
            catch (Exception ex)
            {
            }
        }
        public void clouse()
        {
            try
            {
            if (this.sqlConnection.State == System.Data.ConnectionState.Open)
            {
                this.sqlConnection.Close();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        public SQL_DB_WOC() 
        {
            sqlConnection = new SqlConnection(LOg_base);
        }
        public SQL_DB_WOC(string str)
        {
            try
            {
            LOg_base=str;
            sqlConnection = new SqlConnection(LOg_base);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при подключении к базе данных просим проверить параметры подключения","SQL error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        ////////////////////////////////
        public bool LOgininng(string Login, string Password) /*для поключения к базе данных проверка пользователя*/
        {
            try
            {

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT [id],[Nik_name] FROM [User] WHERE [Login]=@log and [Password]=@pas ", sqlConnection);
                cmd.Parameters.Add("@log", SqlDbType.NVarChar).Value = Login;
                cmd.Parameters.Add("@pas", SqlDbType.NVarChar).Value = Password;
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dt);
            
                if (dt.Rows.Count > 0) 
                {
                    GloBal.Conf_oll.us.ID = (int)dt.Rows[0][0];
                    GloBal.Conf_oll.us.Name= (string)dt.Rows[0][1];
                    GloBal.Conf_oll.us.Login=Login ;
                    GloBal.Conf_oll.us.Passwword=Password;
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public DataTable Admin_oll_user() /*загрузка всех пользователей которые ксть в базе*/
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("select 'DEL'as'WOC',* from [User] ;", sqlConnection);

            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public void Admin_insert_user(DataRow row) /*добовление пользователя*/
        {
            DataTable dt=new DataTable();
            string sql = $"insert into [User](Nik_name,Login,Password) values ('{row[2]}','{row[3]}','{row[4]}');";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);

            if (cmd.ExecuteNonQuery()<1)
            {
                throw new Exception("ошипка с регистрацией");
            }

        }
        public void Admin_del_user(int id) /*удаление пользователя базы*/
        {
            SqlCommand cmd = new SqlCommand((string)$"Delete FROM [User] WHERE [id]={id}", sqlConnection);
            sqlDataAdapter.SelectCommand = cmd;
            if (cmd.ExecuteNonQuery() < 1)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при удалении таблици дел");
            }
        }
        public void Admin_update_user(DataRow row) /*обновление пользователя базы*/
        {
            SqlCommand cmd = new SqlCommand((string)$"UPDATE [User] set [User].Nik_name='{row[2]}',[User].Login='{row[3]}',[User].Password='{row[4]}' where [User].id={row[1]};", sqlConnection);
            sqlDataAdapter.SelectCommand = cmd;
            if (cmd.ExecuteNonQuery() < 1)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при обновлении пользователя");
            }
        }

        public void Admin_del_user_2(string id, string Name=null, string Login = null, string Password = null) /*удаление пользователя*/
        {
            string sql= (string)$"Delete FROM [User] WHERE [id]={id}";
            if (Name != null && Login != null && Password != null)
            {
                sql = (string)$"Delete FROM [User] WHERE [id]={id} and [Nik_name]={Name} and [Login]={Login} and [Password]={Password}";
            }
            else 
            {
                if (Name != null)
                {
                    sql += $" and [Nik_name]={Name}";
                }
                 if (Login != null) 
                {
                    sql += $" and [Login]={Login}";
                }
                 if (Password != null)
                {
                    sql += $" and [Password]={Password}";
                }
            }
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            sqlDataAdapter.SelectCommand = cmd;
            if (cmd.ExecuteNonQuery() < 1)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при удалении ");
            }
        }
        public void Admin_del_user_For_id(string id1, string id2) /*удаление пользователей*/
        {
            string sql = (string)$"Delete FROM [User] WHERE [id]>={id1} and [id]<={id2}";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            sqlDataAdapter.SelectCommand = cmd;
            if (cmd.ExecuteNonQuery() < 1)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при удалении ");
            }
        }

        public DataTable Admin_serch_user_2(string id, string Name = null, string Login = null, string Password = null) /*поиск пользователя*/
        {
            DataTable dt = new DataTable();
            string sql = (string)$"Select 'DEL'as'WOC', * FROM [User] WHERE [id]={id}";
            if (Name != null && Login != null && Password != null)
            {
                sql = (string)$"Select 'DEL'as'WOC', * FROM [User] WHERE [id]={id} and [Nik_name]={Name} and [Login]={Login} and [Password]={Password}";
            }
            else
            {
                if (Name != null)
                {
                    sql += $" and [Nik_name]={Name}";
                }
                if (Login != null)
                {
                    sql += $" and [Login]={Login}";
                }
                if (Password != null)
                {
                    sql += $" and [Password]={Password}";
                }
            }
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(dt);
            return dt;
        }
        public DataTable Admin_serch_user_For_id(string id1, string id2) /*поиск пользователей */
        {
            DataTable dt = new DataTable();
            string sql = (string)$"Select 'DEL'as'WOC', * FROM [User] WHERE [id]>={id1} and [id]<={id2}";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(dt);
            return dt;
        }


        public DataTable TO_greadviu_1(int _user_id)/* начальная загрузка всех таблиц пользователя*/ 
        {
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT 'Woc' AS 'Продолжить работу','Delete' AS [Delete], Таблица AS ID_table, COUNT(*) AS 'Дел' " +
                                    "FROM dbo.Дела " +
                                    "where [User]=@id_us " +
                                    "GROUP BY Таблица HAVING COUNT(*) >= 1; ", sqlConnection);
            cmd.Parameters.Add("@id_us", SqlDbType.Int).Value = _user_id;

            sqlDataAdapter = new SqlDataAdapter(cmd);
            sqlDataAdapter.Fill(dataTable);

            return dataTable;

        }
        public DataSet TO_greadviu_2(int _user_id,int id_table)/*загрузка таблици дел из базы данных*/
        {
                DataSet ds = new DataSet();
                DataTable dataTable = new DataTable();
                sqlDataAdapter = new SqlDataAdapter();
                string[] arr_table = new string[] { "Дела","Общие_данные", "Процесс окозания услуги", "Данные_услуги",
                                                "Данные ФЛ/ИП", "Данные ЮЛ", "Сроки и даты", "Сведения по обьектам недвижимости" };

                SqlCommand cmd = new SqlCommand((string)$"SELECT [Conf_ID]  FROM [Дела] WHERE [Таблица]={id_table} and [User]={_user_id} ", sqlConnection);
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dataTable);
                string list_conf_id = "(";
                foreach (DataRow item in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++) 
                    {
                    list_conf_id += $"{item[i]} ,";
                    }
                }
             list_conf_id=list_conf_id.TrimEnd(',');
             list_conf_id += ")";

            for (int i = 0; i < 8; i++)
                {
                    cmd = new SqlCommand("SELECT 'DELETE'AS [WOC],* FROM [" + arr_table[i] + "] WHERE [Conf_ID] IN "+ list_conf_id, sqlConnection);
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(ds, arr_table[i]);
                }
            return ds;
        }

        public void DEL_user_table(int _user_id, int id_table)/*Удаление таблици данных пользователя*/
        {
            SqlCommand cmd = new SqlCommand((string)$"Delete FROM [Дела] WHERE [User]={_user_id} and [Таблица]={id_table}", sqlConnection);
            if (cmd.ExecuteNonQuery() < 1)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при удалении таблици или такой таблици не существует");
            }
        }
        public int Save_new_exel(DataSet ds, int user_Id,int id_table) /*сохранение таблиц дел*/
        {
            if (ds.Tables.Contains("Дела") != true && id_table == 0)
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT MAX(Conf_ID) as max_ID,MAX(Таблица) as [Таблица] FROM Дела ", sqlConnection);
                sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dt);
                int[] arr_id;
                if ($"{dt.Rows[0][0]}" == "")
                {
                    arr_id=new int[]{1,1, user_Id };
                }
                else
                {
                arr_id = new int[] {
                Int32.Parse(dt.Rows[0][0].ToString())+1/* ID последнего дела*/,
                Int32.Parse(dt.Rows[0][1].ToString())+1/* ID последней таблици*/,
                user_Id
                };
                }       
                GloBal.loat_line = 50;
                GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                int r_col = ds.Tables["Общие_данные"].Rows.Count;

                List<string> string_value = new List<string>();
                string str_1 = " ";
                for (int i = 0; i < r_col; i++)
                {
                    str_1 = str_1 + (string)$"({arr_id[0] + i },{arr_id[1]},{user_Id}),";
                }
                str_1 = str_1.TrimEnd(','); string_value.Add(str_1);
                cmd = new SqlCommand("Insert into  [Дела] Values " + string_value[0] + ";", sqlConnection);
                cmd.ExecuteNonQuery();

                string[] str_2 = new string[7];
                int qwe = 0;
                GloBal.loat_line = 55;
                GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                foreach (DataTable item in ds.Tables)
                {
                    string ccc = $"Insert into [{item.TableName}] (";
                    ccc = ccc + $" [Conf_ID] ,[Таблица], ";
                    for (int i = 0; i < item.Columns.Count; i++)
                    {
                        if (item.TableName == "Данные ЮЛ" && item.Columns[i].ColumnName == "ФИО_1")
                        {
                            ccc = ccc + "[ФИО],";
                        }
                        else
                        {
                            ccc = ccc + "[" + item.Columns[i].ColumnName + "],";
                        }
                    }
                    ccc = ccc.TrimEnd(',');
                    ccc = ccc + ") VALUES ";
                    for (int j = 0; j < r_col; j++)
                    {
                        ccc = ccc + "(";
                        ccc = ccc + $"{arr_id[0] + j },{arr_id[1]}, ";
                        for (int k = 0; k < item.Columns.Count; k++)
                        {
                            //значения строк
                            if (item.Rows[j][k].ToString() != "")
                            {
                                ccc = ccc + " N'" + $"{item.Rows[j][k]}" + "',";
                            }
                            else
                            {
                                ccc = ccc + "null,";
                            }
                        }
                        ccc = ccc.TrimEnd(',');
                        ccc = ccc + "),";
                    }
                    ccc = ccc.TrimEnd(',');
                    str_2[qwe] = ccc;
                    qwe++;
                    GloBal.loat_line += 3;/*55+21=76+21=97*/
                    GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                }
                foreach (var item in str_2)
                {
                    cmd = new SqlCommand(item, sqlConnection);
                    cmd.ExecuteNonQuery();
                    GloBal.loat_line += 3;/*97*/
                    GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                }

                return arr_id[1];
            }
            else if (ds.Tables.Contains("Дела") != true && id_table != 0) 
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT MAX(Conf_ID) as max_ID FROM Дела ", sqlConnection);
                sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dt);
                int[] arr_id = new int[] {
                Int32.Parse(dt.Rows[0][0].ToString())/* ID последнего дела*/,
                id_table/* ID последней таблици*/,
                user_Id
                };
                GloBal.loat_line = 50;
                GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                int r_col = ds.Tables["Общие_данные"].Rows.Count;

                List<string> string_value = new List<string>();
                string str_1 = " ";
                for (int i = 0; i < r_col; i++)
                {
                    str_1 = str_1 + (string)$"({arr_id[0] + i + 1},{arr_id[1]},{user_Id}),";
                }
                str_1 = str_1.TrimEnd(','); string_value.Add(str_1);
                cmd = new SqlCommand("Insert into  [Дела] Values " + string_value[0] + ";", sqlConnection);
                cmd.ExecuteNonQuery();

                string[] str_2 = new string[7];
                int qwe = 0;
                GloBal.loat_line = 55;
                GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                foreach (DataTable item in ds.Tables)
                {
                    string ccc = $"Insert into [{item.TableName}] (";
                    ccc = ccc + $" [Conf_ID] ,[Таблица], ";
                    for (int i = 0; i < item.Columns.Count; i++)
                    {
                        if (item.TableName == "Данные ЮЛ" && item.Columns[i].ColumnName == "ФИО_1")
                        {
                            ccc = ccc + "[ФИО],";
                        }
                        else
                        {
                            ccc = ccc + "[" + item.Columns[i].ColumnName + "],";
                        }
                    }
                    ccc = ccc.TrimEnd(',');
                    ccc = ccc + ") VALUES ";
                    for (int j = 0; j < r_col; j++)
                    {
                        ccc = ccc + "(";
                        ccc = ccc + $"{arr_id[0] + j + 1},{arr_id[1]}, ";
                        for (int k = 0; k < item.Columns.Count; k++)
                        {
                            //значения строк
                            if (item.Rows[j][k].ToString() != "")
                            {
                                ccc = ccc + " N'" + $"{item.Rows[j][k]}" + "',";
                            }
                            else
                            {
                                ccc = ccc + "null,";
                            }
                        }
                        ccc = ccc.TrimEnd(',');
                        ccc = ccc + "),";
                    }
                    ccc = ccc.TrimEnd(',');
                    str_2[qwe] = ccc;
                    qwe++;
                    GloBal.loat_line += 3;/*55+21=76+21=97*/
                    GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                }
                foreach (var item in str_2)
                {
                    cmd = new SqlCommand(item, sqlConnection);
                    cmd.ExecuteNonQuery();
                    GloBal.loat_line += 3;/*97*/
                    GloBal.main_Worc.progressBar1.Value = GloBal.loat_line;
                }
                GloBal.loat_line = 100;
                GloBal.main_Worc.progressBar1.Value = 100;
                System.Windows.Forms.MessageBox.Show("Дела записаны в таблицу номер :"+ arr_id[1]+"\n а номер первого ID дела ="+ arr_id[0], "Дозапись",MessageBoxButtons.OK,MessageBoxIcon.Information);

                return arr_id[1];
            }
            else
            {
                return -1;
            }
        }
        public void DEL_Дело(int _user_id, int id_table,int conf_id)/*Удаление таблици данных пользователя*/
        {
            SqlCommand cmd = new SqlCommand((string)$"Delete FROM [Дела] WHERE [User]={_user_id} and [Таблица]={id_table} and [Conf_ID]={conf_id}", sqlConnection);
            if (cmd.ExecuteNonQuery() < 1)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при удалении дела");
            }
        }
        public int Update_Дело(string name_table, int conf_id ,DataRow row)/*Удаление таблици данных пользователя*/
        {
            string str = $"UPDATE [{name_table}] set ";
            for (int i = 2; i < row.Table.Columns.Count; i++)
            {
                if (row[i]!=null)
                {
                str += $"[{row.Table.Columns[i]}]=N'{row[i]}' ,";
                }
                else
                {
                str += $"[{row.Table.Columns[i]}]=null ,";
                }
            }
            str=str.TrimEnd(',');
            str += $"WHERE [Conf_ID]={conf_id}";
            SqlCommand cmd = new SqlCommand(str, sqlConnection);
            if (cmd.ExecuteNonQuery() == 0)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при обновлении дела");
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public int Save_new_Дел(DataSet ds, int user_Id, int table_Id) /*сохранение таблиц дел*/
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand((string)$"SELECT MAX(Conf_ID) as max_ID FROM Дела where [User]={user_Id}", sqlConnection);
                sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dt);
                int Conf_id = Int32.Parse(dt.Rows[0][0].ToString())+1/* ID последнего дела*/;
                string str = "";
                List<string> list = new List<string>();
                int bb = 0;
                foreach (DataTable item in ds.Tables)
                {
                    if (item.Rows.Count!=0 && bb< item.Rows.Count)
                    {
                        bb = item.Rows.Count;
                    }
                }
                 foreach (DataTable item in ds.Tables)
                {
                    if (bb != 0 &&(item.Rows.Count!=0 || item.TableName=="Дела"))
                    {
                        str = $"Insert into [{item.TableName}] (";
                        for (int i = 0; i < item.Columns.Count; i++)
                        {
                            str += $"[{item.Columns[i]}] ,";
                        }
                        str = str.TrimEnd(',');
                        str += ") Values (";
                        for (int i = 0; i < item.Columns.Count; i++)
                        {
                            if (item.TableName == "Дела")
                            {
                                str = str.TrimEnd('(');
                                str += $" ({Conf_id} ,{table_Id} ,{user_Id} ) ,";
                                i = item.Columns.Count;
                            }
                            else if ((string)$"{item.Columns[i]}" == "Conf_ID" || (string)$"{item.Columns[i]}" == "Таблица") 
                            {
                                switch ((string)$"{item.Columns[i]}")
                                {
                                    case "Conf_ID":
                                        str += $" {Conf_id} ,";
                                        break;
                                    case "Таблица":
                                        str += $" {table_Id} ,";
                                        break;
                                }
                            }
                            else if (item.Rows[0][i] == null || item.Rows[0][i].ToString() == "")
                            {
                                str += $" null ,";
                            }
                            else
                            {
                                str += $"N'{item.Rows[0][i]}',";
                            }
                        }
                        str = str.TrimEnd(',');
                        if (item.TableName != "Дела")
                            str += ")";
                        list.Add(str);
                    }
               }
                    if (list.Count!=0)
                    {
                        foreach (string item2 in list)
                        {
                            cmd = new SqlCommand(item2, sqlConnection);
                            cmd.ExecuteNonQuery();
                        }
                    }
                return Conf_id;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
               
        }

        public DataSet serch_Дело(DataTable dt ,int Id_user ,int Id_table) 
        {
            DataSet ds = new DataSet();
            DataTable dt2 = new DataTable();
            try
            {
                
                string[] arr_table = new string[] { "Дела","Общие_данные", "Процесс окозания услуги", "Данные_услуги",
                                                "Данные ФЛ/ИП", "Данные ЮЛ", "Сроки и даты", "Сведения по обьектам недвижимости" };
                

                string str = $"SELECT Conf_ID from [{dt.TableName}] where ";

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if ( $"{dt.Rows[0][i]}" != "")
                        {
                        str += $"  [{dt.Columns[i].ColumnName}] = N'{dt.Rows[0][i]}'  and";
                        }
                    }
                str = str.TrimEnd('d');
                str = str.TrimEnd('n');
                str = str.TrimEnd('a');
                SqlCommand cmd = new SqlCommand(str , sqlConnection);
                sqlDataAdapter.SelectCommand = cmd;
                dt=new DataTable();
                sqlDataAdapter.Fill(dt);
                string str2="";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str2 += $"{dt.Rows[i][0]} ,";
                }
                str2= str2.TrimEnd(',');

                str = $"SELECT [Conf_id] from [Дела] where [Conf_ID] IN ({str2}) and [User]={Id_user} and [Таблица]={Id_table} ";
                cmd = new SqlCommand(str, sqlConnection);
                sqlDataAdapter.SelectCommand = cmd;
                dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                str2 = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str2 += $"{dt.Rows[i][0]} ,";
                }
                str2 = str2.TrimEnd(',');

                if (str2 != "")
                {

                    foreach (string item in arr_table)
                    {
                        str = $"SELECT 'DELETE' AS [WOC],* from [{item}] where [Conf_ID] IN ({str2}) ORDER BY [Conf_ID] DESC ";

                        cmd = new SqlCommand(str, sqlConnection);
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(ds, item);
                    }
                }
                else 
                {

                MessageBox.Show("Ничего нет");
                }
            return ds;
            }
            catch (Exception ex)
            {
            return ds;
                MessageBox.Show("ВЫ ввели некоректный запрос");
            }
        }
        ////////////////////////////////
        //ЗАПРОСЫ 
        public DataTable select__(string cmd_)
        {
            DataTable dt=new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(cmd_, sqlConnection);
                sqlDataAdapter.SelectCommand = cmd;
                sqlDataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        //ЗАПРОСЫ 
        ////////////////////////////////

    }
}