using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest.Repository
{
    public class Repository<T> : IRepository<T>
    {
        public virtual string All(string script)
        {
            return typeof(T).Name + ":" + script;
        }
    }
}
