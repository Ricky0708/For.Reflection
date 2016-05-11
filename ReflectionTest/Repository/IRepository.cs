using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest.Repository
{
    public interface IRepository<T>
    {
        string All(string script);
    }
}
