using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest
{
    public static class TestStaticType
    {
        #region Method
        public static string TestGenericTypeWithMethod()
        {
            return ("No T");

        }

        public static string TestGenericTypeWithMethod(string first)
        {
            return ("No T" + ":" + first);
        }

        public static string TestGenericTypeWithMethod(string first, string second)
        {
            return ("No T" + ":" + first + ":" + second);
        }
        #endregion

        #region MethodWithGeneric
        public static string TestGenericTypeWithMethodWithGeneric<O>()
        {
            return ("No T" + typeof(O).Name);

        }

        public static string TestGenericTypeWithMethodWithGeneric<O>(string first)
        {
            return ("No T" + typeof(O).Name + ":" + first);
        }

        public static string TestGenericTypeWithMethodWithGeneric<O>(string first, string second)
        {
            return ("No T" + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion

        public static void QQ(string pp)
        {
            var n = pp;
        }
    }
}
