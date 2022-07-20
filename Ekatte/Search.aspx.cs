using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text;
using System.Data;

namespace Ekatte
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //Метод за получаване на броя записи в CSV файл
        //Използва се за валидиране на броя записи в базата данни спрямо броя записи във файла
        int getrealcount(string file)
        {
            int count = 0;
            using (StreamReader s = new StreamReader(file))
            {
                while (!s.EndOfStream)
                {
                    s.ReadLine();
                    count++;
                }
            }
            return count-1;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            bool updated = false;
            //за броене на елементите на БД
            int obl_c = 0, obsh_c = 0, km_c = 0, ek_c = 0;
            //Свързване с базата данни
            MySqlConnection con = new MySqlConnection("Data Source=localhost;Database=ekatte;User ID=root;Password=Tangratu");
            con.Open();
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obl.csv", Encoding.GetEncoding(1251)))
            {
                string check = "SELECT COUNT(*) FROM oblast";
                //Проверка за вече съществуващи данни
                MySqlCommand ch = new MySqlCommand(check, con);
                //Броят записи в БД
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                //Ако броят записи е еднакъв във файла и БД значи всичко е записано и трябва да записва отново
                string update = System.IO.File.GetLastWriteTime("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obl.csv").ToString("yyyy-MM-dd HH:mm:ss");


                if (lines == 0 || int.Parse(new MySqlCommand("SELECT COUNT(*) FROM last_change WHERE t_name = 'oblast' AND change_time < '" + update + "'", con).ExecuteScalar().ToString()) > 0)
                {
                    updated = true;
                    string tag, name, inp;
                    s.ReadLine(); //Прескачане на header частта от файла
                    while (!s.EndOfStream)
                    {
                        //Чете по редове, разделя на отделни стойности
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];
                        name = temp[2];
                        //Записва стойности в БД
                        string ins_qry =
                            "INSERT INTO oblast (tag,name) VALUES (@tag,@name) ON DUPLICATE KEY UPDATE name = @name";

                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@tag", tag);
                        com.Parameters.AddWithValue("@name", name);
                        com.ExecuteNonQuery();
                        obl_c++;
                    }
                    new MySqlCommand("UPDATE last_change SET change_time ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE t_name = 'oblast'", con).ExecuteNonQuery();
                }
                else
                {
                    obl_c = lines;
                }

            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obst.csv", Encoding.GetEncoding(1251)))
            {
                //Същата проверка като горе
                string check = "SELECT COUNT(*) FROM obshtina";
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                string update = System.IO.File.GetLastWriteTime("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obst.csv").ToString("yyyy-MM-dd HH:mm:ss");
                if (lines == 0 || int.Parse(new MySqlCommand("SELECT COUNT(*) FROM last_change WHERE t_name = 'obshtina' AND change_time < '" + update + "'", con).ExecuteScalar().ToString()) > 0)
                {


                    updated = true;
                    string tag, cat, inp, id, name;
                    s.ReadLine();
                    while (!s.EndOfStream)
                    {

                        id = ""; //Тук се записва трибуквеното съкращение извелечено от идентификационния номер на общината
                        //Чрез това съкращение се намира съответната област и се взима нейния id за foreign key полето
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        name = temp[2];
                        tag = temp[0];
                        cat = temp[3];
                        foreach (char item in tag)
                        {
                            //Отделя се буквената част от индентификационния номер
                            if (Char.IsDigit(item))
                            {
                                break;
                            }
                            id += item;
                        }
                        //Този query взима id на областта съответстваща на общината
                        string getid = "SELECT id FROM oblast WHERE tag = '" + id + "'";
                        MySqlCommand idied = new MySqlCommand(getid, con);
                        string ins_qry =
                            "INSERT INTO obshtina (cat,idobl,tag,name) VALUES (@cat,@idobl,@tag,@name) ON DUPLICATE KEY UPDATE cat = @cat, idobl=@idobl, name = @name";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@cat", cat);
                        com.Parameters.AddWithValue("@idobl", idied.ExecuteScalar().ToString());
                        com.Parameters.AddWithValue("@tag", tag);
                        com.Parameters.AddWithValue("@name", name);
                        com.ExecuteNonQuery();
                        obsh_c++;
                    }
                    new MySqlCommand("UPDATE last_change SET change_time ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE t_name = 'obshtina'", con).ExecuteNonQuery();

                }
                else { obsh_c = lines; }
            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_kmet.csv", Encoding.GetEncoding(1251)))
            {
                string check = "SELECT COUNT(*) FROM kmetstvo";
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                string update = System.IO.File.GetLastWriteTime("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_kmet.csv").ToString("yyyy-MM-dd HH:mm:ss");
                if (lines == 0 || int.Parse(new MySqlCommand("SELECT COUNT(*) FROM last_change WHERE t_name = 'kmetstvo' AND change_time < '" + update + "'", con).ExecuteScalar().ToString()) > 0)
                {


                    updated = true;
                    string tag, inp, id, name;
                    s.ReadLine();

                    while (!s.EndOfStream)
                    {
                        id = "";
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];
                        name = temp[1];
                        foreach (char item in tag)
                        {
                            //Тук се чете пълния индентификационен номер на общината, който се намира преди тирето
                            //С него се достига id на общината, в която е кметството
                            if (item == '-')
                            {
                                break;
                            }
                            id += item;
                        }
                        string getid = "SELECT id FROM obshtina WHERE tag = '" + id + "'";
                        MySqlCommand idied = new MySqlCommand(getid, con);
                        string ins_qry =
                            "INSERT INTO kmetstvo (idobsh,tag,name) VALUES (@idobsh,@tag,@name) ON DUPLICATE KEY UPDATE idobsh=@idobsh,name=@name";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@idobsh", idied.ExecuteScalar().ToString());
                        com.Parameters.AddWithValue("@tag", tag);
                        com.Parameters.AddWithValue("@name", name);
                        com.ExecuteNonQuery();
                        km_c++;
                    }
                    new MySqlCommand("UPDATE last_change SET change_time ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE t_name = 'kmetstvo'", con).ExecuteNonQuery();
                }
                else { km_c = lines; }
            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_atte.csv", Encoding.GetEncoding(1251)))
            {
                //Поради неточности във файла Ek_atte тук проверка като горните не е възможна
                //Прави се проста проверка дали БД е празно
                string check = "SELECT COUNT(*) FROM ekatte";
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                string update = System.IO.File.GetLastWriteTime("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_atte.csv").ToString("yyyy-MM-dd HH:mm:ss");
                if (lines == 0 || int.Parse(new MySqlCommand("SELECT COUNT(*) FROM last_change WHERE t_name = 'ekatte' AND change_time < '" + update + "'", con).ExecuteScalar().ToString()) > 0)
                {


                    updated = true;
                    string name, ekatte, cat, inp, tag, id = "";
                    //Тук първите два реда не съдържат информация
                    s.ReadLine();
                    s.ReadLine();
                    while (!s.EndOfStream)
                    {

                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        ekatte = temp[0];
                        name = temp[2];
                        tag = temp[5];
                        cat = temp[7];
                        //някой записи в файла имат идентификационен номер на кметството завършващ на 00
                        //тези записи са невалидни тъй като няма кметства с такива номера
                        if (!tag.Contains("00"))
                        {


                            string getid = "SELECT id FROM kmetstvo WHERE tag = '" + tag + "'";
                            MySqlCommand idied = new MySqlCommand(getid, con);
                            string ins_qry =
                                "INSERT INTO ekatte (name,ekatte,cat,idkm) VALUES (@name,@ekatte,@cat,@idkm) ON DUPLICATE KEY UPDATE name=@name,cat=@cat,idkm=@idkm";
                            MySqlCommand com = new MySqlCommand(ins_qry, con);
                            com.Parameters.AddWithValue("@name", name);
                            com.Parameters.AddWithValue("@ekatte", ekatte);
                            com.Parameters.AddWithValue("@cat", cat);
                            com.Parameters.AddWithValue("@idkm", idied.ExecuteScalar().ToString());
                            com.ExecuteNonQuery();
                            ek_c++;
                        }
                        else
                        {
                            id = "";
                            foreach (char item in tag)
                            {

                                if (item == '-')
                                {
                                    break;
                                }
                                id += item;
                            }
                            string getid = "SELECT id FROM obshtina WHERE tag = '" + id + "'";
                            MySqlCommand idied = new MySqlCommand(getid, con);
                            string ins_qry =
                                "INSERT INTO undef_ektta (name,ekatte,cat,idobsh) VALUES (@name,@ekatte,@cat,@idobsh) ON DUPLICATE KEY UPDATE name=@name,cat=@cat,idobsh=@idobsh";
                            MySqlCommand com = new MySqlCommand(ins_qry, con);
                            com.Parameters.AddWithValue("@name", name);
                            com.Parameters.AddWithValue("@ekatte", ekatte);
                            com.Parameters.AddWithValue("@cat", cat);
                            com.Parameters.AddWithValue("@idobsh", idied.ExecuteScalar().ToString());
                            com.ExecuteNonQuery();
                            ek_c++;
                        }

                    }
                    new MySqlCommand("UPDATE last_change SET change_time ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE t_name = 'ekatte'", con).ExecuteNonQuery();
                    new MySqlCommand("UPDATE last_change SET change_time ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE t_name = 'undef_ekatte'", con).ExecuteNonQuery();
                }
                else { ek_c = lines + int.Parse(new MySqlCommand("SELECT COUNT(ekatte) FROm undef_ektta",con).ExecuteScalar().ToString()); }
            }
            
            //Скрива се бутона за вкарване на данни, показва се текстово поле и бутон за търсене
            Tb_search.Visible = true;
            Bt_search.Visible = true;
            Bt_import.Visible = false;
            Lb_obl.Text += obl_c.ToString();
            Lb_obsh.Text += obsh_c.ToString();
            Lb_km.Text += km_c.ToString();
            Lb_ek.Text += ek_c.ToString();
            if (int.Parse(new MySqlCommand("SELECT COUNT(name) FROM full_ekatte", con).ExecuteScalar().ToString()) == 0 || updated)
            {
                new MySqlCommand("INSERT INTO full_ekatte(name,ekatte,kmetstvo,obshtina,oblast)  SELECT ek.name,ek.ekatte,km.name,obs.name,obl.name FROM ekatte AS ek JOIN kmetstvo AS km ON idkm = km.id JOIN obshtina AS obs ON km.idobsh = obs.id JOIN oblast AS obl ON obs.idobl = obl.id ON DUPLICATE KEY UPDATE name = ek.name, kmetstvo = km.name, obshtina = obs.name, oblast = obl.name", con).ExecuteNonQuery();
                new MySqlCommand("INSERT INTO full_ekatte(name,ekatte,obshtina,oblast)  SELECT ek.name,ek.ekatte,obs.name,obl.name FROM undef_ektta AS ek JOIN obshtina AS obs ON ek.idobsh = obs.id JOIN oblast AS obl ON obs.idobl = obl.id ON DUPLICATE KEY UPDATE name = ek.name, obshtina = obs.name, oblast = obl.name", con).ExecuteNonQuery();
            }
            con.Close();
        }

        protected void Bt_search_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Data Source=localhost;Database=ekatte;User ID=root;Password=Tangratu");
            con.Open();
            //Извличат се данни за всички градове с имена започващи с това записано в полето
            if (Tb_search.Text != "")
            {
               string search = "SELECT name,ekatte,kmetstvo,obshtina,oblast FROM full_ekatte  WHERE name LIKE '" + Tb_search.Text + "%" + "' ORDER BY name";
                MySqlDataAdapter mda = new MySqlDataAdapter(search, con);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                GV_ek.DataSource = dt;
                GV_ek.DataBind();
            }
            else
            {
                Response.Write("Empty field");
            }
            
            con.Close();

        }
    }
}