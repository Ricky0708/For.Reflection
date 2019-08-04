using For.Reflection;
using Newtonsoft.Json;
using ReflectionTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a inject property value test");
            Console.WriteLine("from a class has 11 property, and 8 complex properties and 3 basic properties");
            Console.WriteLine("below is the test result...");
            Console.WriteLine("");
            Console.WriteLine("");

            JsonProfile();
            AssginByProfileCode();
            DelegateCallProfile();
            StandardCallProfileWithTypeConvert();
            TypeReflection();
            StandardCall();
            DelegateCall();
            DelegateCallB();

            Console.ReadLine();
        }

        static void JsonProfile()
        {
            var fromObj = new FromTestProfile()
            {
                Address = "AF",
                Age = "18",
                Balance = "2000",
                Birthday = DateTime.Today.ToString("yyyy/MM/dd"),
                Name = "Ricky",
                Sex = "1"
            };
            var json = JsonConvert.SerializeObject(fromObj);
            TimeSpan ts;
            Stopwatch stopWatch;

            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                var n = JsonConvert.DeserializeObject<FromTestProfile>(json);
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;


            Console.WriteLine("");
            Console.WriteLine("json profile 1000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }

        static void AssginByProfileCode()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            var processor = new TypeProcessor<ToTestProfile>();
            var processorFrom = new TypeProcessor<FromTestProfile>();
            var fromObj = new FromTestProfile()
            {
                Address = "AF",
                Age = "18",
                Balance = "2000",
                Birthday = DateTime.Today.ToString("yyyy/MM/dd"),
                Name = "Ricky",
                Sex = "1"
            };

            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                var ToObj = new ToTestProfile
                {
                    Address = fromObj.Address,
                    Age = long.Parse(fromObj.Age),
                    Balance = decimal.Parse(fromObj.Balance),
                    Birthday = DateTime.Parse(fromObj.Birthday),
                    Name = fromObj.Name,
                    Sex = int.Parse(fromObj.Sex)
                };
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;


            Console.WriteLine("");
            Console.WriteLine("AssginByCode profile 1000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }

        static void StandardCallProfileWithTypeConvert()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
   
            var fromObj = new FromTestProfile()
            {
                Address = "AF",
                Age = "18",
                Balance = "2000",
                Birthday = DateTime.Today.ToString("yyyy/MM/dd"),
                Name = "Ricky",
                Sex = "1"
            };

            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                var processorTo = new TypeProcessor<ToTestProfile>();
                var processorFrom = new TypeProcessor<FromTestProfile>();
                var ToObj = processorTo.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    processorTo.SetProperty(ToObj, item.Name, processorFrom.GetProperty(fromObj, item.Name));
                }
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;


            Console.WriteLine("");
            Console.WriteLine("stand call profile with convert 1000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");

        }

        static void DelegateCallProfile()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            var processor = new TypeProcessor<ToTestProfile>();
            var processorFrom = new TypeProcessor<FromTestProfile>();
            stopWatch = new Stopwatch();
            var fromObj = processorFrom.CreateInstance();
            processorFrom.SetProperty(fromObj, "Address", "Address");
            processorFrom.SetProperty(fromObj, "Age", "18");
            processorFrom.SetProperty(fromObj, "Balance", "2000");
            processorFrom.SetProperty(fromObj, "Birthday", DateTime.Now.ToString("yyyy/MM/dd"));
            processorFrom.SetProperty(fromObj, "Name", "Ricky");
            processorFrom.SetProperty(fromObj, "Sex", "2");

            stopWatch.Start();
            Type toType = typeof(ToTestProfile);
            Type fromType = typeof(FromTestProfile);
            Core.delgCreateInstance delgCreate = Core.GenCreateInstanceDelg(Core.MakeCtorInfo(typeof(ToTestProfile), null));
            List<Core.delgSetProperty> setters = new List<Core.delgSetProperty>();
            List<Core.delgGetProperty> getters = new List<Core.delgGetProperty>();
            PropertyInfo[] fromPropertyInfos = fromObj.GetType().GetProperties();
            PropertyInfo[] toPropertyInfos = typeof(ToTestProfile).GetProperties();
            for (int i = 0; i < fromPropertyInfos.Count(); i++)
            {
                setters.Add(Core.GenSetPropertyValueDelg(toType, toPropertyInfos[i]));
                getters.Add(Core.GenGetPropertyValueDelg(fromType, fromPropertyInfos[i]));
            }
            object toObj;
            for (int i = 0; i < 1000000; i++)
            {
                toObj = delgCreate();
                for (int n = 0; n < setters.Count(); n++)
                {
                    setters[n](toObj, getters[n](fromObj));
                }

            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("");
            Console.WriteLine("delegate call profile 10000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }

        static void StandardCall()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            var processor = new TypeProcessor<TestType>();
            var fromObj = processor.CreateInstance();

            //processor.SetProperty(instance, "T", "dd");
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


            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                var ToObj = processor.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    processor.SetProperty(ToObj, item.Name, processor.GetProperty(fromObj, item.Name));
                }
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;


            Console.WriteLine("");
            Console.WriteLine("stand call 1000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }

        static void TypeReflection()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            var processor = new TypeProcessor<ToTestProfile>();
            var fromObj = new ToTestProfile()
            {
                Address = "AF",
                Age = 18,
                Balance = 2000,
                Birthday = DateTime.Today,
                Name = "Ricky",
                Sex = 1
            };

            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                var ToObj = processor.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    item.SetValue(ToObj, item.GetValue(fromObj));
                    //processor.SetProperty(ToObj, item.Name, processorFrom.GetProperty(fromObj, item.Name));
                }
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;


            Console.WriteLine("");
            Console.WriteLine("type reflection profile 1000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }


        static void DelegateCall()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            var processor = new TypeProcessor<TestType>();
            stopWatch = new Stopwatch();
            object fromObj = processor.CreateInstance();
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

            stopWatch.Start();
            Type o = typeof(TestType);
            Core.delgCreateInstance delgCreate = Core.GenCreateInstanceDelg(Core.MakeCtorInfo(typeof(TestType), null));
            List<Core.delgSetProperty> setters = new List<Core.delgSetProperty>();
            List<Core.delgGetProperty> getters = new List<Core.delgGetProperty>();
            PropertyInfo[] propertyInfos = fromObj.GetType().GetProperties();
            for (int i = 0; i < propertyInfos.Count(); i++)
            {
                setters.Add(Core.GenSetPropertyValueDelg(o, propertyInfos[i]));
                getters.Add(Core.GenGetPropertyValueDelg(o, propertyInfos[i]));
            }
            object toObj;
            for (int i = 0; i < 1000000; i++)
            {
                toObj = delgCreate();
                for (int n = 0; n < setters.Count(); n++)
                {
                    setters[n](toObj, getters[n](fromObj));
                }

            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("");
            Console.WriteLine("delegate call 10000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }

        static void DelegateCallB()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            var processor = new TypeProcessor<TestType>();
            stopWatch = new Stopwatch();
            object fromObj = processor.CreateInstance();
            Core.delgCreateInstance delgCreate = Core.GenCreateInstanceDelg(Core.MakeCtorInfo(typeof(TestType), null));
            Type o = typeof(TestType);

            stopWatch = new Stopwatch();
            stopWatch.Start();
            PropertyInfo pp = fromObj.GetType().GetProperty("U");
            Core.delgGetProperty dd = Core.GenGetPropertyValueDelg(o, pp);
            Core.delgSetProperty xx = Core.GenSetPropertyValueDelg(o, pp);
            for (int i = 0; i < 1000000; i++)
            {
                fromObj = delgCreate();
                dd(fromObj);

            }

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("");
            Console.WriteLine("delegate call 10000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");
        }

    }
}
