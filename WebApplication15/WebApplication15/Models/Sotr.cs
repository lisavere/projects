using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using WebApplication15.Models;
using WebApplication15.Controllers;
using System.Data;
namespace WebApplication15.Models
{
    public class Sotr
    {
        public string idSotr { get; set; }
        public string IdOtdely { get; set; }
        public string FIO { get; set; }
        public string email { get; set; }
        public string Dolzn { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string psswd { get; set; }
        [Required]
        [Display(Name = "Повторный пароль")]
        [DataType(DataType.Password)]
        // [Range(3, 20, ErrorMessage = "поле должно содержать более 3-х символов")]
        public string newpsswd { get; set; }
        [Required]
        [Compare("newpsswd", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string Confirmpsswd { get; set; }

        //фильтрация по отделам, ролям, по должностям
        public int? OtdelID { get; set; }
        public Otdely Otdel { get; set; }
      //  public Role role { get; set; }


        public string idRole { get; set; }


    }
}