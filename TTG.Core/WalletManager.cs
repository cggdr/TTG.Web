using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public  class WalletManager:BaseManager<Wallet>
    { private VirtualCurrencyManager _vir = new VirtualCurrencyManager();
        public Wallet GetWallet(int id,string coinname)
        {
            int virID=_vir.FindVirID(coinname);
            var _where = PredicateBuilder.New<Wallet>();
            _where = _where.And(u => u.UserID == id);
            _where = _where.And(u => u.VirCurID == virID);
            return Repository.Find(_where);
        }
        public string GetWalletName(int virCurID)
        {
            return _vir.Find(virCurID).Name;
        }
        public List<int> GetWalletIDList(int id)
        {
            List<Wallet> wa=FindList(u => u.UserID == id).ToList();
            List<int> wd =new List<int>();
           foreach(var item in wa)
            {
                wd.Add(item.WalletID);
            }
            return wd;
        }
    }

}
