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
            //за броене на елементите на БД
            int obl_c=0, obsh_c=0, km_c=0, ek_c=0; 
            //Свързване с базата данни
            MySqlConnection con = new MySqlConnection("Data Source=localhost;Database=ekatte;User ID=root;Password=Tangratu");
            con.Open();
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\ekatte_obl.csv", Encoding.GetEncoding(1251)))
            { 
                string check = "SELECT COUNT(*) FROM oblast";
                //Проверка за вече съществуващи данни
                MySqlCommand ch = new MySqlCommand(check, con);
                //Броят записи в БД
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                //Ако броят записи е еднакъв във файла и БД значи всичко е записано и трябва да записва отново
                if (lines != getrealcount("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\ekatte_obl.csv"))
                {

                    if(lines > 0)
                    {
                        //Ако броят записи във БД е различен от този във файл, но не е нулев значи има някаква грешка
                        //Всички записи в БД се изтриват
                        new MySqlCommand("DELETE FROM oblast", con).ExecuteNonQuery();
                    }

                    string tag, name, inp;
                    s.ReadLine(); //Прескачане на header частта от файла
                    while (!s.EndOfStream)
                    {
                        //Чете по редове, разделя на отделни стойности
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];
                        name = temp[1];
                        //Записва стойности в БД
                        string ins_qry =
                            "INSERT INTO oblast (tag,name) VALUES (@tag,@name)";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@tag", tag);
                        com.Parameters.AddWithValue("@name", name);
                        com.ExecuteNonQuery();
                        obl_c++;
                    }
                }
                else { obl_c = lines; }
            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obst.csv", Encoding.GetEncoding(1251)))
            {
                //Същата проверка като горе
                string check = "SELECT COUNT(*) FROM obshtina";
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                if (lines != getrealcount("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obst.csv"))
                {
                    if(lines > 0)
                    {
                        new MySqlCommand("DELETE FROM obshtina", con).ExecuteNonQuery();
                    }


                    string tag, cat, inp,id;
                    s.ReadLine();
                    while (!s.EndOfStream)
                    {

                        id = ""; //Тук се записва трибуквеното съкращение извелечено от идентификационния номер на общината
                        //Чрез това съкращение се намира съответната област и се взима нейния id за foreign key полето
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];
                        cat = temp[1];
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
                            "INSERT INTO obshtina (cat,idobl,tag) VALUES (@cat,@idobl,@tag)";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@cat", cat);
                        com.Parameters.AddWithValue("@idobl", idied.ExecuteScalar().ToString());
                        com.Parameters.AddWithValue("@tag", tag);
                        com.ExecuteNonQuery();
                        obsh_c++;
                    }
                }
                else { obsh_c = lines; }
            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_kmet.csv", Encoding.GetEncoding(1251)))
            {
                string check = "SELECT COUNT(*) FROM kmetstvo";
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                if (lines != getrealcount("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_kmet.csv"))
                {
                    if (lines > 0)
                    {
                        new MySqlCommand("DELETE FROM kmetstvo", con).ExecuteNonQuery();
                    }


                    string tag, inp, id;
                    s.ReadLine();
                    
                    while (!s.EndOfStream)
                    {
                        id = "";
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];                        
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
                            "INSERT INTO kmetstvo (idobsh,tag) VALUES (@idobsh,@tag)";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);                        
                        com.Parameters.AddWithValue("@idobsh", idied.ExecuteScalar().ToString());
                        com.Parameters.AddWithValue("@tag", tag);
                        com.ExecuteNonQuery();
                        km_c++;
                    }
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
                if (lines == 0)
                {
                    


                    string name, ekatte, cat,inp, tag;
                    //Тук първите два реда не съдържат информация
                    s.ReadLine();
                    s.ReadLine();
                    while (!s.EndOfStream)
                    {
                        
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        ekatte = temp[0];
                        name = temp[1];
                        tag = temp[2];
                        cat = temp[3];
                        //някой записи в файла имат идентификационен номер на кметството завършващ на 00
                        //тези записи са невалидни тъй като няма кметства с такива номера
                        if (!tag.Contains("00"))
                        {


                            string getid = "SELECT id FROM kmetstvo WHERE tag = '" + tag + "'";
                            MySqlCommand idied = new MySqlCommand(getid, con);
                            string ins_qry =
                                "INSERT INTO ekatte (name,ekatte,cat,idkm) VALUES (@name,@ekatte,@cat,@idkm)";
                            MySqlCommand com = new MySqlCommand(ins_qry, con);
                            com.Parameters.AddWithValue("@name", name);
                            com.Parameters.AddWithValue("@ekatte", ekatte);
                            com.Parameters.AddWithValue("@cat", cat);
                            com.Parameters.AddWithValue("@idkm", idied.ExecuteScalar().ToString());
                            com.ExecuteNonQuery();
                            ek_c++;
                        }
                    }
                }
                else { ek_c = lines; }
            }
            con.Close();
            //Скрива се бутона за вкарване на данни, показва се текстово поле и бутон за търсене
            Tb_search.Visible = true;
            Bt_search.Visible = true;
            Bt_import.Visible = false;            
            Lb_obl.Text = obl_c.ToString();
            Lb_obsh.Text = obsh_c.ToString();
            Lb_km.Text = km_c.ToString();
            Lb_ek.Text = ek_c.ToString();
        }

        protected void Bt_search_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Data Source=localhost;Database=ekatte;User ID=root;Password=Tangratu");
            con.Open();
            //Извличат се данни за всички градове с имена започващи с това записано в полето
            string search = "SELECT * FROM ekatte WHERE name LIKE '" + Tb_search.Text + "%" + "'";
            MySqlDataAdapter mda = new MySqlDataAdapter(search, con);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            GV_ek.DataSource = dt;
            GV_ek.DataBind();
            con.Close();

        }
    }
}