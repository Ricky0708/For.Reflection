using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Test
{
    public class TestType : BasicTestType, InterfaceTestType
    {


        #region Ctor
        public TestType()
        {
        }

        public TestType(string first)
        {
            T = first;
        }

        public TestType(string first, int second)
        {
            T = first;
            U = second;
        }
        #endregion

        #region Method
        public string TestMethod()
        {
            return ("No T");

        }

        public string TestMethod(string first)
        {
            return ("No T" + ":" + first);
        }

        public string TestMethod(string first, string second)
        {
            return ("No T" + ":" + first + ":" + second);
        }
        #endregion

        #region MethodWithGeneric
        public string TestMethod<O>()
        {
            return ("No T" + typeof(O).Name);

        }

        public string TestMethod<O>(string first)
        {
            return ("No T" + typeof(O).Name + ":" + first);
        }

        public string TestMethod<O>(string first, string second)
        {
            return ("No T" + typeof(O).Name + ":" + first + ":" + second);
        }


        #endregion

        #region void
        public void TestVoid()
        {
            GenericTypeName = "No generic";
            I = "Not params";
        }

        public void TestVoid(string first)
        {
            I = first;
        }

        public void TestVoid(string first, string second)
        {
            I = first + second;
        }

        public void TestVoid<O>()
        {
            GenericTypeName = typeof(O).Name;
        }

        public void TestVoid<O>(string first)
        {
            GenericTypeName = typeof(O).Name;
            I = first;
        }

        public void TestVoid<O>(string first, string second)
        {
            GenericTypeName = typeof(O).Name;
            I = first + second;
        }
        #endregion
    }
}
