using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
    //数据上下文工厂 获取当前线程的数据上下文，避免多个上下文的出现
   public class ContextFactory
    {
        public static TTGContext CurrentContext()
        {
            TTGContext _nContext = CallContext.GetData("TTGContext") as TTGContext;
            if (_nContext == null)
            {
                _nContext = new TTGContext();
                CallContext.SetData("TTGContext",_nContext);

            }
            return _nContext;
        }
    }
}
