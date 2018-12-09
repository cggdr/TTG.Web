using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public class InteractionManager:BaseManager<Interaction>
    {
        private WalletManager _walletManager = new WalletManager();
        public List<Interaction> GetBill(int id)
        { List<int> wd = _walletManager.GetWalletIDList(id);
            List<Interaction> interaction = new List<Interaction>();
            foreach(var item in wd)
            {
                try
                {
                    List<Interaction> it = FindList(u => u.FWalletID == item).ToList();
                    interaction.AddRange(it);
                }
                catch { }
              }
            return interaction;
        } 
    }
}
