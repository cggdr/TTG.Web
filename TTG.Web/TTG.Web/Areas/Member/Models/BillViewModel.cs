using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Member.Models
{
    public class BillViewModel
    {
        public DateTime OperatingTime { get; set; }
        public string CoinName { get; set; }
        public double Amount { get; set; }
        public string WalletAddress { get; set; }
        public int Status { get; set; }
    }
}