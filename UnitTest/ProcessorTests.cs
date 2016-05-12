using Microsoft.VisualStudio.TestTools.UnitTesting;
using For.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Test;
using System.Reflection;

namespace UnitTest.Test
{
    [TestClass()]
    public partial class ProcessorTests
    {

        [TestMethod()]
        public void MakeTypeTest()
        {
            Type type;

            //test type without generic type 
            type = Processor.MakeType(typeof(TestType), null);
            if (type != typeof(TestType)) Assert.Fail();
            type = Processor.MakeType("UnitTest.Test.TestType", "UnitTest", null);
            if (type != typeof(TestType)) Assert.Fail();

            //test static type without generic type 
            type = Processor.MakeType(typeof(TestStaticType), null);
            if (type != typeof(TestStaticType)) Assert.Fail();
            type = Processor.MakeType("UnitTest.Test.TestStaticType", "UnitTest", null);
            if (type != typeof(TestStaticType)) Assert.Fail();

            //test type with generic type 
            type = Processor.MakeType(typeof(TestGenericType<>), typeof(TestType));
            if (type != typeof(TestGenericType<TestType>)) Assert.Fail();
            type = Processor.MakeType("UnitTest.Test.TestGenericType`1", "UnitTest", typeof(TestType));
            if (type != typeof(TestGenericType<TestType>)) Assert.Fail();

            //test static type with generic type 
            type = Processor.MakeType(typeof(TestGenericStaticType<>), typeof(TestType));
            if (type != typeof(TestGenericStaticType<TestType>)) Assert.Fail();
            type = Processor.MakeType("UnitTest.Test.TestGenericStaticType`1", "UnitTest", typeof(TestType));
            if (type != typeof(TestGenericStaticType<TestType>)) Assert.Fail();
        }

        [TestMethod()]
        public void CreateInstanceTest()
        {
            Type type;

            type = Processor.MakeType(typeof(TestType), null);
            CreateInstanceWithoutGenericType(type);

            type = Processor.MakeType(typeof(TestGenericType<>), new[] { typeof(TestType) });
            CreateInstanceWithGenericType(type, typeof(TestType).Name);
        }

        [TestMethod()]
        public void MethodCallTest()
        {
            Type type;
            MethodInfo methodInfo;
            string result;
            type = Processor.MakeType(typeof(TestType), null);
            object instance = Processor.CreateInstance(typeof(TestType), null, null);



            methodInfo = Processor.MakeMethodInfo(type, "TestMethod", null, null);
            result = Processor.MethodCall<string>(instance, methodInfo, null);
            if (result != "No T") Assert.Fail();

            methodInfo = Processor.MakeMethodInfo(type, "TestMethod", null, new[] { typeof(string) });
            result = Processor.MethodCall<string>(instance, methodInfo, new object[] { "AA" });
            if (result != "No T" + ":" + "AA") Assert.Fail();

            methodInfo = Processor.MakeMethodInfo(type, "TestMethod", null, new[] { typeof(string), typeof(string) });
            result = Processor.MethodCall<string>(instance, methodInfo, new object[] { "AA", "BB" });
            if (result != "No T" + ":" + "AA" + ":" + "BB") Assert.Fail();
            //-----------------------------------------//
            methodInfo = Processor.MakeMethodInfo(type, "TestMethod", new[] { typeof(string) }, null);
            result = Processor.MethodCall<string>(instance, methodInfo, null);
            if (result != "No T" + typeof(string).Name) Assert.Fail();

            methodInfo = Processor.MakeMethodInfo(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string) });
            result = Processor.MethodCall<string>(instance, methodInfo, new[] { "AA" });
            if (result != "No T" + typeof(string).Name + ":" + "AA") Assert.Fail();

            methodInfo = Processor.MakeMethodInfo(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string), typeof(string) });
            result = Processor.MethodCall<string>(instance, methodInfo, new[] { "AA", "BB" });
            if (result != "No T" + typeof(string).Name + ":" + "AA" + ":" + "BB") Assert.Fail();
            //----------------------------------------//

        }

        [TestMethod()]
        public void VoidCallTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetFieldValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetFieldValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetPropertyValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPropertyValueTest()
        {
            Assert.Fail();
        }
    }
}