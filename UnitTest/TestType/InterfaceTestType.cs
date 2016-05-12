using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Test
{
    interface InterfaceTestType
    {
        #region Method
        string TestMethod();

        string TestMethod(string first);

        string TestMethod(string first, string second);
        #endregion

        #region MethodWithGeneric
        string TestMethod<O>();

        string TestMethod<O>(string first);

        string TestMethod<O>(string first, string second);
        #endregion


        #region Void
        void TestVoid();

        void TestVoid(string first);

        void TestVoid(string first, string second);
        #endregion

        #region VoidWithGeneric
        void TestVoid<O>();

        void TestVoid<O>(string first);

        void TestVoid<O>(string first, string second);
        #endregion
    }
}
