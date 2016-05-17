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
        public void CreateInstanceTest()
        {
            Type type = typeof(TestType);
            CreateInstance(type, null, null, null, null);
            CreateInstance(type, null, null, new[] { typeof(string) }, "AA");
            CreateInstance(type, null, null, new[] { typeof(string), typeof(int) }, "AA", 99);

            type = typeof(TestGenericType<>);
            CreateInstance(type, new[] { typeof(TestType) }, typeof(TestType).Name, null, null);
            CreateInstance(type, new[] { typeof(TestType) }, typeof(TestType).Name, new[] { typeof(string) }, "AA");
            CreateInstance(type, new[] { typeof(TestType) }, typeof(TestType).Name, new[] { typeof(string), typeof(int) }, "AA", 99);
        }

        [TestMethod()]
        public void MethodCallTest()
        {
            Type type = typeof(TestType);


            MethodCall(type, "TestMethod", null, null, null);
            MethodCall(type, "TestMethod", null, new[] { typeof(string) }, "AA");
            MethodCall(type, "TestMethod", null, new[] { typeof(string), typeof(string) }, "AA", "BB");

            MethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, null, null);
            MethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string) }, "AA");
            MethodCallWithGenericType(type, "TestMethod", new[] { typeof(string) }, new[] { typeof(string), typeof(string) }, "AA", "BB");

            //---------------------------------------------
            type = typeof(TestGenericType<string>);
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
            Type type = typeof(TestType);

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
            TypeProcessor processor = new TypeProcessor(typeof(TestType));
            fromObj = processor.CreateInstance();
            processor.SetField(fromObj, "A", new TestType());
            processor.SetField(fromObj, "B", new TestType());
            processor.SetField(fromObj, "C", new TestType());
            processor.SetField(fromObj, "D", new TestType());
            processor.SetField(fromObj, "E", new TestType());
            processor.SetField(fromObj, "F", new TestType());
            processor.SetField(fromObj, "G", new TestType());
            processor.SetField(fromObj, "H", new TestType());
            processor.SetField(fromObj, "I", "I");
            processor.SetField(fromObj, "J", 99);
            processor.SetField(fromObj, "K", true);


            for (int i = 0; i < 10; i++)
            {
                var ToObj = processor.CreateInstance();
                foreach (FieldInfo item in fromObj.GetType().GetFields())
                {
                    processor.SetField(ToObj, item.Name, processor.GetField(fromObj, item.Name));
                    if (!processor.GetField(fromObj, item.Name).Equals(processor.GetField(ToObj, item.Name)))
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
            TypeProcessor processor = new TypeProcessor(typeof(TestType));
            fromObj = processor.CreateInstance();
            processor.SetProperty(fromObj, "L", new TestType());
            processor.SetProperty(fromObj, "M", new TestType());
            processor.SetProperty(fromObj, "N", new TestType());
            processor.SetProperty(fromObj, "O", new TestType());
            processor.SetProperty(fromObj, "P", new TestType());
            processor.SetProperty(fromObj, "Q", new TestType());
            processor.SetProperty(fromObj, "R", new TestType());
            processor.SetProperty(fromObj, "S", new TestType());
            processor.SetProperty(fromObj, "T", "I");
            processor.SetProperty(fromObj, "U", 99);
            processor.SetProperty(fromObj, "V", true);
            processor.SetProperty(fromObj, "GenericTypeName", "A");


            for (int i = 0; i < 10; i++)
            {
                var ToObj = processor.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    processor.SetProperty(ToObj, item.Name, processor.GetProperty(fromObj, item.Name));
                    if (!processor.GetProperty(fromObj, item.Name).Equals(processor.GetProperty(ToObj, item.Name)))
                    {
                        Assert.Fail();
                    }
                }
            }
        }


    }
}