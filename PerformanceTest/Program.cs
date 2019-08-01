using For.Reflection;
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

            StandardCallProfileWithTypeConvert();
            TypeReflection();
            StandardCall();
            DelegateCall();
            DelegateCallB();

            Console.ReadLine();
        }
        static void StandardCallProfileWithTypeConvert()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            TypeProcessor processor = new TypeProcessor(typeof(ToTestProfile));
            TypeProcessor processorFrom = new TypeProcessor(typeof(FromTestProfile));
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
                var ToObj = processor.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    processor.SetProperty(ToObj, item.Name, processorFrom.GetProperty(fromObj, item.Name));
                }
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;


            Console.WriteLine("");
            Console.WriteLine("stand call profile 1000000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");

        }
        static void StandardCall()
        {
            TimeSpan ts;
            Stopwatch stopWatch;
            TypeProcessor processor = new TypeProcessor(typeof(TestType));
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
            TypeProcessor processor = new TypeProcessor(typeof(ToTestProfile));
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
            TypeProcessor processor = new TypeProcessor(typeof(TestType));
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
            TypeProcessor processor = new TypeProcessor(typeof(TestType));
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
