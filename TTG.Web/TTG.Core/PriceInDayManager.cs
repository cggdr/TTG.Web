using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace TTG.Core
{
    public class PriceInDayManager:BaseManager<PriceInDay>
    {
        public List<PriceInDay> FindList(string _s)
        {
            var _where = PredicateBuilder.True<PriceInDay>();
            _where = _where.And(u => u.CoinToCoin.Contains(_s));
          
            List<PriceInDay> pd=Repository.FindList(_where.Expand()).ToList();
            return pd;
        }
       
    }
}
