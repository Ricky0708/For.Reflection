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

        private void CreateInstance(Type type, string genericTypeName, Type[] argsType, params object[] args)
        {
            object instance;
            string compareResult = "";

            if (args != null)
            {
                foreach (var item in args)
                {
                    compareResult += item.ToString();
                }
            }

            instance = Processor.CreateInstance(type,
                argsType,
                args);
            if (instance == null || instance.GetType() != type)
            {
                Assert.Fail();
            }
            else
            {
                string reslut = (string)Processor.GetPropertyValue(instance, "T") +
                      (((int)Processor.GetPropertyValue(instance, "U")).ToString() == "0" ? "" : ((int)Processor.GetPropertyValue(instance, "U")).ToString());
                if (reslut != compareResult ||
                        (string)Processor.GetPropertyValue(instance, "GenericTypeName") != genericTypeName)
                {
                    Assert.Fail();
                }

            }
        }


        private void MethodCall(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            MethodInfo methodInfo;
            object instance = Processor.CreateInstance(type, null, null);
            string result;
            string resultCompare = "No T";
            methodInfo = Processor.MakeMethodInfo(type, methodName, genericsType, parametersType);
            result = Processor.MethodCall<string>(instance, methodInfo, args);
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
            MethodInfo methodInfo;
            object instance = Processor.CreateInstance(type, null, null);
            string result;
            string resultCompare = type.GetGenericArguments()[0].Name;
            methodInfo = Processor.MakeMethodInfo(type, methodName, genericsType, parametersType);
            result = Processor.MethodCall<string>(instance, methodInfo, args);
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
            MethodInfo methodInfo;
            object instance = Processor.CreateInstance(type, null, null);
            string result;
            string resultCompare = type.GetGenericArguments()[0].Name + genericsType[0].Name;
            methodInfo = Processor.MakeMethodInfo(type, methodName, genericsType, parametersType);
            result = Processor.MethodCall<string>(instance, methodInfo, args);
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
            MethodInfo methodInfo;
            object instance = Processor.CreateInstance(type, null, null);
            string result;
            string resultCompare = "No T" + genericsType[0].Name;
            methodInfo = Processor.MakeMethodInfo(type, methodName, genericsType, parametersType);
            result = Processor.MethodCall<string>(instance, methodInfo, args);
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
            MethodInfo methodInfo;
            object instance = Processor.CreateInstance(type, null, null);
            string resultCompare = "No generic";
            methodInfo = Processor.MakeMethodInfo(type, methodName, genericsType, parametersType);
            Processor.VoidCall(instance, methodInfo, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += (string)item;
                }
            }
            string reslut = (string)Processor.GetPropertyValue(instance, "GenericTypeName") + (string)Processor.GetFieldValue(instance, "I");

            if (reslut != resultCompare)
            {
                Assert.Fail();
            }

        }

        private void VoidCallWithGenericType(Type type, string methodName, Type[] genericsType, Type[] parametersType, params object[] args)
        {
            MethodInfo methodInfo;
            object instance = Processor.CreateInstance(type, null, null);
            string resultCompare = genericsType[0].Name;
            methodInfo = Processor.MakeMethodInfo(type, methodName, genericsType, parametersType);
            Processor.VoidCall(instance, methodInfo, args);
            if (args != null)
            {
                foreach (var item in args)
                {
                    resultCompare += (string)item;
                }
            }
            string reslut = (string)Processor.GetPropertyValue(instance, "GenericTypeName") + (string)Processor.GetFieldValue(instance, "I");

            if (reslut != resultCompare)
            {
                Assert.Fail();
            }

        }

    }
}
