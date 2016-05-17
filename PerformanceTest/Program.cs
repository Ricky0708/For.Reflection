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
            TimeSpan ts;
            Stopwatch stopWatch;

            TypeProcessor processor = new TypeProcessor(typeof(TestType));
            object instance;
            instance = processor.CreateInstance();
            processor.SetProperty(instance, "T", "dd");

            for (int i = 0; i < 50000; i++)
            {
                instance = processor.CreateInstance();
            }


  

            object fromObj;
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


            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 50000; i++)
            {
                //Processor.GetPropertyValue(fromObj, "L");
                var ToObj = processor.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    processor.SetProperty(ToObj, item.Name, processor.GetProperty(fromObj, item.Name));
                }
            }
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("This is a inject property value test");
            Console.WriteLine("from a class has 11 property, and 8 complex properties and 3 basic properties");
            Console.WriteLine("below is the test result...");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("stand call 50000 times");
            Console.WriteLine("RunTime: " + ts.ToString() + "ms");

            stopWatch = new Stopwatch();
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
            Console.ReadLine();
        }
    }
}
