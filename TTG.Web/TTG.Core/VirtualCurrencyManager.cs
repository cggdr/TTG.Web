using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace TTG.Core
{
   public class VirtualCurrencyManager:BaseManager<VirtualCurrency>
    {
        public int FindVirID (string coinname)
        {
          
            var _where = PredicateBuilder.True<VirtualCurrency>();


            return Repository.Find(u => u.Name == coinname).VirCurID;

        }
     }
}
