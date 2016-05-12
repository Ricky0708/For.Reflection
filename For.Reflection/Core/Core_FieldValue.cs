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

        public delegate void delgSetField(object instance, object value);
        public delegate object delgGetField(object instance);


        public static FieldInfo GetFieldInfo(Type instanceType, string fieldName)
        {
            return instanceType.GetField(fieldName);
        }

        /// <summary>
        /// set field value
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="fieldName"></param>
        /// <param name="valueType"></param>
        public static delgSetField GenSetFieldValueDelg(Type instanceType, FieldInfo field)
        {

            var typeName = instanceType.FullName;
            var keyName = typeName + field.FieldType.Name + field.Name + "_Set";//typeName + fieldName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
            {
                Caches.Lock(CacheType.SetFieldValue);
                if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
                {
                    try
                    {
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        ParameterExpression valueExp = Expression.Parameter(typeof(object), "value");
                        MemberExpression fieldExp = Expression.Field(Expression.Convert(targetExp, instanceType), field);
                        BinaryExpression assignExp = Expression.Assign(fieldExp, Expression.Convert(valueExp, field.FieldType));
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

        public static delgGetField GenGetFieldValueDelg(Type instanceType, FieldInfo field)
        {
            var typeName = instanceType.FullName;
            var keyName = typeName + field.FieldType.Name + field.Name + "_Get";//typeName + fieldName + value.GetType().Name + "_Set";
            if (!Caches.IsExist(CacheType.GetFieldValue, keyName))
            {
                Caches.Lock(CacheType.GetFieldValue);
                if (!Caches.IsExist(CacheType.GetFieldValue, keyName))
                {
                    try
                    {
                        //GenericSetActionExpression(instance, fieldName, value);
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        MemberExpression fieldExp = Expression.Field(Expression.Convert(targetExp, instanceType), field);

                        LambdaExpression lambdax = Expression.Lambda(typeof(delgGetField), Expression.Convert(fieldExp, typeof(object)), targetExp);
                        delgGetField delg = (delgGetField)lambdax.Compile();
                        Caches.Add(CacheType.GetFieldValue, keyName, delg);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Caches.Unlock(CacheType.GetFieldValue);
                    }
                }
            }
            delgGetField SetFieldAction = (delgGetField)Caches.GetValue(CacheType.GetFieldValue, keyName);
            return SetFieldAction;
        }
    }
}
