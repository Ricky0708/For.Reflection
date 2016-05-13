﻿using For.Reflection;
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
            //Core.delgMethodCall methodCallA;
            //Core.delgMethodCall methodCallB;
            //var methodInfo = Core.MakeMethodInfo(typeof(TestType), "TestGenericTypeWithMethod", null, null);
            //object instance;
            //object instance2;
            //instance = Processor.CreateInstance(typeof(TestType), null, null);
            //instance2 = Processor.CreateInstance(typeof(TestType), null, null);
            //Processor.SetPropertyValue(instance, "T", "dd");


            //methodCallA = Core.GenMethodCallDelg(instance, methodInfo);
            //methodCallA(null);
            //methodCallB = Core.GenMethodCallDelg(instance2, methodInfo);
            //methodCallB(null);

            TimeSpan ts;
            Stopwatch stopWatch;
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


            stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 50000; i++)
            {
                var ToObj = Processor.CreateInstance(o, null, null);
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    Processor.SetPropertyValue(ToObj, item.Name, Processor.GetPropertyValue(fromObj, item.Name));
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
            Core.delgCreateInstance delgCreate = Core.GenCreateInstanceDelg(typeof(TestType), null);
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
            Console.ReadLine();

        }
    }
}