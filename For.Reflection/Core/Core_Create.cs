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
        /// <summary>
        /// delegate for create instance
        /// can not use in static type
        /// </summary>
        /// <param name="args">ctor args</param>
        /// <returns></returns>
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

                            //add type to cache  
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
            //get type from cache
            return (Type)Caches.GetValue(CacheType.Type, keyName);

        }

        /// <summary>
        /// make ctor info for create instance
        /// </summary>
        /// <param name="T">type of instance</param>
        /// <param name="types">types of ctor args type, if no args give it null</param>
        /// <returns></returns>
        public static ConstructorInfo MakeCtorInfo(Type T, params Type[] argsType)
        {
            string keyName = T.FullName + argsType.TypesToStringName();
            ConstructorInfo result = null;
            if (!Caches.IsExist(CacheType.CtorInfo, keyName))
            {
                Caches.Lock(CacheType.CtorInfo);
                try
                {
                    if (!Caches.IsExist(CacheType.CtorInfo, keyName))
                    {
                        if (CheckIsStaticType(T))
                        {
                            throw new ArgumentOutOfRangeException("Can not make ConstructorInfo for static type!");
                        }
                        if (argsType != null)
                        {
                            result = T.GetConstructor(argsType);
                        }
                        else
                        {
                            result = T.GetConstructor(new Type[] { });
                        }

                        if (result == null)
                        {
                            throw new ArgumentOutOfRangeException("Can't find Constructor");
                        }
                        else
                        {
                            Caches.Add(CacheType.CtorInfo, keyName, result);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Caches.Unlock(CacheType.CtorInfo);
                }

            }
            else
            {
                result = (ConstructorInfo)Caches.GetValue(CacheType.CtorInfo, keyName);

            }
            return result;
        }


        /// <summary>
        /// make delegate for create instance
        /// </summary>
        /// <param name="T">create type</param>
        /// <param name="argsType">ctor args type</param>
        /// <returns></returns>
        public static delgCreateInstance GenCreateInstanceDelg(ConstructorInfo ctorInfo)
        {

            if (ctorInfo == null)
            {
                return null;
            }
            string keyName = ctorInfo.ReflectedType.FullName + ctorInfo.ToString();
            //Delegate instance;
            delgCreateInstance instance;

            //check delegate exist in cache
            if (!Caches.IsExist(CacheType.Create, keyName))
            {
                Caches.Lock(CacheType.Create);
                try
                {
                    if (!Caches.IsExist(CacheType.Create, keyName))
                    {
                        ParameterExpression pxpr = Expression.Parameter(typeof(object[]), "args");
                        //becouse delegate params is object,so have to make convert expression to real type
                        Expression[] argsExp = ConvertParasInfoToExpr(pxpr, ctorInfo.GetParameters());
                        NewExpression ctor = Expression.New(ctorInfo, argsExp);
                        //create delegate and add to cache
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
            //get delegate from cache
            instance = (delgCreateInstance)Caches.GetValue(CacheType.Create, keyName);
            return instance;
        }
    }
}
