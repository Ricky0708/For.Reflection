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

            //test static type without generic type 
            type = Processor.MakeType(typeof(TestStaticType), null);
            if (type != typeof(TestStaticType)) Assert.Fail();

            //test type with generic type 
            type = Processor.MakeType(typeof(TestGenericType<>), typeof(TestType));
            if (type != typeof(TestGenericType<TestType>)) Assert.Fail();

            //test static type with generic type 
            type = Processor.MakeType(typeof(TestGenericStaticType<>), typeof(TestType));
            if (type != typeof(TestGenericStaticType<TestType>)) Assert.Fail();
        }

        [TestMethod()]
        public void CreateInstanceTest()
        {
            Type type;

            type = Processor.MakeType(typeof(TestType), null);
            CreateInstance(type, null, null, null);
            CreateInstance(type, null, new[] { typeof(string) }, "AA");
            CreateInstance(type, null, new[] { typeof(string), typeof(int) }, "AA", 99);


            type = Processor.MakeType(typeof(TestGenericType<>), new[] { typeof(TestType) });
            CreateInstance(type, typeof(TestType).Name, null, null);
            CreateInstance(type, typeof(TestType).Name, new[] { typeof(string) }, "AA");
            CreateInstance(type, typeof(TestType).Name, new[] { typeof(string), typeof(int) }, "AA", 99);
        }

        [TestMethod()]
        public void MethodCallTest()
        {
            Type type;
            type = Processor.MakeType(typeof(TestType), null);

            MethodCall(type, "TestMethod", null, null, null);
            MethodCall(type, "TestMethod", null, new[] { typeof(string) }, "AA");
            MethodCall(type, "TestMethod", null, new[] { typeof(string), typeof(string) }, "AA", "BB");

            MethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, null, null);
            MethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string) }, "AA");
            MethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string), typeof(string) }, "AA", "BB");

            //---------------------------------------------
            type = Processor.MakeType(typeof(TestGenericType<>), typeof(string));
            GenericMethodCall(type, "TestMethod", null, null, null);
            GenericMethodCall(type, "TestMethod", null, new[] { typeof(string) }, "AA");
            GenericMethodCall(type, "TestMethod", null, new[] { typeof(string), typeof(string) }, "AA", "BB");

            GenericMethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, null, null);
            GenericMethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string) }, "AA");
            GenericMethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string), typeof(string) }, "AA", "BB");
        }

        [TestMethod()]
        public void VoidCallTest()
        {
            Type type;
            type = Processor.MakeType(typeof(TestType), null);

            VoidCall(type, "TestVoid", null, null, null);
            VoidCall(type, "TestVoid", null, new[] { typeof(string) }, "AA");
            VoidCall(type, "TestVoid", null, new[] { typeof(string), typeof(string) }, "AA", "BB");

            VoidCallWithGenericType(type, "TestVoid", new[] { typeof(string) }, null);
            VoidCallWithGenericType(type, "TestVoid", new[] { typeof(string) }, new[] { typeof(string) }, "AA");
            VoidCallWithGenericType(type, "TestVoid", new[] { typeof(string) }, new[] { typeof(string), typeof(string) }, "AA", "BB");
        }

        [TestMethod()]
        public void FieldValueTest()
        {
            Type o;
            object fromObj;
            o = Processor.MakeType(typeof(TestType));
            fromObj = Processor.CreateInstance(o, null, null);
            Processor.SetFieldValue(fromObj, "A", new TestType());
            Processor.SetFieldValue(fromObj, "B", new TestType());
            Processor.SetFieldValue(fromObj, "C", new TestType());
            Processor.SetFieldValue(fromObj, "D", new TestType());
            Processor.SetFieldValue(fromObj, "E", new TestType());
            Processor.SetFieldValue(fromObj, "F", new TestType());
            Processor.SetFieldValue(fromObj, "G", new TestType());
            Processor.SetFieldValue(fromObj, "H", new TestType());
            Processor.SetFieldValue(fromObj, "I", "I");
            Processor.SetFieldValue(fromObj, "J", 99);
            Processor.SetFieldValue(fromObj, "K", true);


            for (int i = 0; i < 10; i++)
            {
                var ToObj = Processor.CreateInstance(o, null, null);
                foreach (FieldInfo item in fromObj.GetType().GetFields())
                {
                    Processor.SetFieldValue(ToObj, item.Name, Processor.GetFieldValue(fromObj, item.Name));
                    if (!Processor.GetFieldValue(fromObj, item.Name).Equals(Processor.GetFieldValue(ToObj, item.Name)))
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod()]
        public void PropertyValueTest()
        {
            Type o;
            object fromObj;
            o = Processor.MakeType(typeof(TestType));
            fromObj = Processor.CreateInstance(o, null, null);
            Processor.SetPropertyValue(fromObj, "L", new TestType());
            Processor.SetPropertyValue(fromObj, "M", new TestType());
            Processor.SetPropertyValue(fromObj, "N", new TestType());
            Processor.SetPropertyValue(fromObj, "O", new TestType());
            Processor.SetPropertyValue(fromObj, "P", new TestType());
            Processor.SetPropertyValue(fromObj, "Q", new TestType());
            Processor.SetPropertyValue(fromObj, "R", new TestType());
            Processor.SetPropertyValue(fromObj, "S", new TestType());
            Processor.SetPropertyValue(fromObj, "T", "I");
            Processor.SetPropertyValue(fromObj, "U", 99);
            Processor.SetPropertyValue(fromObj, "V", true);
            Processor.SetPropertyValue(fromObj, "GenericTypeName", "A");


            for (int i = 0; i < 10; i++)
            {
                var ToObj = Processor.CreateInstance(o, null, null);
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    Processor.SetPropertyValue(ToObj, item.Name, Processor.GetPropertyValue(fromObj, item.Name));
                    if (!Processor.GetPropertyValue(fromObj, item.Name).Equals(Processor.GetPropertyValue(ToObj, item.Name)))
                    {
                        Assert.Fail();
                    }
                }
            }
        }

   
    }
}