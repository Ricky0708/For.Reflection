using For.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        delegate object ObjectActivator();
        private void button1_Click(object sender, EventArgs e)
        {
            ////Normal type

            //Type o;
            //object obj;
            //int q = 0;
            //for (int i = 0; i < 1000000; i++)
            //{
            //    //Activator.CreateInstance<TestType>();
            //    o = ReflectionHelper.MakeType(typeof(TestType));
            //    obj = ReflectionHelper.CreateInstance(o, null);
            //    //obj = oo.DynamicInvoke();
            //}



            //// Test Generic ctor 
            //o = ReflectionHelper.MakeType(typeof(TestGenericType<>).FullName, Assembly.GetEntryAssembly().FullName, typeof(string));
            //obj = ReflectionHelper.CreateInstance(o, null, null);
            //obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string) }, new[] { "A" });
            //obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string), typeof(string) }, new[] { null, "B" });

            //// Test ctor
            //o = ReflectionHelper.MakeType(typeof(TestType));
            //obj = ReflectionHelper.CreateInstance(o, null, null);
            //obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string) }, new[] { "A" });
            //obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string), typeof(string) }, new[] { "A", "B" });

            // Test Methd


        }

        private void button2_Click(object sender, EventArgs e)
        {
            ////static type

            //Type o;
            //object obj;

            //// Test Generic Type Methd
            //o = ReflectionHelper.MakeType(typeof(TestGenericStaticType<>), typeof(string));
            //obj = ReflectionHelper.MakeMethodInfo(o, "TestGenericTypeWithMethod", null, null);
            //MessageBox.Show((string)ReflectionHelper.MethodCall(null, (MethodInfo)obj, null));

            ////o = ReflectionHelper.MakeType(typeof(TestGenericStaticType<>), typeof(string));
            //obj = ReflectionHelper.MakeMethodInfo(o, "TestGenericTypeWithMethod", null, new[] { typeof(string), typeof(string) });
            //MessageBox.Show((string)ReflectionHelper.MethodCall(null, (MethodInfo)obj, new[] { "ddd","fff" }));

            //o = ReflectionHelper.MakeType(typeof(TestStaticType));
            //for (int i = 0; i < 1000000; i++)
            //{
            //    obj = ReflectionHelper.MakeMethodInfo(o, "QQ", null, new[] { typeof(string) });
            //    ReflectionHelper.VoidCall(null, (MethodInfo)obj, new[] { "ff" });
            //}

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // SetField

            Type o;
            object obj;
            o = Processor.MakeType(typeof(TestType));
            obj = Processor.CreateInstance(o, null, null);
            for (int i = 0; i < 50000; i++)
            {
                o = Processor.MakeType(typeof(TestType));
                obj = Processor.CreateInstance(o, null, null);
                Processor.SetFieldValue(obj, "A", new TestType());
                Processor.SetFieldValue(obj, "B", new TestType());
                Processor.SetFieldValue(obj, "C", new TestType());
                Processor.SetFieldValue(obj, "D", new TestType());
                Processor.SetFieldValue(obj, "E", new TestType());
                Processor.SetFieldValue(obj, "F", new TestType());
                Processor.SetFieldValue(obj, "G", new TestType());
                Processor.SetFieldValue(obj, "H", new TestType());
                Processor.SetFieldValue(obj, "I", "I");
                Processor.SetFieldValue(obj, "J", 99);
                Processor.SetFieldValue(obj, "K", true);
            }

            for (int i = 0; i < 50000; i++)
            {
                o = Processor.MakeType(typeof(TestType));
                obj = Processor.CreateInstance(o, null, null);
                Processor.SetPropertyValue(obj, "L", new TestType());
                Processor.SetPropertyValue(obj, "M", new TestType());
                Processor.SetPropertyValue(obj, "N", new TestType());
                Processor.SetPropertyValue(obj, "O", new TestType());
                Processor.SetPropertyValue(obj, "P", new TestType());
                Processor.SetPropertyValue(obj, "Q", new TestType());
                Processor.SetPropertyValue(obj, "R", new TestType());
                Processor.SetPropertyValue(obj, "S", new TestType());
                Processor.SetPropertyValue(obj, "T", "I");
                Processor.SetPropertyValue(obj, "U", 99);
                Processor.SetPropertyValue(obj, "V", true);
            }

            for (int i = 0; i < 1000000; i++)
            {
                Processor.GetPropertyValue(obj, "V");

            }
        }

        private void button4_Click(object sender, EventArgs e)
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

            for (int i = 0; i < 50000; i++)
            {
                var ToObj = Processor.CreateInstance(o, null, null);
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    Processor.SetPropertyValue(ToObj, item.Name, Processor.GetPropertyValue(fromObj, item.Name));
                }
            }



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
        }
    }
}
