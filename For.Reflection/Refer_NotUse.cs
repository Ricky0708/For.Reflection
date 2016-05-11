using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    internal class Refer_NotUse
    {
        private delegate object EmitCreate();

        private static void GenericSetActionEmit(object instance, string fieldName, dynamic value)
        {
            var field = instance.GetType().GetField(fieldName);
            var typeName = instance.GetType().Name;

            DynamicMethod method = new DynamicMethod("SetField", typeof(void), new Type[] { instance.GetType(), value.GetType() });
            ILGenerator ilGenerator = method.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Box, field.FieldType);
            ilGenerator.Emit(OpCodes.Unbox_Any, field.FieldType);
            ilGenerator.Emit(OpCodes.Stfld, field);
            ilGenerator.Emit(OpCodes.Ret);
            method.DefineParameter(1, ParameterAttributes.In, "obj");
            method.DefineParameter(2, ParameterAttributes.In, "value");
            var delegateType = typeof(Action<,>).MakeGenericType(instance.GetType(), value.GetType());
            var setAction = method.CreateDelegate(delegateType);
            Caches.Add(CacheType.SetFieldValue, typeName + fieldName + value.GetType().Name + "_Set", setAction);
        }
        private static EmitCreate CreateObject(Type T)
        {
            ConstructorInfo emptyConstructor = T.GetConstructor(Type.EmptyTypes);
            DynamicMethod dynamicMethod = new DynamicMethod("EmitCreate", T, Type.EmptyTypes, T);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            var delegateType = dynamicMethod.CreateDelegate(typeof(EmitCreate));

            return ((EmitCreate)delegateType);
        }


        //public static object CreateInstance(Type T, Type[] argsType, object[] args)
        //{

        //    string keyName = T.FullName + argsType.TypesToStringName();
        //    Delegate instance;
        //    ooo ii;
        //    if (!Dictionarys.IsExist(DictionaryType.CreateExpression, keyName))
        //    {
        //        Dictionarys.Lock(DictionaryType.CreateExpression);
        //        try
        //        {
        //            if (!Dictionarys.IsExist(DictionaryType.CreateExpression, keyName))
        //            {

        //                ParameterExpression[] pxpr = MakeParamsExpression(argsType);
        //                ConstructorInfo ctorInfo = MakeCtorInfo(T, argsType);
        //                NewExpression ctor = Expression.New(ctorInfo, pxpr);
        //                ii = (ooo)Expression.Lambda(typeof(ooo), ctor, pxpr).Compile();
        //                Dictionarys.Add(DictionaryType.CreateExpression, keyName, ii);

        //            }
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            Dictionarys.Unlock(DictionaryType.CreateExpression);
        //        }
        //    }

        //    ii = (ooo)Dictionarys.GetValue(DictionaryType.CreateExpression, keyName);
        //    return ii.Invoke();
        //}
    }
}
