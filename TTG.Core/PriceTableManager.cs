using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace TTG.Core
{
    public class PriceTableManager:BaseManager<PriceTable>
    {
        public List<PriceTable> FindList(string _s)
        {
            var _where = PredicateBuilder.New<PriceTable>();
            _where = _where.And(u => u.CoinToCoin.Contains(_s));

            List<PriceTable> pd = Repository.FindList(_where).ToList();
            return pd;
        }
        public List<PriceTable> FindListNowaday(string _s)
        {
            var _where = PredicateBuilder.New<PriceTable>();
            _where = _where.And(u => u.CoinToCoin.Contains(_s));
           
            List<PriceTable> pd = Repository.FindList(_where).OrderByDescending(u => u.KDateTime).Take(12).ToList();
            return pd;
        }
        public PriceTable IfFirstOrNo(string coinname,DateTime time)
        {
            var _where = PredicateBuilder.New<PriceTable>();
            _where = _where.And(u => u.CoinToCoin == coinname);
            var a = DateTime.Now.ToString("yyyy/MM/dd");
            
            _where = _where.And(p => DbFunctions.DiffHours(p.KDateTime,time)< 24);
            PriceTable pt=Repository.Find(_where);
            return pt;

        }
     
    }
}
