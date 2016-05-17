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
    public partial class ProcessorTests
    {

        private void CreateInstance(Type type, Type[] genericTypes, string genericTypeName, Type[] argsType, params object[] args)
        {
            object instance;
            string compareResult = "";
            TypeProcessor processor = new TypeProcessor(type, genericTypes, argsType);
            if (args != null)
            {
                foreach (var item in args)
                {
                    compareResult += item.ToString();
                }
            }

            instance = processor.CreateInstance(args);
            if (instance == null || instance.GetType().Name != type.Name)
            {
                Assert.Fail();
            }
            else
            {
                string reslut = (string)processor.GetProperty(instance, "T") +
                      (((int)processor.GetProperty(instance, "U")).ToString() == "0" ? "" : ((int)processor.GetProperty(instance, "U")).ToString());
                if (reslut != compareResult ||
                        (string)processor.GetProperty(instance, "GenericTypeName") != genericTypeName)
                {
                    Assert.Fail();
                }

            }
        }


        private void MethodCall(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            TypeProcessor processor = new TypeProcessor(type, null, null);
            object instance = processor.CreateInstance(null);
            string result;
            string resultCompare = "No T";
            result = (string)processor.MethodCall(instance, methodName, genericsType, parametersType, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += ":" + (string)item;
                }
            }
            if (result != resultCompare) Assert.Fail();

        }

        private void GenericMethodCall(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            TypeProcessor processor = new TypeProcessor(type, null, null);
            object instance = processor.CreateInstance();
            string result;
            string resultCompare = type.GetGenericArguments()[0].Name;
            result = (string)processor.MethodCall(instance, methodName, genericsType, parametersType, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += ":" + (string)item;
                }
            }
            if (result != resultCompare) Assert.Fail();

        }


        private void GenericMethodCallWithGenericType(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            TypeProcessor processor = new TypeProcessor(type, null, null);
            object instance = processor.CreateInstance();
            string result;
            string resultCompare = type.GetGenericArguments()[0].Name + genericsType[0].Name;
            result = (string)processor.MethodCall(instance, methodName, genericsType, parametersType, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += ":" + (string)item;
                }
            }
            if (result != resultCompare) Assert.Fail();

        }


        private void MethodCallWithGenericType(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            TypeProcessor processor = new TypeProcessor(type, null, null);
            object instance = processor.CreateInstance();
            string result;
            string resultCompare = "No T" + genericsType[0].Name;
            result = (string)processor.MethodCall(instance, methodName, genericsType, parametersType, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += ":" + (string)item;
                }
            }
            if (result != resultCompare) Assert.Fail();

        }

        private void VoidCall(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            TypeProcessor processor = new TypeProcessor(type, null, null);
            object instance = processor.CreateInstance();
            string resultCompare = "No generic";
            processor.VoidCall(instance, methodName, genericsType, parametersType, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += (string)item;
                }
            }
            string reslut = (string)processor.GetProperty(instance, "GenericTypeName") + (string)processor.GetField(instance, "I");

            if (reslut != resultCompare)
            {
                Assert.Fail();
            }

        }

        private void VoidCallWithGenericType(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            TypeProcessor processor = new TypeProcessor(type, null, null);
            object instance = processor.CreateInstance();
            string resultCompare = genericsType[0].Name;
            processor.VoidCall(instance, methodName, genericsType, parametersType, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += (string)item;
                }
            }
            string reslut = (string)processor.GetProperty(instance, "GenericTypeName") + (string)processor.GetField(instance, "I");

            if (reslut != resultCompare)
            {
                Assert.Fail();
            }

        }

    }
}
