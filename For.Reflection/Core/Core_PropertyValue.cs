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

        /// <summary>
        /// get property value
        /// property name is include in delegate, tell delegate which instance to set value
        /// </summary>
        /// <param name="instance">static type fill in null</param>
        /// <returns>property value</returns>
        public delegate void delgSetProperty(object instance, object value);

        /// <summary>
        /// get property value
        /// property name is include in delegate, tell delegate which instance to get value
        /// </summary>
        /// <param name="instance">static type fill in null</param>
        /// <returns>field value</returns>
        public delegate object delgGetProperty(object instance);


        /// <summary>
        /// get PropertyInfo for create delegate
        /// </summary>
        /// <param name="instanceType">type of instance</param>
        /// <param name="propertyName">property name</param>
        /// <returns>PropertyInfo</returns>
        public static PropertyInfo GetPropertyInfo(Type instanceType, string propertyName)
        {
            return instanceType.GetProperty(propertyName);
        }

        /// <summary>
        /// make delegate by instanceType and propertyinfo for set property value
        /// </summary>
        /// <param name="type">type of instance</param>
        /// <param name="property">property info of property</param>
        /// <returns>delegate set property</returns>
        public static delgSetProperty GenSetPropertyValueDelg(Type type, PropertyInfo property)
        {

            var typeName = type.FullName;
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
                        MemberExpression propertyExp = Expression.Property(Expression.Convert(targetExp, type), property);
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


        /// <summary>
        /// make delegate by instanceType and property info for get property value
        /// </summary>
        /// <param name="type">type of instance</param>
        /// <param name="property">property info of property</param>
        /// <returns>delegate get property</returns>
        public static delgGetProperty GenGetPropertyValueDelg(Type type, PropertyInfo property)
        {
            var typeName = type.FullName;
            var keyName = typeName + property.PropertyType.Name + property.Name + "_Get";//typeName + propertyName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.GetPropertyValue, keyName))
            {
                Caches.Lock(CacheType.GetPropertyValue);
                if (!Caches.IsExist(CacheType.GetPropertyValue, keyName))
                {
                    try
                    {
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        MemberExpression propertyExp = Expression.Property(Expression.Convert(targetExp, type), property);

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
