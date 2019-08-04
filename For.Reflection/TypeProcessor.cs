using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    public class TypeProcessor<T> : ITypeProcessor<T>
    {
        #region var
        private Type instanceType;
        private ConstructorInfo ctorInfo;
        private Core.delgCreateInstance delgCreate;
        private Hashtable o = new Hashtable();
        private Dictionary<string, Core.delgGetProperty> cacheGetProperty = new Dictionary<string, Core.delgGetProperty>();
        private Dictionary<string, Core.delgSetProperty> cacheSetProperty = new Dictionary<string, Core.delgSetProperty>();
        private Dictionary<string, Core.delgGetField> cacheGetField = new Dictionary<string, Core.delgGetField>();
        private Dictionary<string, Core.delgSetField> cacheSetField = new Dictionary<string, Core.delgSetField>();
        //private static Dictionary<Type, ITypeProcessor> _cache = new Dictionary<Type, ITypeProcessor>();
        private static object _lockObj = new object();
        private static TypeProcessor<T> _thisInstance;
        #endregion

        #region ctor
        /// <summary>
        /// Construct TypeProcessor
        /// </summary>
        /// <param name="type">type of instance</param>
        /// <param name="genericTypes">generic type of type</param>
        /// <param name="argsTypes">args type of type</param>
        public TypeProcessor(Type[] genericTypes = null, Type[] argsTypes = null)
        {
            if (_thisInstance == null)
            {
                lock (_lockObj)
                {
                    if (_thisInstance == null)
                    {
                        _thisInstance = this;
                        init(typeof(T), genericTypes, argsTypes);
                    }
                }
            }
        }

        /// <summary>
        /// init type setting
        /// </summary>
        /// <param name="type">type of instance </param>
        /// <param name="genericTypes">generic type of instance</param>
        /// <param name="argsType">args type of type</param>
        private void init(Type type, Type[] genericTypes, Type[] argsType)
        {
            _thisInstance.instanceType = Core.MakeType(type, genericTypes); // make type and cache it
            _thisInstance.ctorInfo = Core.MakeCtorInfo(_thisInstance.instanceType, argsType); // make construct info and cache it
            _thisInstance.delgCreate = Core.GenCreateInstanceDelg(_thisInstance.ctorInfo); // make delegate create and cache it

            //make property operator caches
            PropertyInfo[] props = _thisInstance.instanceType.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.CanRead)
                {
                    Core.delgGetProperty delgGetProperty = Core.GenGetPropertyValueDelg(_thisInstance.instanceType, prop);
                    _thisInstance.cacheGetProperty.Add(prop.Name, delgGetProperty);
                }
                if (prop.CanWrite)
                {
                    Core.delgSetProperty delgSetProperty = Core.GenSetPropertyValueDelg(_thisInstance.instanceType, prop);
                    _thisInstance.cacheSetProperty.Add(prop.Name, delgSetProperty);
                }
            }

            //make field operator caches
            FieldInfo[] fields = _thisInstance.instanceType.GetFields();
            foreach (FieldInfo field in fields)
            {
                Core.delgGetField delgGetField = Core.GenGetFieldValueDelg(_thisInstance.instanceType, field);
                Core.delgSetField delgSetField = Core.GenSetFieldValueDelg(_thisInstance.instanceType, field);
                _thisInstance.cacheGetField.Add(field.Name, delgGetField);
                _thisInstance.cacheSetField.Add(field.Name, delgSetField);
            }
        }

        #endregion

        #region action
        /// <summary>
        /// create new instance
        /// </summary>
        /// <param name="args">construct args</param>
        /// <returns>new instance</returns>
        public T CreateInstance(object[] args = null)
        {
            return (T)_thisInstance.delgCreate(args);
        }

        /// <summary>
        /// invoke method
        /// </summary>
        /// <param name="methodName">method name</param>
        /// <param name="genericsType">generics type of method</param>
        /// <param name="argsType">args type of method args</param>
        /// <param name="args">args of method call</param>
        /// <returns></returns>
        public object MethodCall(object instance, string methodName, Type[] genericsType = null, Type[] argsType = null, object[] args = null)
        {
            MethodInfo methodInfo = Core.MakeMethodInfo(_thisInstance.instanceType, methodName, genericsType, argsType);
            Core.delgMethodCall delg = Core.GenMethodCallDelg(methodInfo);
            return delg(instance, args);
        }

        /// <summary>
        /// invoke void
        /// </summary>
        /// <param name="voidName">void name</param>
        /// <param name="genericsType">generics type of void</param>
        /// <param name="argsType">args type of void args</param>
        /// <param name="args">args of void call</param>
        public void VoidCall(object instance, string voidName, Type[] genericsType = null, Type[] argsType = null, object[] args = null)
        {
            MethodInfo methodInfo = Core.MakeMethodInfo(_thisInstance.instanceType, voidName, genericsType, argsType);
            Core.delgVoidCall delg = Core.GenVoidCallDelg(methodInfo);
            delg(instance, args);
        }

        #endregion

        #region Field

        public object GetField(object instance, FieldInfo field)
        {
            return _thisInstance.GetField(instance, field.Name);
        }
        public object GetField(object instance, string fieldName)
        {
            return _thisInstance.cacheGetField[fieldName](instance);
        }

        public void SetField(object instance, FieldInfo field, object value)
        {
            _thisInstance.SetField(instance, field.Name, value);
        }

        public void SetField(object instance, string fieldName, object value)
        {
            _thisInstance.cacheSetField[fieldName](instance, value);
        }
        #endregion

        #region Property
        public object GetProperty(object instance, PropertyInfo prop)
        {
            return _thisInstance.GetProperty(instance, prop.Name);
        }

        public object GetProperty(object instance, string propName)
        {
            return _thisInstance.cacheGetProperty[propName](instance);
        }

        public void SetProperty(object instance, PropertyInfo prop, object value)
        {
            _thisInstance.SetProperty(instance, prop.Name, value);
        }

        public void SetProperty(object instance, string propName, object value)
        {
            _thisInstance.cacheSetProperty[propName](instance, value);
            
        }
        #endregion

    }
}
