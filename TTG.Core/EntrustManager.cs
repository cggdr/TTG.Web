using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
  public  class EntrustManager:BaseManager<Entrust>
    {
        public List<Entrust> FindListNowaday(string _s,int? id)
        {
            int i = 0;
            int j = _s.Length - 1;
            while (i < _s.Length)
            {
                if (_s[i] != '/') i++;
                else break;
            }
            string coinname = _s.Substring(0, i-0);
            string payname = _s.Substring(i+1 , j-i);
            var _where = PredicateBuilder.New<Entrust>();
            
            _where = _where.And(u => u.CoinName==coinname);
            _where = _where.And(u => u.PayCoinName == payname);
            if (id != -1) _where = _where.And(u => u.User.UserID == id );
            List<Entrust> pd = Repository.FindList(_where).OrderByDescending(u => u.SucOrDefTime).Take(12).ToList();
            return pd;
        }

        public List<Entrust> FindListSell(string _s)
        {
            int i = 0;
            int j =_s.Length;
            while (i < _s.Length)
            {
                if (_s[i] != '/') i++;
                else break;
            }
            string coinname = _s.Substring(0, i - 0);
            string payname = _s.Substring(i + 1, j - i-1);
            var _where = PredicateBuilder.New<Entrust>();

            _where = _where.And(u => u.CoinName == coinname);
            _where = _where.And(u => u.PayCoinName == payname);
            _where = _where.And(u => u.IsSuccess == 0);
         
            return FindList(_where).OrderByDescending(u => u.EnstructTime).ToList();
        }
        public override Entrust Find(int id)
        {
            return Repository.Find(u => u.ID == id);
        }
        public List<Entrust> FindListNowEn(int id,string cointocoin)
        {   var _where = PredicateBuilder.New<Entrust>();
            if (!string.IsNullOrEmpty(cointocoin))
            {
                int i = 0;
                int j = cointocoin.Length - 1;
                while (i < cointocoin.Length)
                {
                    if (cointocoin[i] != '/') i++;
                    else break;
                }
                string coinname = cointocoin.Substring(0, i - 0);
                string payname = cointocoin.Substring(i + 1, j - i);
                _where = _where.And(u => u.CoinName == coinname);
                _where = _where.And(u => u.PayCoinName == payname);
            }
            _where = _where.And(u => u.FUserID == id);
            _where = _where.And(u=>u.IsSuccess==0);
            List<Entrust> pd = new List<Entrust>();
            if(!string.IsNullOrEmpty(cointocoin)) pd = Repository.FindList(_where).OrderByDescending(u => u.EnstructTime).Take(3).ToList();
            else pd = Repository.FindList(_where).OrderByDescending(u => u.EnstructTime).ToList();
           
            return pd;
        }
        public List<Entrust> FindListHistoryEn(int id, string cointocoin)
        { var _where = PredicateBuilder.New<Entrust>();
            if(!string.IsNullOrEmpty(cointocoin))
         { 
            int i = 0;
            int j = cointocoin.Length - 1;
            while (i < cointocoin.Length)
            {
                if (cointocoin[i] != '/') i++;
                else break;
            }
            string coinname = cointocoin.Substring(0, i - 0);
            string payname = cointocoin.Substring(i + 1, j - i);
            _where = _where.And(u => u.CoinName == coinname);
            _where = _where.And(u => u.PayCoinName == payname);
        }
            _where = _where.And(u => u.FUserID == id);
            _where = _where.And(u => u.IsSuccess != 0);
            List<Entrust> pd = new List<Entrust>();
            if (!string.IsNullOrEmpty(cointocoin))  pd = Repository.FindList(_where).OrderByDescending(u => u.EnstructTime).Take(3).ToList();
            else pd = Repository.FindList(_where).OrderByDescending(u => u.EnstructTime).ToList();
            return pd;
        }
        public List<Entrust> FindListSuccessfulEn(int id, string cointocoin)
        {
            var _where = PredicateBuilder.New<Entrust>();
            if (!string.IsNullOrEmpty(cointocoin))
            {
                int i = 0;
                int j = cointocoin.Length - 1;
                while (i < cointocoin.Length)
                {
                    if (cointocoin[i] != '/') i++;
                    else break;
                }
                string coinname = cointocoin.Substring(0, i - 0);
                string payname = cointocoin.Substring(i + 1, j - i);
                _where = _where.And(u => u.CoinName == coinname);
                _where = _where.And(u => u.PayCoinName == payname);
            }
            _where = _where.And(u => u.FUserID == id);
            _where = _where.And(u => u.IsSuccess == 1);

            List<Entrust> pd = Repository.FindList(_where).OrderByDescending(u => u.EnstructTime).ToList();
            return pd;
        }
    }
}
