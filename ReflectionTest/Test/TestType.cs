using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionTest
{
    public class TestType
    {

        public TestType pp;

        #region Ctor
        public TestType()
        {
            //MessageBox.Show("No T");
        }

        public TestType(string first)
        {
            MessageBox.Show("No T" + ":" + first);
        }

        public TestType(string first, string second)
        {
            MessageBox.Show("No T" + ":" + first + ":" + second);
        }
        #endregion

        #region Method
        public string TestGenericTypeWithMethod()
        {
            return ("No T");

        }

        public string TestGenericTypeWithMethod(string first)
        {
            return ("No T" + ":" + first);
        }

        public string TestGenericTypeWithMethod(string first, string second)
        {
            return ("No T" + ":" + first + ":" + second);
        }
        #endregion

        #region MethodWithGeneric
        public string TestGenericTypeWithMethodWithGeneric<O>()
        {
            return ("No T" + typeof(O).Name);

        }

        public string TestGenericTypeWithMethodWithGeneric<O>(string first)
        {
            return ("No T" + typeof(O).Name + ":" + first);
        }

        public string TestGenericTypeWithMethodWithGeneric<O>(string first, string second)
        {
            return ("No T" + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion
    }
}
