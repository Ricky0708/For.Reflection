using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace For.Reflection
{
    /// <summary>
    /// 基本上MakeType只有在建立未事先建立型別的泛型型別才會用到，機會比較少，所以拆開
    /// </summary>
    public static partial class Core
    {

        public delegate object delgCreateInstance(params object[] args);

        /// <summary>
        /// Make type or generic type by Type
        /// </summary>
        /// <param name="T">Type</param>
        /// <param name="genericTypes">Generic types</param>
        /// <returns></returns>
        public static Type MakeType(Type T, params Type[] genericTypes)
        {
            string keyName = T.FullName + genericTypes.TypesToStringName();

            if (!Caches.IsExist(CacheType.Type, keyName))
            {
                Caches.Lock(CacheType.Type);
                try
                {
                    if (!Caches.IsExist(CacheType.Type, keyName))
                    {

                        if (genericTypes == null || genericTypes.Count() == 0)
                        {
                            Caches.Add(CacheType.Type, keyName, T);
                        }
                        else
                        {
                            //static type can not be generic type
                            foreach (Type type in genericTypes)
                            {
                                if (CheckIsStaticType(type))
                                {
                                    throw new Exception("static type can not be generic type");
                                }
                            }
                            Caches.Add(CacheType.Type, keyName, T.MakeGenericType(genericTypes));
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Caches.Unlock(CacheType.Type);
                }
            }
            return (Type)Caches.GetValue(CacheType.Type, keyName);

        }


        public static delgCreateInstance GenCreateInstanceDelg(Type T, Type[] argsType)
        {

            if (T == null)
            {
                return null;
            }
            string keyName = T.FullName + argsType.TypesToStringName();
            //Delegate instance;
            delgCreateInstance instance;

            if (!Caches.IsExist(CacheType.Create, keyName))
            {
                Caches.Lock(CacheType.Create);
                try
                {
                    if (!Caches.IsExist(CacheType.Create, keyName))
                    {
                        if (CheckIsStaticType(T))
                        {
                            throw new Exception("Can not create static type");
                        }
                        ParameterExpression pxpr = Expression.Parameter(typeof(object[]), "args");
                        ConstructorInfo ctorInfo = MakeCtorInfo(T, argsType);
                        Expression[] argsExp = ConvertParasInfoToExpr(pxpr, ctorInfo.GetParameters());
                        NewExpression ctor = Expression.New(ctorInfo, argsExp);

                        instance = (delgCreateInstance)Expression.Lambda(typeof(delgCreateInstance), ctor, pxpr).Compile();
                        Caches.Add(CacheType.Create, keyName, instance);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Caches.Unlock(CacheType.Create);
                }
            }

            instance = (delgCreateInstance)Caches.GetValue(CacheType.Create, keyName);
            return instance;
        }
    }
}
