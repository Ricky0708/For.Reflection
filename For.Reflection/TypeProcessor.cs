using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    public class TypeProcessor : ITypeProcessor
    {
        #region var
        private Type instanceType;
        private ConstructorInfo ctorInfo;
        private Core.delgCreateInstance delgCreate;
        private Dictionary<string, Core.delgGetProperty> cacheGetProperty = new Dictionary<string, Core.delgGetProperty>();
        private Dictionary<string, Core.delgSetProperty> cacheSetProperty = new Dictionary<string, Core.delgSetProperty>();
        private Dictionary<string, Core.delgGetField> cacheGetField = new Dictionary<string, Core.delgGetField>();
        private Dictionary<string, Core.delgSetField> cacheSetField = new Dictionary<string, Core.delgSetField>();
        #endregion

        #region ctor

        /// <summary>
        /// Construct TypeProcessor
        /// </summary>
        /// <param name="type">type of instance</param>
        /// <param name="genericTypes">generic type of type</param>
        /// <param name="argsType">args type of type</param>
        public TypeProcessor(Type type, Type[] genericTypes = null, Type[] argsType = null)
        {
            //init type setting
            init(type, genericTypes, argsType);
        }

        /// <summary>
        /// init type setting
        /// </summary>
        /// <param name="type">type of instance </param>
        /// <param name="genericTypes">generic type of instance</param>
        /// <param name="argsType">args type of type</param>
        private void init(Type type, Type[] genericTypes, Type[] argsType)
        {
            instanceType = Core.MakeType(type, genericTypes); // make type and cache it
            ctorInfo = Core.MakeCtorInfo(instanceType, argsType); // make construct info and cache it
            delgCreate = Core.GenCreateInstanceDelg(ctorInfo); // make delegate create and cache it

            //make property operator caches
            PropertyInfo[] props = instanceType.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.CanRead)
                {
                    Core.delgGetProperty delgGetProperty = Core.GenGetPropertyValueDelg(instanceType, prop);
                    cacheGetProperty.Add(prop.Name, delgGetProperty);
                }
                if (prop.CanWrite)
                {
                    Core.delgSetProperty delgSetProperty = Core.GenSetPropertyValueDelg(instanceType, prop);
                    cacheSetProperty.Add(prop.Name, delgSetProperty);
                }
            }

            //make field operator caches
            FieldInfo[] fields = instanceType.GetFields();
            foreach (FieldInfo field in fields)
            {
                Core.delgGetField delgGetField = Core.GenGetFieldValueDelg(instanceType, field);
                Core.delgSetField delgSetField = Core.GenSetFieldValueDelg(instanceType, field);
                cacheGetField.Add(field.Name, delgGetField);
                cacheSetField.Add(field.Name, delgSetField);
            }
        }

        #endregion

        #region action
        /// <summary>
        /// create new instance
        /// </summary>
        /// <param name="args">construct args</param>
        /// <returns>new instance</returns>
        public object CreateInstance(object[] args = null)
        {
            return delgCreate(args);
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
            MethodInfo methodInfo = Core.MakeMethodInfo(instanceType, methodName, genericsType, argsType);
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
            MethodInfo methodInfo = Core.MakeMethodInfo(instanceType, voidName, genericsType, argsType);
            Core.delgVoidCall delg = Core.GenVoidCallDelg(methodInfo);
            delg(instance, args);
        }

        #endregion

        #region Field

        public object GetField(object instance, FieldInfo field)
        {
            string fieldName = field.Name;
            return GetField(instance, fieldName);
        }
        public object GetField(object instance, string fieldName)
        {
            if (cacheGetField.ContainsKey(fieldName))
            {
                return cacheGetField[fieldName](instance);
            }
            return null;
        }

        public void SetField(object instance, FieldInfo field, object value)
        {
            string fieldName = field.Name;
            SetField(instance, fieldName, value);
        }

        public void SetField(object instance, string fieldName, object value)
        {
            if (cacheSetField.ContainsKey(fieldName))
            {
                cacheSetField[fieldName](instance, value);
            }
            else
            {
                throw new ArgumentOutOfRangeException(fieldName);
            }
        }
        #endregion

        #region Property
        public object GetProperty(object instance, PropertyInfo prop)
        {
            string propName = prop.Name;
            return GetProperty(instance, propName);
        }

        public object GetProperty(object instance, string propName)
        {
            if (cacheGetProperty.ContainsKey(propName))
            {
                return cacheGetProperty[propName](instance);
            }
            return null;
        }

        public void SetProperty(object instance, PropertyInfo prop, object value)
        {
            string propName = prop.Name;
            SetProperty(instance, propName, value);
        }

        public void SetProperty(object instance, string propName, object value)
        {
            if (cacheSetProperty.ContainsKey(propName))
            {
                cacheSetProperty[propName](instance, value);
            }
            else
            {
                throw new ArgumentOutOfRangeException(propName);
            }
        }
        #endregion

    }
}
