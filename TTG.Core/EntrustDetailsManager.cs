using System.Collections.Generic;
using System.Linq;

namespace TTG.Core
{
    public class EntrustDetailsManager:BaseManager<EntrustDetails>
    {
       //查询委托号id的委托详情（返回列表） 再sum
     
        public double FindRemainningAmount(int entrustID)
        {
            //List<EntrustDetails> end = new List<EntrustDetails>();
        
            //var _where = PredicateBuilder.True<EntrustDetails>();
            //_where.And(u => u.EntrustID == entrustID);
            //int i= FindList(u=>u.EntrustID==entrustID).Count();
            //当lambda表达式只有一条时不需要连接表达式 否则会出错

           List<EntrustDetails> en=Repository.FindList(u => u.EntrustID == entrustID).ToList();
            return en.Sum(u=>u.Amount);


        }
    }
}
