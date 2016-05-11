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

        public delegate void delgSetField(object obj, object args);
        public delegate object delgGetField(object obj);


        public static FieldInfo GetFieldInfo(Type instanceType, string fieldName)
        {
            return instanceType.GetField(fieldName);
        }

        /// <summary>
        /// set field value
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="propertyName"></param>
        /// <param name="valueType"></param>
        public static delgSetField GenSetFieldValueDelg(Type instanceType, FieldInfo field)
        {

            var typeName = instanceType.FullName;
            var keyName = typeName + field.FieldType.Name + "_Set";//typeName + fieldName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
            {
                Caches.Lock(CacheType.SetFieldValue);
                if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
                {
                    try
                    {
                        //GenericSetActionExpression(instance, fieldName, value);
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        ParameterExpression valueExp = Expression.Parameter(typeof(object), "value");
                        MemberExpression fieldExp = Expression.Field(Expression.Convert(targetExp, instanceType), field);
                        BinaryExpression assignExp = Expression.Assign(fieldExp, Expression.Convert(valueExp, valueType));
                        LambdaExpression lambdax = Expression.Lambda(typeof(delgSetField), assignExp, targetExp, valueExp);
                        delgSetField delg = (delgSetField)lambdax.Compile();
                        Caches.Add(CacheType.SetFieldValue, keyName, delg);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Caches.Unlock(CacheType.SetFieldValue);
                    }
                }
            }
            delgSetField SetFieldAction = (delgSetField)Caches.GetValue(CacheType.SetFieldValue, keyName);
            return SetFieldAction;
        }

        public static delgGetField GenGetFieldValueDelg(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName);
            var typeName = instance.GetType().FullName;
            var keyName = typeName + fieldName + "_Get";//typeName + fieldName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
            {
                Caches.Lock(CacheType.SetFieldValue);
                if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
                {
                    try
                    {
                        //GenericSetActionExpression(instance, fieldName, value);
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        MemberExpression fieldExp = Expression.Field(Expression.Convert(targetExp, instance.GetType()), field);

                        LambdaExpression lambdax = Expression.Lambda(typeof(delgGetField), Expression.Convert(fieldExp, typeof(object)), targetExp);
                        delgGetField delg = (delgGetField)lambdax.Compile();
                        Caches.Add(CacheType.SetFieldValue, keyName, delg);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Caches.Unlock(CacheType.SetFieldValue);
                    }
                }
            }
            delgGetField SetFieldAction = (delgGetField)Caches.GetValue(CacheType.SetFieldValue, keyName);
            return SetFieldAction;
        }
    }
}
