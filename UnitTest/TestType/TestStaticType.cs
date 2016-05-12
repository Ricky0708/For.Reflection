using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Test
{
    public static class TestStaticType
    {
        #region Method
        public static string TestMethod()
        {
            return ("No T");

        }

        public static string TestMethod(string first)
        {
            return ("No T" + ":" + first);
        }

        public static string TestMethod(string first, string second)
        {
            return ("No T" + ":" + first + ":" + second);
        }
        #endregion

        #region MethodWithGeneric
        public static string TestMethod<O>()
        {
            return ("No T" + typeof(O).Name);

        }

        public static string TestMethod<O>(string first)
        {
            return ("No T" + typeof(O).Name + ":" + first);
        }

        public static string TestMethod<O>(string first, string second)
        {
            return ("No T" + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion


    }
}
