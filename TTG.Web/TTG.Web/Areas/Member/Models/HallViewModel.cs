using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Member.Models
{
    public class HallViewModel
    {

        public DateTime EntrustTime { get; set; }
        public int EntrusterID { get; set; }
        public double Price { get; set; }
        public double  TotalAmount { get; set; }
        public double  SelledAmount { get; set; }
        public double  RemainingAmount { get; set; }
        public int ID { get; set; }
        public string CoinToCoin { get; set; }
        public int IsSuccess { get; set; }
    }
}