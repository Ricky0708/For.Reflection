using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Test
{
    public class TestGenericType<GenericT> : BasicTestType, InterfaceTestType
    {

        #region Ctor
        public TestGenericType()
        {
            GenericTypeName = typeof(GenericT).Name;
        }

        public TestGenericType(string first)
        {
            GenericTypeName = typeof(GenericT).Name;
            T = first;
        }

        public TestGenericType(string first, int second)
        {
            GenericTypeName = typeof(GenericT).Name;
            T = first;
            U = second;
        }
        #endregion

        #region Method
        public string TestMethod()
        {
            return (typeof(GenericT).Name);

        }

        public string TestMethod(string first)
        {
            return (typeof(GenericT).Name + ":" + first);
        }

        public string TestMethod(string first, string second)
        {
            return (typeof(GenericT).Name + ":" + first + ":" + second);
        }
        #endregion

        #region MethodWithGeneric
        public string TestMethod<O>()
        {
            return (typeof(GenericT).Name + typeof(O).Name);

        }

        public string TestMethod<O>(string first)
        {
            return (typeof(GenericT).Name + typeof(O).Name + ":" + first);
        }

        public string TestMethod<O>(string first, string second)
        {
            return (typeof(GenericT).Name + typeof(O).Name + ":" + first + ":" + second);
        }
        #endregion

        #region void
        public void TestVoid()
        {
            GenericTypeName = typeof(GenericT).Name;
            I = "Not params";
        }

        public void TestVoid(string first)
        {
            GenericTypeName = typeof(GenericT).Name;
            I = first;
        }

        public void TestVoid(string first, string second)
        {
            GenericTypeName = typeof(GenericT).Name;
            I = first + second;
        }

        public void TestVoid<O>()
        {
            GenericTypeName = typeof(GenericT).Name + typeof(O).Name;
        }

        public void TestVoid<O>(string first)
        {
            GenericTypeName = typeof(GenericT).Name + typeof(O).Name;
            I = first;
        }

        public void TestVoid<O>(string first, string second)
        {
            GenericTypeName = typeof(GenericT).Name + typeof(O).Name;
            I = first + second;
        }
        #endregion
    }
}
