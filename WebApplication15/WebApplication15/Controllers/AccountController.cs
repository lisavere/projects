using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using WebApplication15.Models;
using WebApplication15.Controllers;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;

namespace WebApplication15.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        MySqlConnection con = new MySqlConnection();
        // MySqlConnection con1 = new MySqlConnection();
        MySqlCommand cmd = new MySqlCommand();
        MySqlCommand cmd1 = new MySqlCommand();
        MySqlCommand cmd2 = new MySqlCommand();
        MySqlCommand cmd3 = new MySqlCommand();
        MySqlCommand cmd4 = new MySqlCommand();
        MySqlCommand cmd5 = new MySqlCommand();
        MySqlCommand cmd6 = new MySqlCommand();
        MySqlCommand cmd7 = new MySqlCommand();
        MySqlCommand cmd8 = new MySqlCommand();
        public ActionResult Index()
        {
            return View();
        }

        static string GetHash(string plaintext)
        {
            var sha = new SHA1Managed();
            //перевести строку в байт массив
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            return Convert.ToBase64String(hash);
        }
        [HttpGet]
        public ActionResult login()
        {

            return View();
        }
        void ConnectionString()
        {
            con.ConnectionString = "Data Source=127.0.0.1;port=3306;Initial catalog=database;UserId=root;Password=1234";

        }
        [HttpPost]
        public ActionResult Login(Sotr sotr)
        {
            //List<Sotr> sotrs = new List<Sotr>();
            ConnectionString();

            string hashpsswd = GetHash(sotr.psswd);
            cmd.CommandText = "Select*from sotr where Login='" + sotr.Login + "' and psswd='" + hashpsswd + "'";

            cmd.Connection = con;
            cmd3.Connection = con;
            con.Open();

            MySqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {

                //  List<Sotr> sotr1 = new List<Sotr>();
                //  cmd3.CommandText = " Select IdSotr,IdOtdely from sotr whereLogin='" + sotr.Login + "' and psswd='" + sotr.psswd + "'";
                // sotr1.Add(new Sotr{idSotr=sdr["idSotr"].ToString(),IdOtdely=sdr["IdOtdely"].ToString()});

                string OtdelId = sdr["IdOtdely"].ToString();
                string SotrId = sdr["idSotr"].ToString();
                string dolzn = sdr["Dolzn"].ToString();
                string role = sdr["idRole"].ToString();
                string FIO = sdr["FIO"].ToString();
                //sotr.IdOtdely;
                Session["OtdelyId"] = OtdelId;
                Session["idSotr"] = SotrId;
                Session["FIO"] = FIO;
                //  Session["idRole"] = role;
                //   while (sdr.Read()) { sotr.Add(new Application { idApp = sdr["IdApp"].ToString(), Date_create = sdr["Date_Create"].ToString(), Aim = sdr["Aim"].ToString(), Naimen_PO = sdr["Naimen_PO"].ToString(), Aim_create_using = sdr["Aim_create_using"].ToString(), Sotr_using = sdr["Sotr_using"].ToString(), Time_using = sdr["Time_using"].ToString(), Konfident = sdr["Konfident"].ToString(), Requirem = sdr["Requirem"].ToString(), Status = sdr["Status"].ToString() }); }


                //Session["'" + sotr.IdOtdely + "'"] = IdOtdely;
                // Sotr sotrId = new Sotr();
                //  Idsotr = sotrId.idSotr;
                //   if (sotr.idRole == 1) { return View("Application"); } // sotr
                //  else { if (sotr.idRole == 2) { return View("Index"); } //boss
                //  else { if (sotr.idRole == 3) { return View("Index");} //superviser
                //  else {return View ("Admin");} //admin
                //  } 
                // }
                // Расписать переменные по переменным по роли
                //else { 

                if (role == "4") //boss
                {
                    //con.Close();
                    return RedirectToAction("AppRaspred", "Home");
                }

                else
                {

                    if (role == "1") { return RedirectToAction("SpisokSotr", "Home"); } //admin
                    else
                    {
                        if (role == "2") { return RedirectToAction("Application", "Home"); } // sotr
                        else
                        {
                            if (role == "3") { return RedirectToAction("Superviser", "Home"); } //superviser
                            else { return RedirectToAction("Developer", "Home"); } //developer
                        }
                    }


                    //return RedirectToAction("CreateApp","Home");
                }
                // представление alert-danger если пследнеее не оч

                //  View("Succeed");
                //}
                //return View("Index");
            }
            con.Close();
            return View();

        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("login", "Account");
        }
        [HttpGet]
        public ActionResult resetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(Sotr sotr)
        {
            //подключаешься к БД, получаешь текущую сессию, и пишешь если пароль введенный совпадает с паролем в БД,
            //то то задаешь новый пароль дважды, если они равны, то сохраняешь новый пароль
            ConnectionString();
          //  if (ModelState.IsValid)
          //  {
                string hashpsswd = GetHash(sotr.Confirmpsswd);
                var SotrId = (string)Session["idSotr"];
                cmd4.CommandText = "Select psswd from sotr where idSotr='" + SotrId + "'";
                cmd5.CommandText = "Update sotr set psswd='" + hashpsswd + "' where idSotr='" + SotrId + "'";
                // cmd6.CommandText = "Insert into sotr(psswd) values('" + sotr.psswd + "')where idSotr='" + SotrId + "'";
                cmd4.Connection = con;
                cmd5.Connection = con;
                cmd6.Connection = con;
                //   con.Open();
                // ValidateInputAttribute input;
                string psswd1;
                string hashpsswd1 = GetHash(sotr.psswd);
                string psswd2 = hashpsswd1;
                //  ValidateInputAttribute psswd2;//введенный пароль в input
                // con.Open();
                con.Open();
                MySqlDataReader sdr2 = cmd4.ExecuteReader();
                // MySqlDataReader sdr3 = cmd5.ExecuteReader();
                // MySqlDataReader sdr4 = cmd6.ExecuteReader();

                if (sdr2.Read())
                {
                    psswd1 = sdr2["psswd"].ToString();
                    if (psswd2 == psswd1)
                    {
                        sdr2.Close();

                        if (sotr.newpsswd == sotr.Confirmpsswd) { cmd5.ExecuteNonQuery(); }

                        //вывести сообщение об успешной смене пароля! }
                        else
                        {

                            //Введенные пароли не совпадают выдать сообщение
                            return View("Error");
                        }
                        //cmd5.ExecuteNonQuery();

                        // sdr2.Close();
                        //     cmd5.ExecuteNonQuery();
                        // cmd6.ExecuteNonQuery();
                        //   return View("ResetPassword2");
                        //      sotr.psswd = "0";
                        //     cmd6.ExecuteNonQuery();
                        // ViewBag.Message = "Success";
                        //return View();
                        Response.Write("<script>alert('Пароль успешно изменен!');</script>");
                        return View("login");
                    }
                    else
                    {
                        return
                              //Json( JsonRequestBehavior.AllowGet);
                              //   con.Close();
                              View("Error");
                        //Введенные вами пароли не совпадают или текущий пароль не правильно введен!
                    }
                }



                //Сделать вывод : "Пользователь добавлен!"
                con.Close();
                //если введенный будет равен psswd , то
                //ввести текущий пароль, если пароль из представления совпадает с паролем из бд, то гуд! меняем, если нет, то нет!! на представление
                return View();
             
          //  }
          //  return View();
        }
        [HttpGet]
        public ActionResult register(Sotr sotr)
        {
            // получить список отделов из БД с индексом и наименованием их
            //выбору делать в представл по наименованию а запоминать индекс в сотрудника
            ConnectionString();
            List<Otdely> otdely = new List<Otdely>();
            List<Role> role = new List<Role>();
            string query1 = "Select IdOtdely,Naimen_otd from otdely";
            string query2 = "Select idRole,Naimen_role from role";
            MySqlCommand cmd1 = new MySqlCommand(query1);
            MySqlCommand cmd2 = new MySqlCommand(query2);
            cmd1.Connection = con;
            cmd2.Connection = con;
            con.Open();
            MySqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read()) { otdely.Add(new Otdely { IdOtdely = dr["IdOtdely"].ToString(), Naimen_otd = dr["Naimen_otd"].ToString() }); }
            dr.Close();
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read()) { role.Add(new Role { idRole = dr2["idRole"].ToString(), Naimen_role = dr2["Naimen_role"].ToString() }); }
            dr2.Close();
            SelectList otdels = new SelectList(otdely, "IdOtdely", "Naimen_otd");
            ViewBag.Otdely = otdels;
            SelectList roles = new SelectList(role, "idRole", "Naimen_role");
            ViewBag.Role = roles;
            con.Close();
            return View();
        }
        [HttpPost]
        public ActionResult Register(Sotr sotr)
        {

            // ОПРЕДЕЛИТЬ РОЛЬ
            // Два выпадающих списка роли и отделения
            ConnectionString();
            cmd.CommandText = "Select*from sotr where Login='" + sotr.Login + "'";

            cmd.Connection = con;
            cmd1.Connection = con;
            //  con1.Open();
            con.Open();

            MySqlDataReader sdr1 = cmd.ExecuteReader();
            //MySqlDataReader sdr2 = cmd1.ExecuteNonQuery();
            if (sdr1.Read())
            {
                //Sotr sotrId = new Sotr();
                //  int Idsotr = sotrId.idSotr;
                //con.Close();
                // sdr1.Close();
                return View("Register_fail");
                // con.Close();
            }

            else
            {

                //con.Open();  
                sdr1.Close();
                //sotr.IdOtdely = "1";
                string hashpsswd = GetHash(sotr.psswd);
                cmd1.CommandText = "INSERT INTO sotr(Login,psswd,FIO,Dolzn,IdOtdely,idRole,email) Values('" + sotr.Login + "','" + hashpsswd + "','" + sotr.FIO + "','" + sotr.Dolzn + "','" + sotr.IdOtdely + "','" + sotr.idRole + "','" + sotr.email + "')";
                cmd1.ExecuteNonQuery();
                con.Close();
                //  con1.Close();
                return View("login");
            }
            // return View();
            //  con1.Close();
            // con.Close();
        }

    }
}