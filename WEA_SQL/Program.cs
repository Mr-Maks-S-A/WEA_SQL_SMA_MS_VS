using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEA_SQL
{
    static class GloBal 
    {
        public static int loat_line = 0;
        //основной класс с данными для настроек приложения и так же пользователя
        public static Serialise_oll Conf_oll =new Serialise_oll() ;
        //класс для сохронения пароля и настроек пользователя
        public static load_conf load_Conf =new load_conf() ;
        //для работы с базой данных
        public static SQL_DB_WOC SQL ;

        //формы
        public static int form;
        public static ADMIN_OPTIONS admin ;//0
        public static Form1 Logining; //1
        public static Main_worc main_Worc ;//2


        /*шрифт*/
        public static PrivateFontCollection myFont = new PrivateFontCollection();
        //закругления
        public static GraphicsPath PoundedRectangel(Rectangle rect ,float RoundSize)
        {
                GraphicsPath Gpath = new GraphicsPath();
                Gpath.AddArc(rect.X, rect.Y, RoundSize, RoundSize, 180, 90);
                Gpath.AddArc(rect.X + rect.Width - RoundSize, rect.Y, RoundSize, RoundSize, 270, 90);
                Gpath.AddArc(rect.X + rect.Width - RoundSize, rect.Y + rect.Height - RoundSize, RoundSize, RoundSize, 0, 90);
                Gpath.AddArc(rect.X, rect.Y + rect.Height - RoundSize, RoundSize, RoundSize, 90, 90);
                Gpath.CloseFigure();
                return Gpath;
        }
    }
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //шрифт
            using (MemoryStream fontStream = new MemoryStream(Properties.Resources.Leto_Text_Sans_Defect))
            {
                var data = Marshal.AllocHGlobal((int)fontStream.Length);
                byte[] Font_data = new byte[fontStream.Length];
                fontStream.Read(Font_data, 0, Font_data.Length);
                Marshal.Copy(Font_data, 0, data, Font_data.Length);
                GloBal.myFont.AddMemoryFont(data, (int)fontStream.Length);
                Marshal.FreeCoTaskMem(data);
            }
            //шрифт

            //Загрузка обьекта с данными по пользователю и настройки
            //GloBal.load_Conf.seri_s_oll(GloBal.Conf_oll);

            GloBal.Conf_oll= GloBal.load_Conf.deser_s_oll();

            //установка строки подключения к базе данных из сохронённого файла
            if (GloBal.Conf_oll.adm.Sql_db_serv != "")
            {
                GloBal.SQL = new SQL_DB_WOC(GloBal.Conf_oll.adm.Sql_db_serv);
            }
            else 
            {
                GloBal.SQL = new SQL_DB_WOC();
            }
            //установка строки подключения к базе данных из сохронённого файла

            GloBal.Logining = new Form1();
            Application.Run(GloBal.Logining);
        }
    }

}
