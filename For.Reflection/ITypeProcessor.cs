using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    internal interface ITypeProcessor
    {
        object MethodCall(object instance, string methodName, Type[] genericsType, Type[] argsType, object[] args);
        void VoidCall(object instance, string voidName, Type[] genericsType, Type[] argsType, object[] args);

        object GetProperty(object instance, PropertyInfo prop);
        object GetProperty(object instance, string propName);
        void SetProperty(object instance, PropertyInfo prop, object value);
        void SetProperty(object instance, string propName, object value);

        object GetField(object instance, FieldInfo field);
        object GetField(object instance, string fieldName);
        void SetField(object instance, FieldInfo field, object value);
        void SetField(object instance, string fieldName, object value);
    }

    interface ITypeProcessor<T> : ITypeProcessor
    {

        T CreateInstance(object[] args = null);

    }
}
