using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace For.Reflection
{
    internal static class Caches
    {

        private static Dictionary<string, object> dictionaryTypes = new Dictionary<string, object>();
        private static Dictionary<string, object> dictionaryCreate = new Dictionary<string, object>();


        private static Dictionary<string, object> dictionaryMethodInfo = new Dictionary<string, object>();
        private static Dictionary<string, object> dictionaryMethodCall = new Dictionary<string, object>();

        private static Dictionary<string, object> dictionarySetFieldValue = new Dictionary<string, object>();
        private static Dictionary<string, object> dictionaryGetFieldValue = new Dictionary<string, object>();

        private static Dictionary<string, object> dictionarySetPropertyValue = new Dictionary<string, object>();
        private static Dictionary<string, object> dictionaryGetPropertyValue = new Dictionary<string, object>();

        /// <summary>
        /// check cache is exist
        /// </summary>
        /// <param name="cacheEnum">which cache</param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool IsExist(CacheType cacheEnum, string key)
        {
            bool result = false;
            switch (cacheEnum)
            {
                case CacheType.Type:
                    result = dictionaryTypes.ContainsKey(key);
                    break;
                case CacheType.Create:
                    result = dictionaryCreate.ContainsKey(key);
                    break;
                case CacheType.MethodInfo:
                    result = dictionaryMethodInfo.ContainsKey(key);
                    break;
                case CacheType.MethodCall:
                    result = dictionaryMethodCall.ContainsKey(key);
                    break;
                case CacheType.SetFieldValue:
                    result = dictionarySetFieldValue.ContainsKey(key);
                    break;
                case CacheType.GetFieldValue:
                    result = dictionaryGetFieldValue.ContainsKey(key);
                    break;
                case CacheType.SetPropertyValue:
                    result = dictionarySetPropertyValue.ContainsKey(key);
                    break;
                case CacheType.GetPropertyValue:
                    result = dictionaryGetPropertyValue.ContainsKey(key);
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// get cache
        /// </summary>
        /// <param name="cacheEnum"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static object GetValue(CacheType cacheEnum, string key)
        {

            object obj = null;
            switch (cacheEnum)
            {
                case CacheType.Type:
                    obj = dictionaryTypes[key];
                    break;
                case CacheType.Create:
                    obj = dictionaryCreate[key];
                    break;
                case CacheType.MethodInfo:
                    obj = dictionaryMethodInfo[key];
                    break;
                case CacheType.MethodCall:
                    obj = dictionaryMethodCall[key];
                    break;
                case CacheType.SetFieldValue:
                    obj = dictionarySetFieldValue[key];
                    break;
                case CacheType.GetFieldValue:
                    obj = dictionaryGetFieldValue[key];
                    break;
                case CacheType.SetPropertyValue:
                    obj = dictionarySetPropertyValue[key];
                    break;
                case CacheType.GetPropertyValue:
                    obj = dictionaryGetPropertyValue[key];
                    break;
                default:
                    break;
            }
            return obj;
        }

        /// <summary>
        /// add to cache
        /// </summary>
        /// <param name="cacheEnum"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static object Add(CacheType cacheEnum, string key, object value)
        {
            switch (cacheEnum)
            {
                case CacheType.Type:
                    dictionaryTypes.Add(key, value);
                    break;
                case CacheType.Create:
                    dictionaryCreate.Add(key, value);
                    break;
                case CacheType.MethodInfo:
                    dictionaryMethodInfo.Add(key, value);
                    break;
                case CacheType.MethodCall:
                    dictionaryMethodCall.Add(key, value);
                    break;
                case CacheType.SetFieldValue:
                    dictionarySetFieldValue.Add(key, value);
                    break;
                case CacheType.GetFieldValue:
                    dictionaryGetFieldValue.Add(key, value);
                    break;
                case CacheType.SetPropertyValue:
                    dictionarySetPropertyValue.Add(key, value);
                    break;
                case CacheType.GetPropertyValue:
                    dictionaryGetPropertyValue.Add(key, value);
                    break;
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// lock cache, make thread save
        /// </summary>
        /// <param name="cacheEnum"></param>
        internal static void Lock(CacheType cacheEnum)
        {
            switch (cacheEnum)
            {
                case CacheType.Type:
                    Monitor.Enter(dictionaryTypes);
                    break;
                case CacheType.Create:
                    Monitor.Enter(dictionaryCreate);
                    break;
                case CacheType.MethodInfo:
                    Monitor.Enter(dictionaryMethodInfo);
                    break;
                case CacheType.MethodCall:
                    Monitor.Enter(dictionaryMethodCall);
                    break;
                case CacheType.SetFieldValue:
                    Monitor.Enter(dictionarySetFieldValue);
                    break;
                case CacheType.GetFieldValue:
                    Monitor.Enter(dictionaryGetFieldValue);
                    break;
                case CacheType.SetPropertyValue:
                    Monitor.Enter(dictionarySetPropertyValue);
                    break;
                case CacheType.GetPropertyValue:
                    Monitor.Enter(dictionaryGetPropertyValue);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// unlock cache
        /// </summary>
        /// <param name="cacheEnum"></param>
        internal static void Unlock(CacheType cacheEnum)
        {
            switch (cacheEnum)
            {
                case CacheType.Type:
                    Monitor.Exit(dictionaryTypes);
                    break;
                case CacheType.Create:
                    Monitor.Exit(dictionaryCreate);
                    break;
                case CacheType.MethodInfo:
                    Monitor.Exit(dictionaryMethodInfo);
                    break;
                case CacheType.MethodCall:
                    Monitor.Exit(dictionaryMethodCall);
                    break;
                case CacheType.SetFieldValue:
                    Monitor.Exit(dictionarySetFieldValue);
                    break;
                case CacheType.GetFieldValue:
                    Monitor.Exit(dictionaryGetFieldValue);
                    break;
                case CacheType.SetPropertyValue:
                    Monitor.Exit(dictionarySetPropertyValue);
                    break;
                case CacheType.GetPropertyValue:
                    Monitor.Exit(dictionaryGetPropertyValue);
                    break;
                default:
                    break;
            }
        }
    }
}
