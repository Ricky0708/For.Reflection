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

        /// <summary>
        /// instance is include in delegate, fill args to call this method
        /// </summary>
        /// <param name="instance">static object, fill in null</param>
        /// <param name="args">method arguments</param>
        /// <returns>method return</returns>
        public delegate object delgMethodCall(object instance, params object[] args);

        /// <summary>
        /// instance is include in delegate, fill args to call this void
        /// </summary>
        /// <param name="instance">static object, fill in null</param>
        /// <param name="args">void arguments</param>
        public delegate void delgVoidCall(object instance, params object[] args);


        /// <summary>
        /// make method info
        /// </summary>
        /// <param name="T">type of caller instance</param>
        /// <param name="methodName">name of method which you will call</param>
        /// <param name="genericsType">if not generic method, fill in null</param>
        /// <param name="parametersType">if no any args, fill in null</param>
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
        /// delegate method call
        /// </summary>
        /// <param name="methodInfo">methodinfo</param>
        /// <returns>delgMethodCall</returns>
        public static delgMethodCall GenMethodCallDelg(MethodInfo methodInfo, params Type[] args)
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
                        ParameterExpression instanceExpr = Expression.Parameter(typeof(object), "target");
                        ParameterExpression pxpr = Expression.Parameter(typeof(object[]), "args");
                        Expression[] argsExp = ConvertParasInfoToExpr(pxpr, methodInfo.GetParameters());
                        MethodCallExpression mxpr;
                        if (methodInfo.IsStatic)
                        {
                            mxpr = Expression.Call(methodInfo, argsExp);
                        }
                        else
                        {
                            mxpr = Expression.Call(Expression.Convert(instanceExpr, methodInfo.DeclaringType), methodInfo, argsExp);
                        }
                        LambdaExpression lambdax = Expression.Lambda(typeof(delgMethodCall), mxpr, instanceExpr, pxpr);
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

        /// <summary>
        /// delegate void call
        /// </summary>
        /// <param name="methodInfo">methodinfo</param>
        /// <returns>delgVoidCall</returns>
        public static delgVoidCall GenVoidCallDelg(MethodInfo methodInfo)
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
                        ParameterExpression instanceExpr = Expression.Parameter(typeof(object), "target");
                        ParameterExpression pxpr = Expression.Parameter(typeof(object[]), "args");
                        Expression[] argsExp = ConvertParasInfoToExpr(pxpr, methodInfo.GetParameters());
                        MethodCallExpression mxpr;
                        if (methodInfo.IsStatic)
                        {
                            mxpr = Expression.Call(methodInfo, argsExp);
                        }
                        else
                        {
                            mxpr = Expression.Call(Expression.Convert(instanceExpr, methodInfo.DeclaringType), methodInfo, argsExp);
                        }
                        LambdaExpression lambdax = Expression.Lambda(typeof(delgVoidCall), mxpr, instanceExpr, pxpr);
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
