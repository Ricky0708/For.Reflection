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
        /// <param name="typeName">Type name</param>
        /// <param name="assemblyName">Type's assembly name</param>
        /// <param name="genericTypes">Generic types</param>
        /// <returns></returns>
        public static Type MakeType(string typeName, string assemblyName, params Type[] genericTypes)
        {
            return Core.MakeType(Type.GetType(typeName + ", " + assemblyName), genericTypes);
        }
  
        public static object CreateInstance(Type T, Type[] argsType, object[] args)
        {
            return Core.GenCreateInstanceDelg(T, argsType)(args);
        }

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
        /// Get MethodInfo
        /// </summary>
        /// <param name="T"></param>
        /// <param name="methodName"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MethodInfo MakeMethodInfo(Type T, string methodName, Type[] GenericsType, Type[] ParametersType)
        {
            return Core.MakeMethodInfo(T, methodName, GenericsType, ParametersType);
        }

        public static object MethodCall(object instance, MethodInfo methodInfo, object[] args)
        {
            return Core.GenMethodCallDelg(instance, methodInfo)(args);
        }

        public static T MethodCall<T>(object instance, MethodInfo methodInfo, object[] args)
        {
            //TODO:Test null
            return (T)Core.GenMethodCallDelg(instance, methodInfo)(args);
        }

        public static void VoidCall(object instance, MethodInfo methodInfo, object[] args)
        {
            Core.GenVoidCallDelg(instance, methodInfo)(args);
        }
        #endregion

        #region Field Value
        public static void SetFieldValue(this object instance, string fieldName, dynamic value)
        {
            Core.GenSetFieldValueDelg(instance, fieldName, value)(instance, fieldName, value);
        }

        public static object GetFieldValue(this object instance, string fieldName)
        {
           
        }

        #endregion
    }
}
