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
            //Normal type

            Type o;
            object obj;
            int q = 0;
            for (int i = 0; i < 1000000; i++)
            {
                //Activator.CreateInstance<TestType>();
                o = ReflectionHelper.MakeType(typeof(TestType));
                obj = ReflectionHelper.CreateInstance(o, null);
                //obj = oo.DynamicInvoke();
            }



            // Test Generic ctor 
            o = ReflectionHelper.MakeType(typeof(TestGenericType<>).FullName, Assembly.GetEntryAssembly().FullName, typeof(string));
            obj = ReflectionHelper.CreateInstance(o, null, null);
            obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string) }, new[] { "A" });
            obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string), typeof(string) }, new[] { null, "B" });

            // Test ctor
            o = ReflectionHelper.MakeType(typeof(TestType));
            obj = ReflectionHelper.CreateInstance(o, null, null);
            obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string) }, new[] { "A" });
            obj = ReflectionHelper.CreateInstance(o, new[] { typeof(string), typeof(string) }, new[] { "A", "B" });

            // Test Methd


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //static type

            Type o;
            object obj;

            // Test Generic Type Methd
            o = ReflectionHelper.MakeType(typeof(TestGenericStaticType<>), typeof(string));
            obj = ReflectionHelper.MakeMethodInfo(o, "TestGenericTypeWithMethod", null, null);
            MessageBox.Show((string)ReflectionHelper.MethodCall(null, (MethodInfo)obj, null));

            //o = ReflectionHelper.MakeType(typeof(TestGenericStaticType<>), typeof(string));
            obj = ReflectionHelper.MakeMethodInfo(o, "TestGenericTypeWithMethod", null, new[] { typeof(string), typeof(string) });
            MessageBox.Show((string)ReflectionHelper.MethodCall(null, (MethodInfo)obj, new[] { "ddd","fff" }));

            o = ReflectionHelper.MakeType(typeof(TestStaticType));
            for (int i = 0; i < 1000000; i++)
            {
                obj = ReflectionHelper.MakeMethodInfo(o, "QQ", null, new[] { typeof(string) });
                ReflectionHelper.VoidCall(null, (MethodInfo)obj, new[] { "ff" });
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // SetField

            Type o;
            object obj;
            o = ReflectionHelper.MakeType(typeof(TestType));
            obj = ReflectionHelper.CreateInstance(o, null, null);
            for (int i = 0; i < 50000; i++)
            {
                o = ReflectionHelper.MakeType(typeof(TestType));
                obj = ReflectionHelper.CreateInstance(o, null, null);
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
                ReflectionHelper.SetFieldValue(obj, "pp", new TestType());
            }

            for (int i = 0; i < 1000000; i++)
            {
                ReflectionHelper.GetFieldValue(obj, "pp");

            }
        }
    }
}
