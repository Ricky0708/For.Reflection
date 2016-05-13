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
        /// make ctor info for create instance
        /// </summary>
        /// <param name="T">type of instance</param>
        /// <param name="types">types of ctor args type, if no args give it null</param>
        /// <returns></returns>
        private static ConstructorInfo MakeCtorInfo(Type T, params Type[] types)
        {
            ConstructorInfo result;
            if (types != null)
            {
                result = T.GetConstructor(types);
            }
            else
            {
                result = T.GetConstructor(new Type[] { });
            }

            if (result == null)
            {
                throw new ArgumentOutOfRangeException("Can't find Constructor");
            }
            return result;
        }

        /// <summary>
        /// now use know
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private static ParameterExpression[] MakeParamsExpression(params Type[] types)
        {
            ParameterExpression[] pxpr = new ParameterExpression[] { };
            if (types != null)
            {
                pxpr = new ParameterExpression[types.Count()];
                int i = 0;
                foreach (var obj in types)
                {
                    pxpr[i] = Expression.Parameter(obj, obj.Name);
                    i++;
                }
            }
            return pxpr;
        }

        /// <summary>
        /// check type is static, if yes, this type can not be create or method call by instance
        /// </summary>
        /// <param name="T">instance type</param>
        /// <returns></returns>
        private static bool CheckIsStaticType(Type T)
        {
            if (T.GetConstructor(Type.EmptyTypes) == null && T.IsAbstract && T.IsSealed)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// convert expression type to real type to expression for delegate useage
        /// </summary>
        /// <param name="pxpr">empty object[] expresstion</param>
        /// <param name="paramsInfo">real args paramsInfo</param>
        /// <returns>args[] Expression</returns>
        private static Expression[] ConvertParasInfoToExpr(ParameterExpression pxpr, ParameterInfo[] paramsInfo)
        {
            Expression[] argsExp = new Expression[paramsInfo.Length];
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(pxpr, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }
            return argsExp;
        }

    }
}
