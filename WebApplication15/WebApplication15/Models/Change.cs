using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class Change
    {
        public string idchange { get; set; }
        public string AppId { get; set; }
        public string date_change { get; set; }
        public string Date_analise_beg { get; set; }
        public string Date_analise_end { get; set; }
        public string Date_test_beg { get; set; }
        public string Date_test_end { get; set; }
        public string Date_expluatation { get; set; }
        public string Notes { get; set; } // примечания
        public string Developer { get; set; }
        public string OtdelyID { get; set; }
    }
}