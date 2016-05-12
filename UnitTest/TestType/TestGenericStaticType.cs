using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Test
{
    public static class TestGenericStaticType<T> 
    {
        #region Method
        public static string TestMethod()
        {
            return (typeof(T).Name);
        }

        public static string TestMethod(string first)
        {
            return (typeof(T).Name + ":" + first);
        }

        public static string TestMethod(string first, string second)
        {
            return (typeof(T).Name + ":" + first + ":" + second);
        }
        #endregion
        #region MethodWithGeneric
        public static string TestMethod<O>()
        {
            return (typeof(T).Name + typeof(O).Name);

        }

        public static string TestMethod<O>(string first)
        {
            return (typeof(T).Name + typeof(O).Name + ":" + first);
        }

        public static string TestMethod<O>(string first, string second)
        {
            return (typeof(T).Name + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion
    }
}
