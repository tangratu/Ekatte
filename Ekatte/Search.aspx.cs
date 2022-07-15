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
            MySqlConnection con = new MySqlConnection("Data Source=localhost;Database=ekatte;User ID=root;Password=Tangratu");
            con.Open();
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\ekatte_obl.csv", Encoding.GetEncoding(1251)))
            {
                string check = "SELECT COUNT(*) FROM oblast";
                
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                if (lines != getrealcount("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\ekatte_obl.csv"))
                {
                    if(lines > 0)
                    {
                        new MySqlCommand("DELETE FROM oblast", con).ExecuteNonQuery();
                    }

                    string tag, name, inp;
                    s.ReadLine();
                    while (!s.EndOfStream)
                    {
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];
                        name = temp[1];
                        string ins_qry =
                            "INSERT INTO oblast (tag,name) VALUES (@tag,@name)";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@tag", tag);
                        com.Parameters.AddWithValue("@name", name);
                        com.ExecuteNonQuery();
                    }
                }
            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_obst.csv", Encoding.GetEncoding(1251)))
            {
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
                        id = "";
                        inp = s.ReadLine();
                        var temp = inp.Split(';');
                        tag = temp[0];
                        cat = temp[1];
                        foreach (char item in tag)
                        {
                            if (Char.IsDigit(item))
                            {
                                break;
                            }
                            id += item;
                        }
                        string getid = "SELECT id FROM oblast WHERE tag = '" + id + "'";
                        MySqlCommand idied = new MySqlCommand(getid, con);
                        string ins_qry =
                            "INSERT INTO obshtina (cat,idobl,tag) VALUES (@cat,@idobl,@tag)";
                        MySqlCommand com = new MySqlCommand(ins_qry, con);
                        com.Parameters.AddWithValue("@cat", cat);
                        com.Parameters.AddWithValue("@idobl", idied.ExecuteScalar().ToString());
                        com.Parameters.AddWithValue("@tag", tag);
                        com.ExecuteNonQuery();
                    }
                }
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
                    }
                }
            }
            using (StreamReader s = new StreamReader("C:\\Users\\Anton\\source\\repos\\Ekatte\\Ekatte\\Ek_atte.csv", Encoding.GetEncoding(1251)))
            {
                string check = "SELECT COUNT(*) FROM ekatte";
                MySqlCommand ch = new MySqlCommand(check, con);
                int lines = int.Parse(ch.ExecuteScalar().ToString());
                if (lines == 0)
                {
                    


                    string name, ekatte, cat,inp, tag;
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
                        }
                    }
                }
            }
            con.Close();
            Tb_search.Visible = true;
            Bt_search.Visible = true;
            Bt_import.Visible = false;
        }

        protected void Bt_search_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Data Source=localhost;Database=ekatte;User ID=root;Password=Tangratu");
            con.Open();
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