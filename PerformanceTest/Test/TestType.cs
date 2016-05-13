using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionTest
{
    public class TestType
    {

        public TestType A;
        public TestType B;
        public TestType C;
        public TestType D;
        public TestType E;
        public TestType F;
        public TestType G;
        public TestType H;
        public string I;
        public int J;
        public bool K;



        public TestType L { get; set; }
        public TestType M { get; set; }
        public TestType N { get; set; }
        public TestType O { get; set; }
        public TestType P { get; set; }
        public TestType Q { get; set; }
        public TestType R { get; set; }
        public TestType S { get; set; }
        public string T { get; set; }
        public int U { get; set; }
        public bool V { get; set; }

        #region Ctor
        public TestType()
        {
            //MessageBox.Show("No T");
        }

        public TestType(string first)
        {
        }

        public TestType(string first, string second)
        {
        }
        #endregion

        public string aa()
        {
            return "aa";
        }

        #region Method
        public string TestGenericTypeWithMethod()
        {
            return ("No T" + T);

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
