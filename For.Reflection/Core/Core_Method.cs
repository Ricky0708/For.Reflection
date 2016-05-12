using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    public static partial class Core
    {
        public delegate object delgMethodCall(params object[] args);
        public delegate void delgVoidCall(params object[] args);


        /// <summary>
        /// Get MethodInfo
        /// </summary>
        /// <param name="T"></param>
        /// <param name="methodName"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static MethodInfo MakeMethodInfo(Type T, string methodName, Type[] genericsType, Type[] parametersType)
        {
            if (parametersType == null)
            {
                parametersType = new Type[] { };
            }
            if (genericsType == null)
            {
                genericsType = new Type[] { };
            }

            string keyName = T.FullName + methodName + genericsType.TypesToStringName() + "Gen" + parametersType.TypesToStringName() + "Par";
            if (!Caches.IsExist(CacheType.MethodInfo, keyName))
            {
                Caches.Lock(CacheType.MethodInfo);
                try
                {
                    if (!Caches.IsExist(CacheType.MethodInfo, keyName))
                    {
                        var resultTyp = (from p in T.GetMethods()
                                         where p.Name == methodName &&
                                            (genericsType.Count() != 0 ?
                                                p.IsGenericMethodDefinition &&
                                                p.GetGenericArguments().Count() == genericsType.Count() :
                                                !p.IsGenericMethodDefinition) &&
                                            p.GetParameters().Count() == parametersType.Count() &&
                                            (parametersType.Count() != 0 ?
                                                p.GetParameters().Select(n => n.ParameterType).SequenceEqual(parametersType) : true)
                                         select p).FirstOrDefault();

                        if (resultTyp.IsGenericMethod)
                        {
                            resultTyp = resultTyp.MakeGenericMethod(genericsType);

                        }
                        Caches.Add(CacheType.MethodInfo, keyName, resultTyp);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Caches.Unlock(CacheType.MethodInfo);
                }
            }
            return (MethodInfo)Caches.GetValue(CacheType.MethodInfo, keyName);
        }

        /// <summary>
        /// Dynamic call method
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static delgMethodCall GenMethodCallDelg(object instance, MethodInfo methodInfo)
        {
            string keyName = methodInfo.ReflectedType.FullName + methodInfo.ToString() + "method"; //methodInfo.DeclaringType.FullName + methodInfo.Name + methodInfo.GetParameters().Select(p => p.GetType()).ToArray().TypesToStringName();


            delgMethodCall methodCall;

            if (!Caches.IsExist(CacheType.MethodCall, keyName))
            {
                Caches.Lock(CacheType.MethodCall);
                try
                {
                    if (!Caches.IsExist(CacheType.MethodCall, keyName))
                    {
                        ConstantExpression cxpr = null;
                        if (instance != null)
                        {
                            cxpr = Expression.Constant(instance);
                        }
                        ParameterExpression pxpr = Expression.Parameter(typeof(object[]), "args");
                        Expression[] argsExp = ConvertParasInfoToExpr(pxpr, methodInfo.GetParameters());
                        MethodCallExpression mxpr = Expression.Call(cxpr, methodInfo, argsExp);

                        LambdaExpression lambdax = Expression.Lambda(typeof(delgMethodCall), mxpr, pxpr);
                        methodCall = (delgMethodCall)lambdax.Compile();
                        Caches.Add(CacheType.MethodCall, keyName, methodCall);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Caches.Unlock(CacheType.MethodCall);
                }
            }
            methodCall = (delgMethodCall)Caches.GetValue(CacheType.MethodCall, keyName);
            return methodCall;
        }

        public static delgVoidCall GenVoidCallDelg(object instance, MethodInfo methodInfo)
        {
            string keyName = methodInfo.ReflectedType.FullName + methodInfo.ToString() + "void";//methodInfo.DeclaringType.FullName + methodInfo.Name + methodInfo.GetParameters().Select(p => p.GetType()).ToArray().TypesToStringName();
            delgVoidCall voidCall;
            if (!Caches.IsExist(CacheType.MethodCall, keyName))
            {
                Caches.Lock(CacheType.MethodCall);
                try
                {
                    if (!Caches.IsExist(CacheType.MethodCall, keyName))
                    {
                        ConstantExpression cxpr = null;
                        if (instance != null)
                        {
                            cxpr = Expression.Constant(instance);
                        }
                        ParameterExpression pxpr = Expression.Parameter(typeof(object[]), "args");
                        Expression[] argsExp = ConvertParasInfoToExpr(pxpr, methodInfo.GetParameters());
                        MethodCallExpression mxpr = Expression.Call(cxpr, methodInfo, argsExp);

                        LambdaExpression lambdax = Expression.Lambda(typeof(delgVoidCall), mxpr, pxpr);
                        voidCall = (delgVoidCall)lambdax.Compile();
                        Caches.Add(CacheType.MethodCall, keyName, voidCall);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Caches.Unlock(CacheType.MethodCall);
                }
            }
            voidCall = (delgVoidCall)Caches.GetValue(CacheType.MethodCall, keyName);

            return voidCall;
        }
    }
}
