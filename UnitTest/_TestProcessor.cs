using Microsoft.VisualStudio.TestTools.UnitTesting;
using For.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Test;


namespace UnitTest.Test
{
    public partial class ProcessorTests
    {
        private void CreateInstanceWithoutGenericType(Type type)
        {
            object instance;
            instance = Processor.CreateInstance(type, null, null);
            if (instance == null || instance.GetType() != type) Assert.Fail();


            instance = Processor.CreateInstance(type,
             new[] { typeof(string) },
             new object[] { "AA" });
            if (instance == null || instance.GetType() != type)
            {
                Assert.Fail();
            }
            else
            {
                if ((string)Processor.GetPropertyValue(instance, "T") != "AA")
                {
                    Assert.Fail();
                }

            }

            instance = Processor.CreateInstance(type,
                new[] { typeof(string), typeof(int) },
                new object[] { "AA", 99 });
            if (instance == null || instance.GetType() != type)
            {
                Assert.Fail();
            }
            else
            {
                if ((string)Processor.GetPropertyValue(instance, "T") != "AA" ||
                    (int)Processor.GetPropertyValue(instance, "U") != 99)
                {
                    Assert.Fail();
                }

            }
        }

        private void CreateInstanceWithGenericType(Type type, string genericTypeName)
        {
            object instance;
            instance = Processor.CreateInstance(type, null, null);
            if (instance == null || instance.GetType() != type)
            {
                Assert.Fail();
            }
            else
            {
                if ((string)Processor.GetPropertyValue(instance, "GenericTypeName") != genericTypeName)
                {
                    Assert.Fail();
                }
            }


            instance = Processor.CreateInstance(type,
             new[] { typeof(string) },
             new object[] { "AA" });
            if (instance == null || instance.GetType() != type)
            {
                Assert.Fail();
            }
            else
            {
                if ((string)Processor.GetPropertyValue(instance, "T") != "AA" ||
                    (string)Processor.GetPropertyValue(instance, "GenericTypeName") != genericTypeName)
                {
                    Assert.Fail();
                }

            }

            instance = Processor.CreateInstance(type,
                new[] { typeof(string), typeof(int) },
                new object[] { "AA", 99 });
            if (instance == null || instance.GetType() != type)
            {
                Assert.Fail();
            }
            else
            {
                if ((string)Processor.GetPropertyValue(instance, "T") != "AA" ||
                        (int)Processor.GetPropertyValue(instance, "U") != 99 ||
                        (string)Processor.GetPropertyValue(instance, "GenericTypeName") != genericTypeName)
                {
                    Assert.Fail();
                }

            }
        }
    }
}
