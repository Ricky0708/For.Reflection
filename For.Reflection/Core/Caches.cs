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

        private static Dictionary<string, object> dictionarySetValue = new Dictionary<string, object>();
        private static Dictionary<string, object> dictionaryGetValue = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheEnum"></param>
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
                    result = dictionarySetValue.ContainsKey(key);
                    break;
                case CacheType.GetFieldValue:
                case CacheType.SetPropertyValue:
                case CacheType.GetPropertyValue:
                default:
                    break;
            }
            return result;
        }

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
                    obj = dictionarySetValue[key];
                    break;
                case CacheType.GetFieldValue:
                case CacheType.SetPropertyValue:
                case CacheType.GetPropertyValue:
                default:
                    break;
            }
            return obj;
        }

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
                    dictionarySetValue.Add(key, value);
                    break;
                case CacheType.GetFieldValue:
                case CacheType.SetPropertyValue:
                case CacheType.GetPropertyValue:
                default:
                    break;
            }
            return value;
        }

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
                    Monitor.Enter(dictionarySetValue);
                    break;
                case CacheType.GetFieldValue:
                case CacheType.SetPropertyValue:
                case CacheType.GetPropertyValue:
                default:
                    break;
            }
        }

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
                    Monitor.Exit(dictionarySetValue);
                    break;
                case CacheType.GetFieldValue:
                case CacheType.SetPropertyValue:
                case CacheType.GetPropertyValue:
                default:
                    break;
            }
        }
    }
}
