using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest
{
    public static class TestGenericStaticType<T>
    {
        #region Method
        public static string TestGenericTypeWithMethod()
        {
            return (typeof(T).Name);
        }

        public static string TestGenericTypeWithMethod(string first)
        {
            return (typeof(T).Name + ":" + first);
        }

        public static string TestGenericTypeWithMethod(string first, string second)
        {
            return (typeof(T).Name + ":" + first + ":" + second);
        }
        #endregion
        //delegate object ObjectActivator();
        #region MethodWithGeneric
        public static string TestGenericTypeWithMethodWithGeneric<O>()
        {
            return (typeof(T).Name + typeof(O).Name);

        }

        public static string TestGenericTypeWithMethodWithGeneric<O>(string first)
        {
            return (typeof(T).Name + typeof(O).Name + ":" + first);
        }

        public static string TestGenericTypeWithMethodWithGeneric<O>(string first, string second)
        {
            return (typeof(T).Name + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion
    }
}
