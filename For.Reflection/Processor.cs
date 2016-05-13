using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    public static class Processor
    {

        #region Create
        /// <summary>
        /// Make type or generic type by Type
        /// </summary>
        /// <param name="T">Type</param>
        /// <param name="genericTypes">Generic types</param>
        /// <returns></returns>
        public static Type MakeType(Type T, params Type[] genericTypes)
        {
            return Core.MakeType(T, genericTypes);
        }

        /// <summary>
        /// Make type or generic type by assembly and type full name
        /// </summary>
        /// <param name="typeFullName">Type name</param>
        /// <param name="assemblyName">Type's assembly name</param>
        /// <param name="genericTypes">Generic types</param>
        /// <returns></returns>
        public static Type MakeType(string typeFullName, string assemblyName, params Type[] genericTypes)
        {
            return Core.MakeType(Type.GetType(typeFullName + ", " + assemblyName), genericTypes);
        }

        /// <summary>
        /// create instance
        /// </summary>
        /// <param name="T">type of instance</param>
        /// <param name="argsType">if no args, fill in null</param>
        /// <param name="args">if no args, fill in null</param>
        /// <returns>object</returns>
        public static object CreateInstance(Type T, Type[] argsType, object[] args)
        {
            return Core.GenCreateInstanceDelg(T, argsType)(args);
        }

        /// <summary>
        /// create instance
        /// </summary>
        /// <typeparam name="T">type of instance</typeparam>
        /// <param name="argsType">if no args, fill in null</param>
        /// <param name="args">if no args, fill in null</param>
        /// <returns>T</returns>
        public static T CreateInstance<T>(Type[] argsType, object[] args)
        {
            object instance = Core.GenCreateInstanceDelg(typeof(T), argsType)(args);
            if (instance != null)
            {
                return (T)instance;
            }
            return default(T);
        }
        #endregion

        #region Method & Void
        /// <summary>
        /// make method info
        /// </summary>
        /// <param name="T">type of caller instance</param>
        /// <param name="methodName">name of method which you will call</param>
        /// <param name="genericsType">if not generic method, fill in null</param>
        /// <param name="parametersType">if no any args, fill in null</param>
        public static MethodInfo MakeMethodInfo(Type T, string methodName, Type[] genericsType, Type[] parametersType)
        {
            return Core.MakeMethodInfo(T, methodName, genericsType, parametersType);
        }

        /// <summary>
        /// method call
        /// method call have to dynamic compile
        /// becouse it can't dynamic plug in instance
        /// if you want get better proformance,use delegate and keep cache by yourself
        /// </summary>
        /// <param name="instance">fill null for static type</param>
        /// <param name="methodInfo">methodinfo</param>
        /// <param name="args">method args, if no args, fill in null</param>
        /// <returns>delgMethodCall</returns>
        public static object MethodCall(object instance, MethodInfo methodInfo, object[] args)
        {
            return Core.GenMethodCallDelg(instance, methodInfo)(args);
        }

        /// <summary>
        /// <see cref="MethodCall(object, MethodInfo, object[])"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="methodInfo"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T MethodCall<T>(object instance, MethodInfo methodInfo, object[] args)
        {
            //TODO:Test null
            return (T)Core.GenMethodCallDelg(instance, methodInfo)(args);
        }

        /// <summary>
        /// delegate void call
        /// void call have to dynamic compile
        /// becouse it can't dynamic plug in instance
        /// if you want get better proformance, keep cache by yourself
        /// </summary>
        /// <param name="instance">fill null for static type</param>
        /// <param name="methodInfo">methodinfo</param>
        /// <param name="args">void args, if no args, fill in null</param>
        /// <returns>delgVoidCall</returns>
        public static void VoidCall(object instance, MethodInfo methodInfo, object[] args)
        {
            Core.GenVoidCallDelg(instance, methodInfo)(args);
        }
        #endregion

        #region Field Value
        public static void SetFieldValue(this object instance, string propertyName, dynamic value)
        {
            Core.GenSetFieldValueDelg(instance.GetType(), instance.GetType().GetField(propertyName))(instance, value);
        }

        public static object GetFieldValue(this object instance, string fieldName)
        {
            return Core.GenGetFieldValueDelg(instance.GetType(), instance.GetType().GetField(fieldName))(instance);
        }

        #endregion

        #region Property Value
        public static void SetPropertyValue(this object instance, string propertyName, dynamic value)
        {
            Core.GenSetPropertyValueDelg(instance.GetType(), instance.GetType().GetProperty(propertyName))(instance, value);
        }

        public static object GetPropertyValue(this object instance, string propertyName)
        {
            return Core.GenGetPropertyValueDelg(instance.GetType(), instance.GetType().GetProperty(propertyName))(instance);
        }

        #endregion
    }
}
