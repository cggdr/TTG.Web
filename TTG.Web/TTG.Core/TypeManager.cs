using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
  public  class TypeManager:BaseManager<Type>
    {
        public int CountType()
        {
            int count = Count(); 
            return count;
        }
    }
}
