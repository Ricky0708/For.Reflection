using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    public static partial class Core
    {

        public delegate void delgSetProperty(object instance, object value);
        public delegate object delgGetProperty(object instance);


        public static PropertyInfo GetPropertyInfo(Type instanceType, string propertyName)
        {
            return instanceType.GetProperty(propertyName);
        }

        /// <summary>
        /// set property value
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="propertyName"></param>
        /// <param name="valueType"></param>
        public static delgSetProperty GenSetPropertyValueDelg(Type instanceType, PropertyInfo property)
        {

            var typeName = instanceType.FullName;
            var keyName = typeName + property.PropertyType.Name + property.Name + "_Set";//typeName + propertyName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.SetPropertyValue, keyName))
            {
                Caches.Lock(CacheType.SetPropertyValue);
                if (!Caches.IsExist(CacheType.SetPropertyValue, keyName))
                {
                    try
                    {
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        ParameterExpression valueExp = Expression.Parameter(typeof(object), "value");
                        MemberExpression propertyExp = Expression.Property(Expression.Convert(targetExp, instanceType), property);
                        BinaryExpression assignExp = Expression.Assign(propertyExp, Expression.Convert(valueExp, property.PropertyType));
                        LambdaExpression lambdax = Expression.Lambda(typeof(delgSetProperty), assignExp, targetExp, valueExp);
                        delgSetProperty delg = (delgSetProperty)lambdax.Compile();
                        Caches.Add(CacheType.SetPropertyValue, keyName, delg);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Caches.Unlock(CacheType.SetPropertyValue);
                    }
                }
            }
            delgSetProperty SetPropertyAction = (delgSetProperty)Caches.GetValue(CacheType.SetPropertyValue, keyName);
            return SetPropertyAction;
        }

        public static delgGetProperty GenGetPropertyValueDelg(Type instanceType, PropertyInfo property)
        {
            var typeName = instanceType.FullName;
            var keyName = typeName + property.PropertyType.Name + property.Name + "_Get";//typeName + propertyName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.GetPropertyValue, keyName))
            {
                Caches.Lock(CacheType.GetPropertyValue);
                if (!Caches.IsExist(CacheType.GetPropertyValue, keyName))
                {
                    try
                    {
                        //GenericSetActionExpression(instance, propertyName, value);
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        MemberExpression propertyExp = Expression.Property(Expression.Convert(targetExp, instanceType), property);

                        LambdaExpression lambdax = Expression.Lambda(typeof(delgGetProperty), Expression.Convert(propertyExp, typeof(object)), targetExp);
                        delgGetProperty delg = (delgGetProperty)lambdax.Compile();
                        Caches.Add(CacheType.GetPropertyValue, keyName, delg);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Caches.Unlock(CacheType.GetPropertyValue);
                    }
                }
            }
            delgGetProperty SetPropertyAction = (delgGetProperty)Caches.GetValue(CacheType.GetPropertyValue, keyName);
            return SetPropertyAction;
        }
    }
}
