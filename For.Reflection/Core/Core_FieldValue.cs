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
        /// set field value
        /// field name is include in delegate, tell delegate which instance to set value
        /// </summary>
        /// <param name="instance">static type fill in null</param>
        /// <param name="value">value</param>
        public delegate void delgSetField(object instance, object value);

        /// <summary>
        /// get field value
        /// field name is include in delegate, tell delegate which instance to get value
        /// </summary>
        /// <param name="instance">static type fill in null</param>
        /// <returns>field value</returns>
        public delegate object delgGetField(object instance);

        /// <summary>
        /// get FieldInfo for create delegate
        /// </summary>
        /// <param name="instanceType">type of instance</param>
        /// <param name="fieldName">field name</param>
        /// <returns>FieldInfo</returns>
        public static FieldInfo GetFieldInfo(Type instanceType, string fieldName)
        {
            return instanceType.GetField(fieldName);
        }

        /// <summary>
        /// make delegate by instanceType and field info for set field value
        /// </summary>
        /// <param name="type">type of instance</param>
        /// <param name="field">field info of field</param>
        /// <returns>delegate set field</returns>
        public static delgSetField GenSetFieldValueDelg(Type type, FieldInfo field)
        {

            var typeName = type.FullName;
            var keyName = typeName + field.FieldType.Name + field.Name + "_Set";//typeName + fieldName + value.GetType().Name + "_Set";
            //check delegate exist in cache
            if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
            {
                Caches.Lock(CacheType.SetFieldValue);
                if (!Caches.IsExist(CacheType.SetFieldValue, keyName))
                {
                    try
                    {
                        ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
                        ParameterExpression valueExp = Expression.Parameter(typeof(object), "value");
                        //becouse delegate params is object,so have to make convert expression to real type
                        MemberExpression fieldExp = Expression.Field(Expression.Convert(targetExp, type), field);
                        BinaryExpression assignExp = Expression.Assign(fieldExp, Expression.Convert(valueExp, field.FieldType));
                        LambdaExpression lambdax = Expression.Lambda(typeof(delgSetField), assignExp, targetExp, valueExp);
                        //create delegate and add to cache
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
            //get delegate from cache
            delgSetField SetFieldAction = (delgSetField)Caches.GetValue(CacheType.SetFieldValue, keyName);
            return SetFieldAction;
        }

        /// <summary>
        /// make delegate by instanceType and field info for get field value
        /// </summary>
        /// <param name="type">type of instance</param>
        /// <param name="field">field info of field</param>
        /// <returns>delegate get field</returns>
        public static delgGetField GenGetFieldValueDelg(Type type, FieldInfo field)
        {
            var typeName = type.FullName;
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
                        MemberExpression fieldExp = Expression.Field(Expression.Convert(targetExp, type), field);

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
