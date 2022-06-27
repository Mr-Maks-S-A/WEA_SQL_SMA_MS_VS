using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WEA_SQL
{
    [Serializable]
    class user
    {
        public string Name;
        public string Login;
        public string Passwword;
        public int ID;
        public List<string[]> requests=new List<string[]>();

        public user(string name, string login , string password , int iD )
        {
            requests=new List<string[]>();
            Name = name;
            Login = login;
            Passwword = password;
            ID = iD;
        }
        public user(){
            Name = null;
            Login = null;
            Passwword = null;
        }
    }

    [Serializable]
    class udmin_conf
    {
        public string Server;
        public string DataBase;
        public string Login;
        public string Passwword;
        public string Sql_db_serv=null; /*строка подключения к базе данных*/
        public bool Auto_login=false;    /*переменная для инициализации автоматичесского входа если все проверки прошли успешно*/

        public udmin_conf(string login, string password, string sql_db_serv, bool auto_log)
        {
            Login = login;
            Passwword = password;
            Sql_db_serv = sql_db_serv;
            Auto_login = auto_log;
        }
        public udmin_conf(){}
    }

    [Serializable]
    class Serialise_oll
    {
        public user us=new user();
        public udmin_conf adm=new udmin_conf();
        public Serialise_oll(user _us, udmin_conf _adm )
        {
            us = _us;
            adm = _adm;
        }
        public  Serialise_oll() { }
    }

    class load_conf
    {
        BinaryFormatter BF = new BinaryFormatter();
        string F_N = "Serialise_conf.bin";

        public void seri_s_oll(Serialise_oll file, string file_name = null)
        {
            if (file_name == null)
            {
                file_name = F_N;
            }
            using (var FL = new FileStream(file_name, FileMode.OpenOrCreate))
            {
                BF.Serialize(FL, file);
            }
        }
        public Serialise_oll deser_s_oll(  string file_name = null)
        {
            if (file_name == null)
            {
                file_name = F_N;
            }
            using (var FL = new FileStream(file_name, FileMode.OpenOrCreate))
            {
                Serialise_oll sl = (Serialise_oll)BF.Deserialize(FL);
                return sl;
            }
        }
    }
}
