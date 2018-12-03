using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Member.Models
{
    public class PriceTableViewModel
    {
        public double OpeningPrice { get; set; }
        public DateTime KDateTime { get; set; }
        public double  ClosingPrice { get; set; }
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public double Amount { get; set; }
    }
}