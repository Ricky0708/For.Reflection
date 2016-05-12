using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest
{
    public class TestGenericType<T>
    {

        #region Ctor
        public TestGenericType()
        {
            //MessageBox.Show(typeof(T).Name);
        }

        public TestGenericType(string first)
        {
        }

        public TestGenericType(string first, string second)
        {
        }
        #endregion

        #region Method
        public string TestGenericTypeWithMethod()
        {
            return (typeof(T).Name);

        }

        public string TestGenericTypeWithMethod(string first)
        {
            return (typeof(T).Name + ":" + first);
        }

        public string TestGenericTypeWithMethod(string first, string second)
        {
            return (typeof(T).Name + ":" + first + ":" + second);
        }
        #endregion

        #region MethodWithGeneric
        public string TestGenericTypeWithMethodWithGeneric<O>()
        {
            return (typeof(T).Name + typeof(O).Name);

        }

        public string TestGenericTypeWithMethodWithGeneric<O>(string first)
        {
            return (typeof(T).Name + typeof(O).Name + ":" + first);
        }

        public string TestGenericTypeWithMethodWithGeneric<O>(string first, string second)
        {
            return (typeof(T).Name + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion
    }
}
