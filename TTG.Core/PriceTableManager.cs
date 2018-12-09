using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
    public class PriceTableManager:BaseManager<PriceTable>
    {
        public List<PriceTable> FindList(string _s)
        {
            var _where = PredicateBuilder.True<PriceTable>();
            _where = _where.And(u => u.CoinToCoin.Contains(_s));

            List<PriceTable> pd = Repository.FindList(_where.Expand()).ToList();
            return pd;
        }
        public List<PriceTable> FindListNowaday(string _s)
        {
            var _where = PredicateBuilder.True<PriceTable>();
            _where = _where.And(u => u.CoinToCoin.Contains(_s));
           
            List<PriceTable> pd = Repository.FindList(_where.Expand()).OrderByDescending(u => u.KDateTime).Take(12).ToList();
            return pd;
        }
    }
}
