using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Models
{
    public class Application
    {
        public string idApp { get; set; }//автоинкрементный
        public int idSotr { get; set; }
        public string IdOtdely { get; set; }
        public string Date_create { get; set; } // Дата создания заявки (автоматически)
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        public string Aim { get; set; } // на доработку или разработку (optional)
        public string Naimen_PO { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        public string Aim_create_using { get; set; } // цель создания или использования, описание
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        public string Sotr_using { get; set; } // сотрудники, которые используют
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        public string Time_using { get; set; } //периодичность использования

        public string Konfident { get; set; } // конфиденциальность
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        public string Requirem { get; set; } //Требования (список?)
        public string Status { get; set; } //зарегистрирована заявка или нет ? УБПРАТЬ и поставить "На очередь"
        public string Priority_Otdel { get; set; } // приоритетность со стороны отдела
        public string Date_registration { get; set; } // дата регистрации заявки
        public string Current_status { get; set; } // текущий статус по алгоритмам
        public string Superviser { get; set; } // курирующий проект
        public string Developer { get; set; } // разработчик
        public string Priority_Developer { get; set; }// приоритетность со стороны разработчика
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy'/'MM'/'0:dd}", ApplyFormatInEditMode = true)]
        public string Date_analise_beg { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public string Date_analise_end { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public string Date_test_beg { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public string Date_test_end { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public string Date_expluatation { get; set; }
        public string Notes { get; set; } // примечания
                                          // List<Otdely> otdely = new List<Otdely>();
        public string s { get; set; }
        //  List<Application> ap(); 
        public string sotr_using_dr { get; set; }
        public string time_using_dr { get; set; }
        public string konf_konf { get; set; }
        public string konf_pers { get; set; }
        public string konf_bd { get; set; }
      //  public documentation doc = new documentation();
    }
}