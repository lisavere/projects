using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

namespace WebApplication15.Models
{
    public class Otdely
    {
        public string IdOtdely { get; set; }
        public string Naimen_otd { get; set; }
        public ICollection<Sotr> sotrs { get; set; }
        public Otdely()
        {
            sotrs = new List<Sotr>();
        }
    }
}