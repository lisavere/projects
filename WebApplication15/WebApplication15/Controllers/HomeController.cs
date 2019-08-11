using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using MySql.Data.MySqlClient;
using WebApplication15.Models;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
namespace WebApplication15.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Application(Application app)
        {
            //   <Authorize(Roles:="Agent")>;
            //Генерация документа в excel !!!!!!!
            //<AuthorizeAttribute (Sotr )>
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            Sotr sotr;
            // int IdOtdely = 1;
            var OtdelId = (string)Session["OtdelyId"];
            //ViewBag = OtdelId;
            List<Application> apps = new List<Application>();
            //позже прописать в соответствии в логином паролем , модели сотрудник присвоить
            //коенкретный элемент сотрудника должен содержать конкретный ID
            //  ConnectionString();
            string query = "Select IdApp,IdOtdely,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status, Priority_Otdel,Date_registration,Current_status,Notes from apps where IdOtdely= '" + OtdelId + "' Order By Priority_Otdel";//where IdOtdely='" + sotr.IdOtdely + "'";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read()) { apps.Add(new Application { idApp = sdr["IdApp"].ToString(), Date_create = sdr["Date_Create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString(), Aim_create_using = sdr["Aim_create_using"].ToString(), Sotr_using = sdr["Sotr_using"].ToString(), Time_using = sdr["Time_using"].ToString(), Konfident = sdr["Konfident"].ToString(), Requirem = sdr["Requirem"].ToString(), Status = sdr["Status"].ToString(), Priority_Otdel = sdr["Priority_Otdel"].ToString(), Current_status = sdr["Current_status"].ToString(), Notes = sdr["Notes"].ToString() }); }
            con.Close();
            //  ViewBag.apps = apps;
            return View(apps);

        }

        [HttpGet]
        public ActionResult CreateApp()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "На разработку ПО", Value = "На разработку ПО" });
            list.Add(new SelectListItem { Text = "На доработку(модификацию) программного обеспечения", Value = "На доработку(модификацию) программного обеспечения" });
            //  list.Add("На разработку");
            // list.Add("на доработку(модификацию) программного обеспечения");
            SelectList List = new SelectList(list, "Value", "Text");
            ViewBag.List = List;

            /*    string[] Aim = new string[] { "На разработку ПО", "На доработку(модификацию) программного обеспечения" };
                List<String> aim = Aim.ToList();
                SelectList NewAim = new SelectList(aim,"value","");
                ViewBag.List = NewAim;*/


            List<String> WorkingSpace = new List<String>();
            WorkingSpace.Add("Отделения");
            WorkingSpace.Add("Управления");
            WorkingSpace.Add("Другое");
            SelectList Workingspace = new SelectList(WorkingSpace);
            ViewBag.Workingspace = Workingspace;

            List<String> Time_using = new List<String>();
            Time_using.Add("Ежедневнго");
            Time_using.Add("Ежемесячно");
            Time_using.Add("Другое");
            SelectList time = new SelectList(Time_using);
            ViewBag.time = time;

            //  string[] priority = new string[] { "1", "2", "3","4","5","6","7","8","9","10","11","12" };
            //   List<String> priority_otd = priority.ToList();
            // SelectList Priority = new SelectList(priority_otd);
            // ViewBag.Priority = Priority;



            List<String> v = new List<String>();
            v.Add("Да");
            v.Add("Нет");
            SelectList V = new SelectList(v);
            ViewBag.V = V;
            return View();
        }
        [HttpPost]
        public ActionResult CreateApp(Application app, string konf_konf, string konf_pers, string konf_bd, string Aim, string time_using, string sotr_using)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            // app.idSotr = 1;//Будет добавлено после регистрации
            //app.IdOtdely = 1; //Будет добавлено после регистрации алгоритм
            // List<Application> apps = new List<Application>();
            var OtdelId = (string)Session["OtdelyId"];
            var SotrId = (string)Session["idSotr"];
            //  Session["idSotr"] = SotrId;
            DateTime currentdate = DateTime.Today;
            app.Date_create = currentdate.ToString();
            // int status=0; // написать подсказки в интерактиве
            app.Status = "На очереди";
            Sotr sotr = new Sotr();
            //   if (app.Aim == ""|app.Aim_create_using=="") { return View("Error"); }
            // else{
            //     int idSotr = 1; //добавляется в соответствии с логином
            //  int idOtdely = 1;//добавляется в соответствии с логином
            //Добавить запрет на пустую заявку!!!!
            //   if (app.sotr_using=="Другое") {app.sotr_using=app.sotr_using_dr}
            //if (app.time_using="Другое") {app.time_using=app.time_using_dr}
            //   app.Aim = Aim;
            if (konf_konf == "true") { konf_konf = "Обработка конфиденциальной информации(Да) "; } else { konf_konf = ""; }
            if (konf_pers == "true") { konf_pers = "Обработка персональных данных(Да)"; } else { konf_pers = ""; }
            if (konf_bd == "true") { konf_bd = "Взаимодействие с базами типовых проектных решений(Нет)"; } else { konf_bd = ""; }
            app.Konfident = konf_konf + konf_pers + konf_bd;
            if (app.Sotr_using == "Другое") { app.Sotr_using = app.sotr_using_dr; }
            if (app.Time_using == "Другое") { app.Time_using = app.sotr_using_dr; }
            // app.Sotr_using = sotr_using;
            //  app.Time_using = time_using;
            int lastId;

            if (ModelState.IsValid)
            {
                //app.Priority_Otdel = "1";
                // app.Aim = "На доработку";
                string query = "INSERT INTO apps(Date_create, Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status, Priority_Otdel, idSotr,IdOtdely,Date_analise_beg,Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation) Values (Current_Date(),'" + app.Aim + "','" + app.Naimen_PO + "','" + app.Aim_create_using + "','" + app.Sotr_using + "','" + app.Time_using + "','" + app.Konfident + "','" + app.Requirem + "','" + app.Status + "','" + app.Priority_Otdel + "','" + SotrId + "','" + OtdelId + "','0000-00-00','0000-00-00','0000-00-00','0000-00-00','0000-00-00')";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Connection = con;
                // int lastId;
                string query2 = "SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(query2);
                command.Connection = con;
                con.Open();
                //  cmd.ExecuteNonQuery() ;   
                MySqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                sdr.Close();
                //  if (sdr.Read())
                //   {
                //  while (sdr.Read()) { apps.Add(new Application { Date_create = sdr["Date_create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString() }); }

                /*sql = "SELECT LAST_INSERT_ID();";
                 command = new MySqlCommand(sql, connect);
                 lastId = int.Parse(command.ExecuteScalar().ToString());*/
                MySqlDataReader read = command.ExecuteReader();
                if (read.Read())
                {
                    lastId = read.GetInt32(0);
                    // lastId.ToString();
                    Session["LastId"] = lastId.ToString();
                    Response.Write("<script>alert('Ваша заявка под номером! " + lastId.ToString() + " успешно добавлена!');</script>");
                }
                //while (read.Read()) { lastId = read[""].ToString(); }

                //    lastId = int.Parse(command.ExecuteScalar().ToString());
                // lastId.ToString();
                //   TempData["lastId"] = lastId;

                con.Close();

                // возвратить окно "Добавлено" или ошибка
                //    Response.Write("<script>alert('Succeess!"+lastId.ToString()+"');</script>");
                return View();
                //  return View(lastId.ToString());
            }
            else {
                return View();

            }
            // }
            //  else { return View("Error"); } 
        }
        [HttpGet]
        public ActionResult PriorityOtdel(string id)
        {//получение из дроп дауна

            return View();
        }
        [HttpPost]
        public ActionResult PriorityOtdel(Application app, string id)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Application> apps = new List<Application>();
            var SotrId = (string)Session["idSotr"];
            //var FIO = (string)Session["FIO"];
            string query = "update apps set Priority_Otdel='" + app.Priority_Otdel + "'where idApp='" + id + "'";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Application");
        }
        [HttpGet]
        public ActionResult app(string id)
        {
            // посмотреть нормативно правовые документы
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            var OtdelId = (string)Session["OtdelyId"];
            // var lastId = (string)Session["LastId"];
            //   model.Applicationnewid = lastId;
            //  model.OtdelId = OtdelId;
            //  doc.IdApp = lastId.ToString();
            //   doc.IdOtdely = OtdelId.ToString();
            //string query = "select* from apps where idApp='"+lastId+"'";
            string query1 = "update apps set Status='Отменена отделом', Priority_Otdel=NULL where idApp='" + id + "'";
            //string query2 = "select* from incoming_info_in where Applicationnewid='" + lastId + "'";
            //string query3 = "select* from incoming_info_out where Applicationid='" + lastId + "'";
            //   List<Application> list = new List<Models.Application>();
          //  List<documentation> list1 = new List<Models.documentation>();
            //List<incoming_info_in>list2= new List<Models.incoming_info_in>();
            //  List<incoming_info_out>list3= new List<Models.incoming_info_out>();*/

            /*  MySqlCommand COM = new MySqlCommand(query);
              COM.Connection = con;

              MySqlDataReader dr = COM.ExecuteReader();
              while (dr.Read()) { list.Add(new Application { Aim = dr["Aim"].ToString(), Naimen_PO = dr["Naimen_po"].ToString(), Aim_create_using = dr["Aim_create_using"].ToString(), Sotr_using = dr["Sotr_using"].ToString(), Time_using = dr["Time_using"].ToString(), Konfident = dr["Konfident"].ToString(), Requirem = dr["Requirem"].ToString() }); }
              dr.Close();
             */

            MySqlCommand COM1 = new MySqlCommand(query1);
            COM1.Connection = con;
            con.Open();
            //  MySqlDataReader dr1 = COM1.ExecuteReader();
            // dr1.Read();
            COM1.ExecuteNonQuery();
            // while (dr1.Read()) { list1.Add(new documentation { Naimen = dr1["Naimen"].ToString(), Adress = dr1["Adress"].ToString() }); }
            //dr1.Close();
            //ViewBag.document = list1;
            /*
             MySqlCommand COM2 = new MySqlCommand(query2);
             COM2.Connection = con;

             MySqlDataReader dr2 = COM2.ExecuteReader();
             while (dr2.Read()) { list2.Add(new incoming_info_in {Naimen=dr2["Naimen"].ToString(), Vid_predst=dr2["Vid_predst"].ToString() }); }
             dr2.Close();
            ViewBag.info_in = list2;


            MySqlCommand COM3 = new MySqlCommand(query3);
            COM2.Connection = con;

            MySqlDataReader dr3 = COM3.ExecuteReader();
            while (dr3.Read()) { list3.Add(new incoming_info_out { Naimen_ist = dr2["Naimen_ist"].ToString(), Vid_predst = dr2["Vid_predst"].ToString(),Naimen_doc=dr3["Naimen_doc"].ToString() }); }
            dr3.Close();
            ViewBag.info_out = list3;
            */

            return RedirectToAction("Application");
        }

        [HttpGet]
        public ActionResult Change(Change ch, String Number)
        {
            var OtdelId = (string)Session["OtdelyId"];
            List<Change> list = new List<Change>();
            //  List<Change> list2 = new List<Change>();
            //string otdel = OtdelId.ToString();
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            csb.AllowZeroDateTime = true;
            // csb.ConvertZeroDateTime = true;
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            //string query = "SELECT * FROM change";
            //  string query = "select OtdelyId from change";


            if (Number == "")
            {
                string query = "Select idchange, AppId,OtdelyId,Date_change,Date_analise_beg, Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation,Notes from `database`.`change` where OtdelyId='" + OtdelId + "' limit 20";
                MySqlCommand com = new MySqlCommand(query);
                com.Connection = con;
                con.Open();
                MySqlDataReader dr = com.ExecuteReader();
                while (dr.Read()) { list.Add(new Change { idchange = dr["idchange"].ToString(), AppId = dr["AppId"].ToString(), date_change = dr["Date_change"].ToString(), Date_analise_beg = dr["Date_analise_beg"].ToString(), Date_analise_end = dr["Date_analise_end"].ToString(), Date_test_beg = dr["Date_test_beg"].ToString(), Date_test_end = dr["Date_test_end"].ToString(), Date_expluatation = dr["Date_expluatation"].ToString(), OtdelyID = dr["OtdelyId"].ToString(), Notes = dr["Notes"].ToString() }); }
                con.Close();
                return View(list);
            }
            else
            {
                string query1 = "Select idchange, AppId,OtdelyId,Date_change,Date_analise_beg, Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation,Notes from `database`.`change` where AppId='" + Number + "' and OtdelyId='" + OtdelId + "' order by idchange DESC limit 20 ";
                MySqlCommand com1 = new MySqlCommand(query1);
                com1.Connection = con;
                con.Open();
                MySqlDataReader dr1 = com1.ExecuteReader();
                while (dr1.Read()) { list.Add(new Change { idchange = dr1["idchange"].ToString(), AppId = dr1["AppId"].ToString(), date_change = dr1["Date_change"].ToString(), Date_analise_beg = dr1["Date_analise_beg"].ToString(), Date_analise_end = dr1["Date_analise_end"].ToString(), Date_test_beg = dr1["Date_test_beg"].ToString(), Date_test_end = dr1["Date_test_end"].ToString(), Date_expluatation = dr1["Date_expluatation"].ToString(), OtdelyID = dr1["OtdelyId"].ToString(), Notes = dr1["Notes"].ToString() }); }
                con.Close();

                // ViewBag.list2 = list2;
                return View(list);
            }
        }


        public ActionResult SpisokSotr(Sotr sotr, String otdel)
        { //страница админа
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Sotr> sotrs = new List<Sotr>();
            //   static  List<Application> apps = new List<Application>();

            List<Otdely> otdely = new List<Otdely>();

            //  List<Role> roles = new List<Role>();

            // IQueryable<Sotr> sotrsq = sotrs;

            string query1 = "Select IdOtdely,Naimen_otd from otdely";

            //    string query2 = "Select idRole,Naimen_role from role";


            MySqlCommand cmd1 = new MySqlCommand(query1);
            //  MySqlCommand cmd2 = new MySqlCommand(query2);


            //  cmd.Connection = con;
            cmd1.Connection = con;
            //  cmd2.Connection = con;
            //otdely.Insert(sotr.Otdel.IdOtdely, sotr.Otdel.Naimen_otd);

            con.Open();
            cmd1.ExecuteNonQuery();
            MySqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read()) { otdely.Add(new Otdely { IdOtdely = dr["IdOtdely"].ToString(), Naimen_otd = dr["Naimen_otd"].ToString() }); }
            dr.Close();
            otdely.Insert(0, new Otdely { IdOtdely = "0", Naimen_otd = "Все" });
            SelectList Otdely = new SelectList(otdely, "IdOtdely", "Naimen_otd");
            ViewBag.Otdely = Otdely;
            // string query = "Select idSotr,FIO,Dolzn,Login ,psswd,Naimen_otd,Naimen_role from sotr,otdely,role where sotr.idOtdely=otdely.idOtdely and sotr.idRole=role.idRole";


            if (otdel != "")
            {
                string querynew = "Select idSotr,FIO,Dolzn,Login ,psswd,Naimen_otd,Naimen_role from sotr,otdely,role where sotr.idOtdely='" + otdel + "' and sotr.idRole=role.idRole and sotr.idOtdely=otdely.IdOtdely ";
                MySqlCommand cmdnew = new MySqlCommand(querynew);
                cmdnew.Connection = con;

                MySqlDataReader drnew = cmdnew.ExecuteReader();
                while (drnew.Read()) { sotrs.Add(new Sotr { idSotr = drnew["idSotr"].ToString(), IdOtdely = drnew["Naimen_otd"].ToString(), FIO = drnew["FIO"].ToString(), Dolzn = drnew["Dolzn"].ToString(), Login = drnew["Login"].ToString(), psswd = drnew["psswd"].ToString(), idRole = drnew["Naimen_role"].ToString() }); }
                drnew.Close();

                //   SelectList Otdely = new SelectList(otdely, "IdOtdely", "Naimen_otd");
                return View(sotrs);
            }
            else

            {
                if (otdel == "28")
                {
                    string query = "Select idSotr,FIO,Dolzn,Login ,psswd,Naimen_otd,Naimen_role from sotr,otdely,role where sotr.idOtdely=otdely.idOtdely and sotr.idRole=role.idRole";


                    int? IdOtdely1 = sotr.OtdelID;
                    MySqlCommand cmd = new MySqlCommand(query);

                    MySqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {

                        sotrs.Add(new Sotr { idSotr = sdr["idSotr"].ToString(), IdOtdely = sdr["Naimen_otd"].ToString(), FIO = sdr["FIO"].ToString(), Dolzn = sdr["Dolzn"].ToString(), Login = sdr["Login"].ToString(), psswd = sdr["psswd"].ToString(), idRole = sdr["Naimen_role"].ToString() });

                    }
                    con.Close();
                    return View(sotrs);
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditSuperviserinApp(string id, Application app)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);

            //Edit application =  List<Application>();


            List<Sotr> sotrs = new List<Sotr>();
            List<Application> ap = new List<Application>();
            // Edit a = ap.ToList();
            string query = "select idApp,Date_create,Aim,Naimen_PO,Aim_create_using,Sotr_using,Time_using,konfident,Requirem,Priority_otdel,Naimen_otd,Superviser from apps,otdely where idApp='" + id + "' and apps.IdOtdely=otdely.IdOtdely";
            // from apps,otdely where apps.IdOtdely=otdely.IdOtdely        
            MySqlCommand cmd2 = new MySqlCommand(query);
            cmd2.Connection = con;

            string query1 = "Select idSotr,FIO from sotr where idRole='" + "3" + "'";

            MySqlCommand cmd1 = new MySqlCommand(query1);

            cmd1.Connection = con;

            con.Open();
            MySqlDataReader sdr = cmd2.ExecuteReader();
            while (sdr.Read()) { ap.Add(new Application { IdOtdely = sdr["Naimen_otd"].ToString(), idApp = sdr["IdApp"].ToString(), Date_create = sdr["Date_Create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString(), Aim_create_using = sdr["Aim_create_using"].ToString(), Sotr_using = sdr["Sotr_using"].ToString(), Time_using = sdr["Time_using"].ToString(), Konfident = sdr["Konfident"].ToString(), Requirem = sdr["Requirem"].ToString(), Priority_Otdel = sdr["Priority_Otdel"].ToString(), Superviser = sdr["Superviser"].ToString() }); }

            sdr.Close();

            ViewBag.ap = ap;

            MySqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read()) { sotrs.Add(new Sotr { idSotr = dr["idSotr"].ToString(), FIO = dr["FIO"].ToString() }); }
            dr.Close();


            SelectList sotrss = new SelectList(sotrs, "idSotr", "FIO");
            ViewBag.Sotr = sotrss;

            con.Close();


            return View(ap);
        }

        public ActionResult AppRaspred(Application app)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Application> apps = new List<Application>();
            var i = 0;

            //var OtdelId = (string)Session["OtdelyId"];

            string query = "Select IdApp,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status, Priority_Otdel,Date_registration,Naimen_otd,FIO from apps,otdely,sotr where apps.IdOtdely=otdely.IdOtdely and apps.Superviser=sotr.idSotr and  apps.Status<>'На очереди'   ";//where IdOtdely='" + sotr.IdOtdely + "'";
                                                                                                                                                                                                                                                                                  // sotr,otdely,role where sotr.idOtdely=otdely.idOtdely
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read()) { apps.Add(new Application { IdOtdely = sdr["Naimen_otd"].ToString(), idApp = sdr["IdApp"].ToString(), Date_create = sdr["Date_Create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString(), Aim_create_using = sdr["Aim_create_using"].ToString(), Sotr_using = sdr["Sotr_using"].ToString(), Time_using = sdr["Time_using"].ToString(), Konfident = sdr["Konfident"].ToString(), Requirem = sdr["Requirem"].ToString(), Status = sdr["Status"].ToString(), Superviser = sdr["FIO"].ToString() }); }
            con.Close();
            sdr.Close();
            // List<Application> a = new List<Application>();
            // Какое представление заявки? У начальника??
            List<Application> ap = new List<Application>();
            string query1 = "Select IdApp,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem, Priority_Otdel,Date_registration,Superviser from apps where Status='" + "На очереди" + "'";//where IdOtdely='" + sotr.IdOtdely + "'";
            MySqlCommand cmd1 = new MySqlCommand(query1);
            cmd1.Connection = con;
            con.Open();

            MySqlDataReader sdr1 = cmd1.ExecuteReader();

            while (sdr1.Read()) { i = i + 1; ap.Add(new Application { idApp = sdr1["IdApp"].ToString(), Date_create = sdr1["Date_Create"].ToString(), Aim = sdr1["Aim"].ToString(), Naimen_PO = sdr1["Naimen_PO"].ToString(), Aim_create_using = sdr1["Aim_create_using"].ToString(), Sotr_using = sdr1["Sotr_using"].ToString(), Time_using = sdr1["Time_using"].ToString(), Konfident = sdr1["Konfident"].ToString(), Requirem = sdr1["Requirem"].ToString() }); }
            app.s = i.ToString();
            ViewBag.Application = app.s;
            con.Close();
            // IEnumerable<Application> AIE = apps;
            return View(apps);
            // return View (s);

        }
        [HttpGet]
        public ActionResult EditNew(string id)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Sotr> list = new List<Sotr>();
            string query = "select FIO,idSotr from sotr where idRole in (3,4)";//where IdOtdely='" + sotr.IdOtdely + "'";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { list.Add(new Sotr { idSotr = reader["idSotr"].ToString(), FIO = reader["FIO"].ToString() }); }
            SelectList list1 = new SelectList(list, "idSotr", "FIO");
            ViewBag.list2 = list1;
            con.Close();
            return View();

        }
        [HttpPost]
        public ActionResult EditNew(string id, Application app, string idSotr)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            // app.Superviser = idSotr;
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            string query = "Update apps set Superviser='" + app.Superviser + "', Status='зарегистрирована',Date_registration=Current_Date() where idApp= '" + id + "'";//where IdOtdely='" + sotr.IdOtdely + "'";

            MySqlCommand cmd = new MySqlCommand(query);
            //MySqlDataReader reader = cmd.ExecuteReader();

            cmd.Connection = con;
            con.Open();
            //   if (reader.Read())
            //  {
            cmd.ExecuteNonQuery();

            string query2 = "call new_procedure()";

            MySqlCommand command = new MySqlCommand(query2);
            command.Connection = con;
            command.ExecuteNonQuery();

            Response.Write("<script>alert('Курирующий от Исполнителя успешно добавлен!')</script>;");

            con.Close();
            //"032000-0802033@032" <032000-0802033@032>
          //  Contact("032000-0802033@032", "Вам новая заявка", "Зайдите в электронный журнал!");
            return RedirectToAction("AppRaspred");

        }


        [HttpGet]
        public ActionResult app1(string id)
        {
            // посмотреть нормативно правовые документы
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            var OtdelId = (string)Session["OtdelyId"];
            // var lastId = (string)Session["LastId"];
            //   model.Applicationnewid = lastId;
            //  model.OtdelId = OtdelId;
            //  doc.IdApp = lastId.ToString();
            //   doc.IdOtdely = OtdelId.ToString();
            //string query = "select* from apps where idApp='"+lastId+"'";
            string query1 = "update apps set Status='Отменена начальником отдела' where idApp='" + id + "'";
            //string query2 = "select* from incoming_info_in where Applicationnewid='" + lastId + "'";
            //string query3 = "select* from incoming_info_out where Applicationid='" + lastId + "'";
            //   List<Application> list = new List<Models.Application>();
            // List<documentation> list1 = new List<Models.documentation>();
            //List<incoming_info_in>list2= new List<Models.incoming_info_in>();
            //  List<incoming_info_out>list3= new List<Models.incoming_info_out>();*/

            /*  MySqlCommand COM = new MySqlCommand(query);
              COM.Connection = con;

              MySqlDataReader dr = COM.ExecuteReader();
              while (dr.Read()) { list.Add(new Application { Aim = dr["Aim"].ToString(), Naimen_PO = dr["Naimen_po"].ToString(), Aim_create_using = dr["Aim_create_using"].ToString(), Sotr_using = dr["Sotr_using"].ToString(), Time_using = dr["Time_using"].ToString(), Konfident = dr["Konfident"].ToString(), Requirem = dr["Requirem"].ToString() }); }
              dr.Close();
             */

            MySqlCommand COM1 = new MySqlCommand(query1);
            COM1.Connection = con;
            con.Open();
            //  MySqlDataReader dr1 = COM1.ExecuteReader();
            // dr1.Read();
            COM1.ExecuteNonQuery();
            // while (dr1.Read()) { list1.Add(new documentation { Naimen = dr1["Naimen"].ToString(), Adress = dr1["Adress"].ToString() }); }
            //dr1.Close();
            //ViewBag.document = list1;
            /*
             MySqlCommand COM2 = new MySqlCommand(query2);
             COM2.Connection = con;

             MySqlDataReader dr2 = COM2.ExecuteReader();
             while (dr2.Read()) { list2.Add(new incoming_info_in {Naimen=dr2["Naimen"].ToString(), Vid_predst=dr2["Vid_predst"].ToString() }); }
             dr2.Close();
            ViewBag.info_in = list2;


            MySqlCommand COM3 = new MySqlCommand(query3);
            COM2.Connection = con;

            MySqlDataReader dr3 = COM3.ExecuteReader();
            while (dr3.Read()) { list3.Add(new incoming_info_out { Naimen_ist = dr2["Naimen_ist"].ToString(), Vid_predst = dr2["Vid_predst"].ToString(),Naimen_doc=dr3["Naimen_doc"].ToString() }); }
            dr3.Close();
            ViewBag.info_out = list3;
            */

            return RedirectToAction("AppRaspred");
        }


        public ActionResult AppRaspredNew(Application app)
        {
            List<Application> apps = new List<Application>();
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            // List<Application> apps = new List<Application>();
            var i = 0;

            //var OtdelId = (string)Session["OtdelyId"];

            // List<Application> a = new List<Application>();
            // Какое представление заявки? У начальника??
            string query1 = "Select IdApp,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem, Priority_Otdel,Date_registration,Superviser,Naimen_otd,Status from apps,otdely where apps.Status='" + "На очереди" + "' and   apps.IdOtdely=otdely.IdOtdely";//where IdOtdely='" + sotr.IdOtdely + "'";
            MySqlCommand cmd1 = new MySqlCommand(query1);
            cmd1.Connection = con;
            con.Open();

            MySqlDataReader sdr1 = cmd1.ExecuteReader();

            while (sdr1.Read()) { i = i + 1; apps.Add(new Application { IdOtdely = sdr1["naimen_otd"].ToString(), idApp = sdr1["IdApp"].ToString(), Date_create = sdr1["Date_Create"].ToString(), Aim = sdr1["Aim"].ToString(), Naimen_PO = sdr1["Naimen_PO"].ToString(), Aim_create_using = sdr1["Aim_create_using"].ToString(), Sotr_using = sdr1["Sotr_using"].ToString(), Time_using = sdr1["Time_using"].ToString(), Konfident = sdr1["Konfident"].ToString(), Requirem = sdr1["Requirem"].ToString(), Status = sdr1["Status"].ToString() }); }
            app.s = i.ToString();
            ViewBag.Application = app.s;
            sdr1.Close();
            // string query2 = "call new_procedure()";
            //  MySqlCommand command = new MySqlCommand(query2);
            //  command.Connection = con;
            //  command.ExecuteNonQuery();
            con.Close();
            return View(apps);
        }

        [HttpGet]
        public ActionResult Superviser(Application app, String st, String Number)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            csb.AllowZeroDateTime = true;
            //  csb.ConvertZeroDateTime = true
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Application> ap = new List<Application>();
            var SotrId = (string)Session["idSotr"];
            var FIO = (string)Session["FIO"];
            // string query11 = "select FIO,idRole from sotr where idSotr='"+SotrId+"' ";
            string query = "Select idApp,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status, Priority_Otdel,Date_registration,Priority_Developer,Date_analise_beg,Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation,Notes,Current_status,Developer,Naimen_otd,FIO from apps,otdely,sotr  where apps.Superviser= '" + SotrId + "' and apps.IdOtdely=otdely.IdOtdely and apps.Developer=sotr.idSotr    ";//where IdOtdely='" + sotr.IdOtdely + "'";
            string query5 = "Select idApp,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status, Priority_Otdel,Date_registration,Priority_Developer,Date_analise_beg,Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation,Notes,Developer,Current_status,Naimen_otd from apps,otdely  where apps.Superviser= '" + SotrId + "' and Current_status='" + st + "' and apps.IdOtdely=otdely.IdOtdely ";//where IdOtdely='" + sotr.IdOtdely + "'";
            string query6 = "Select idApp,Date_create,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status, Priority_Otdel,Date_registration,Priority_Developer,Date_analise_beg,Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation,Notes,Developer,Current_status,Naimen_otd,FIO from apps,otdely,sotr  where apps.Superviser= '" + SotrId + "' and idApp='" + Number + "' and apps.IdOtdely=otdely.IdOtdely and apps.Developer=sotr.idSotr ";//where IdOtdely='" + sotr.IdOtdely + "'";
            List<Application> applications = new List<Application>();
            //   static  List<Application> apps = new List<Application>();

            List<String> status = new List<String>();
            status.Insert(0, "все");
            status.Insert(1, "На очереди");
            status.Insert(2, "Анализ");
            status.Insert(3, "Разработка");
            status.Insert(4, "Тестирование");
            status.Insert(5, "Закрыта");

            SelectList Status = new SelectList(status, st);
            ViewBag.Status = Status;
            // status.Insert(0, new string { "" });
            MySqlCommand cmd = new MySqlCommand(query);
            /* if (st == "0")
             {
                 cmd = new MySqlCommand(query);
             }
             else { cmd = new MySqlCommand(query5); }*/
            
   
            cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
      
                cmd = new MySqlCommand(query);
                while (sdr.Read())
                {
                    ap.Add(new Application { Current_status = sdr["Current_status"].ToString(), idApp = sdr["idApp"].ToString(), Date_create = sdr["Date_Create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString(), Aim_create_using = sdr["Aim_create_using"].ToString(), Sotr_using = sdr["Sotr_using"].ToString(), Time_using = sdr["Time_using"].ToString(), Konfident = sdr["Konfident"].ToString(), Requirem = sdr["Requirem"].ToString(), Status = sdr["Status"].ToString(), Priority_Otdel = sdr["Priority_Otdel"].ToString(), Date_registration = sdr["Date_registration"].ToString(), Developer = sdr["FIO"].ToString(), Priority_Developer = sdr["Priority_Developer"].ToString(), Date_analise_beg = sdr["Date_analise_beg"].ToString(), Date_analise_end = sdr["Date_analise_end"].ToString(), Date_test_beg = sdr["Date_test_beg"].ToString(), Date_test_end = sdr["Date_test_end"].ToString(), Date_expluatation = sdr["Date_expluatation"].ToString(), Notes = sdr["Notes"].ToString(), IdOtdely = sdr["Naimen_otd"].ToString() });

                }
         

            /*  List<Otdely> otdely = new List<Otdely>();

              List<Role> roles = new List<Role>();

              // IQueryable<Sotr> sotrsq = sotrs;

              string query1 = "Select IdOtdely,Naimen_otd from otdely";

              string query2 = "Select idRole,Naimen_role from role";


              MySqlCommand cmd1 = new MySqlCommand(query1);
              MySqlCommand cmd2 = new MySqlCommand(query2);


               cmd.Connection = con;
              cmd1.Connection = con;
              cmd2.Connection = con;
              cmd1.ExecuteNonQuery();
              MySqlDataReader dr = cmd1.ExecuteReader();
              while (dr.Read()) { otdely.Add(new Otdely { IdOtdely = dr["IdOtdely"].ToString(), Naimen_otd = dr["Naimen_otd"].ToString() }); }
              dr.Close();
              otdely.Insert(0, new Otdely { IdOtdely = "0", Naimen_otd = "Все" });
              SelectList Otdely = new SelectList(otdely, "IdOtdely", "Naimen_otd");
              ViewBag.Otdely = Otdely;*/



            con.Close();
            return View(ap);




        }

        [HttpGet]
        public ActionResult ChangeAppSuperviser(string id)
        {

            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            csb.AllowZeroDateTime = true;
            // csb.ConvertZeroDateTime = true;
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            string query = "select*from apps where idApp='" + id + "'";
            List<Application> list = new List<Models.Application>();
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            Session["id"] = id;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { list.Add(new Application { Date_analise_beg = reader["Date_analise_beg"].ToString(), Date_analise_end = reader["Date_analise_end"].ToString(), Date_test_beg = reader["Date_test_beg"].ToString(), Date_test_end = reader["Date_test_end"].ToString(), Date_expluatation = reader["Date_expluatation"].ToString(), Developer = reader["Developer"].ToString() }); }
            ViewBag.list = list;
            reader.Close();
            List<Sotr> list1 = new List<Sotr>();
            string query1 = "select FIO,idSotr from sotr where idRole in (3,5)";//where IdOtdely='" + sotr.IdOtdely + "'";
            MySqlCommand cmd1 = new MySqlCommand(query1);
            cmd1.Connection = con;
            // con.Open();
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read()) { list1.Add(new Sotr { idSotr = reader1["idSotr"].ToString(), FIO = reader1["FIO"].ToString() }); }
            SelectList list2 = new SelectList(list1, "idSotr", "FIO");
            ViewBag.listSotr = list2;
            con.Close();
            return View();
        }
        [HttpPost]
        public ActionResult ChangeAppSuperviser(string id, Application app, string idSotr)
        {

            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            string query;


            query = "Update apps set Date_analise_beg=If('" + app.Date_analise_beg + "'='',Date_analise_beg,'" + app.Date_analise_beg + "'), Date_analise_end=If('" + app.Date_analise_end + "'='',Date_analise_end,'" + app.Date_analise_end + "') , Date_test_beg=if('" + app.Date_test_beg + "'='',Date_test_beg,'" + app.Date_test_beg + "'), Date_test_end=if('" + app.Date_test_end + "'='',Date_test_end,'" + app.Date_test_end + "') , Date_expluatation=If('" + app.Date_expluatation + "'='',Date_expluatation,'" + app.Date_expluatation + "'), Developer=if('" + app.Developer + "'='',Developer,'" + app.Developer + "'), Notes='" + app.Notes + "' where idApp= '" + id + "'";//where IdOtdely='" + sotr.IdOtdely + "'";

            string query2 = "call new_procedure()";
            MySqlCommand cmd2 = new MySqlCommand(query2);
            cmd2.Connection = con;
            MySqlCommand cmd = new MySqlCommand(query);
            //MySqlDataReader reader = cmd.ExecuteReader();

            cmd.Connection = con;
            con.Open();
            //   if (reader.Read())
            //  {
            // if (ModelState.IsValid)
            //  {
            cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            //    cmd2.ExecuteNonQuery();

            //   string query3="select email from sotr where idSotr='"+app.Developer+"'";
            //   MySqlCommand command = new MySqlCommand(query3);
            // command.Connection = con;
            // MySqlDataReader rdr = command.ExecuteReader();
            //string emailDeveloper;
            //  Contact("Message");
            // if (rdr.Read()) { emailDeveloper = rdr["email"].ToString(); Contact(emailDeveloper); }

            //  app.Developer
            //Contact("032000-0802033@032", "Вам новая заявка", "Зайдите в электронный журнал!");
            //  Response.Write("<script>alert('Данные успешно изменены!');</script>");
          //  Contact("0802033@032", "У вас новая заявка", "Проверьте электронный журнал!");
            //"032000-0802033@032" <032000-0802033@032>
            //   Contact("Верепекина Е.А. 032000-0802033/032/PFR/RU", "Верепекина Е.А. 032000-0802033/032/PFR/RU", "Hello,this is a new message");
            con.Close();
            return RedirectToAction("Superviser");

        }

        [HttpGet]
        public ActionResult PriorityDeveloper(string id)
        {//получение из дроп дауна

            return View();
        }
        [HttpPost]
        public ActionResult PriorityDeveloper(Application app, string id)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Application> apps = new List<Application>();
            var SotrId = (string)Session["idSotr"];
            //var FIO = (string)Session["FIO"];
            string query = "update apps set Priority_Developer='" + app.Priority_Developer + "'where idApp='" + id + "'";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Developer");
        }

        public ActionResult Developer(Application app)
        {
            //сортировка по должности конкретно его и по Айди т.е. where id sotr==""
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            csb.AllowZeroDateTime = true;
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Application> apps = new List<Application>();
            var SotrId = (string)Session["idSotr"];
            var FIO = (string)Session["FIO"];
            string query = "Select IdApp,Aim,Naimen_PO, Aim_create_using,Sotr_using,Time_using,Konfident,Requirem,Status,Priority_Developer,Date_analise_beg,Date_analise_end,Date_test_beg,Date_test_end,Date_expluatation,Current_status,Notes,Superviser,FIO from apps,sotr where Developer='" + SotrId + "' and apps.Superviser=sotr.idSotr";//where IdOtdely='" + sotr.IdOtdely + "'";
                                                                                                                                                                                                                                                                                                                                                 // apps.Superviser=sotr.idSotr
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read()) { apps.Add(new Application { idApp = sdr["IdApp"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString(), Aim_create_using = sdr["Aim_create_using"].ToString(), Sotr_using = sdr["Sotr_using"].ToString(), Time_using = sdr["Time_using"].ToString(), Konfident = sdr["Konfident"].ToString(), Requirem = sdr["Requirem"].ToString(), Status = sdr["Status"].ToString(), Superviser = sdr["FIO"].ToString(), Priority_Developer = sdr["Priority_Developer"].ToString(), Date_analise_beg = sdr["Date_analise_beg"].ToString(), Date_analise_end = sdr["Date_analise_end"].ToString(), Date_test_beg = sdr["Date_test_beg"].ToString(), Date_test_end = sdr["Date_test_end"].ToString(), Date_expluatation = sdr["Date_expluatation"].ToString(), Notes = sdr["Notes"].ToString(), Current_status = sdr["Current_status"].ToString() }); }
            con.Close();
            return View(apps);

        }
        [HttpGet]
        public ActionResult Delete_otd(string id)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            //get ID сотрудника
            string query = "Delete from otdely where IdOtdely='" + id + "'";//where IdOtdely='" + sotr.IdOtdely + "'";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("AddOtdely");
        }


        public ActionResult Add_otd_new(Otdely otdely)
        {
            // Вывести список отделов и сделать его изменение
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            /*    List<Otdely> otdel = new List<Otdely>(); 
         
                string query1 = "Select * from otdely";
                MySqlCommand cmd1 = new MySqlCommand(query1);
                cmd1.Connection = con;
                con.Open();
                MySqlDataReader dr= cmd1.ExecuteReader();
                while (dr.Read()) { otdel.Add(new Otdely { IdOtdely = dr["IdOtdely"].ToString(), Naimen_otd = dr["Naimen_otd"].ToString() }); }
            
         
                //return View(otdel);
         
             */
            string query = "INSERT INTO otdely (Naimen_otd) Values ('" + otdely.Naimen_otd + "')";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            //Вывести окно "Успешно добавлено!"
            //  MySqlDataReader sdr = cmd.ExecuteReader();
            //  while (sdr.Read()) { apps.Add(new Application { Date_create = sdr["Date_create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString() }); }
            //  con.Close(); 
            return View("Add_otd_new");
            // возвратить окно "Добавлено"
            //   return View("AddOtdely");
        }


        [HttpGet]
        public ActionResult AddOtdely()
        {
            // Вывести список отделов и сделать его изменение
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Port = 3306;
            csb.UserID = "root";
            csb.Database = "database";
            csb.Password = "1234";
            MySqlConnection con = new MySqlConnection(csb.ConnectionString);
            List<Otdely> otdel = new List<Otdely>();

            string query1 = "Select * from otdely";
            MySqlCommand cmd1 = new MySqlCommand(query1);
            cmd1.Connection = con;
            con.Open();
            MySqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read()) { otdel.Add(new Otdely { IdOtdely = dr["IdOtdely"].ToString(), Naimen_otd = dr["Naimen_otd"].ToString() }); }


            //return View(otdel);

            /*   string query = "INSERT INTO otdely (IdOtdely,Naimen_otd) Values ('" + otdely.IdOtdely + "','" + otdely.Naimen_otd + "')";
               MySqlCommand cmd = new MySqlCommand(query);
               cmd.Connection = con;
               cmd.ExecuteNonQuery();*/
            //  MySqlDataReader sdr = cmd.ExecuteReader();
            //  while (sdr.Read()) { apps.Add(new Application { Date_create = sdr["Date_create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString() }); }
            con.Close();
            return View(otdel);
            // return View();
        }
        void ConnectionString()
        {
            con.ConnectionString =
                "Data Source=127.0.0.1;port=3306;Initial catalog=database;UserId=root;Password=1234";

        }
        MySqlConnection con = new MySqlConnection();
        // MySqlConnection con1 = new MySqlConnection();
        MySqlCommand cmd = new MySqlCommand();
        MySqlCommand cmd1 = new MySqlCommand();
        [HttpGet]
        public ActionResult register(Sotr sotr)
        {
            // получить список отделов из БД с индексом и наименованием их
            //выбору делать в представл по наименованию а запоминать индекс в сотрудника
            
            return View();
        }

        /*
         
               private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

         */
        /*   public static string HashPassword(string password)
           {
               byte[] salt;
               byte[] buffer2;
               if (password == null)
               {
                   throw new ArgumentNullException("password!");
               }

               return password;
           }*/






        [HttpPost]
        public ActionResult Register(Sotr sotr)
        {

           

                Response.Write("<script>alert('Новый пользователь успешно добавлен!');</script>");
                //  con1.Close();
                return View();
         
            // return View();
            //  con1.Close();
            // con.Close();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}