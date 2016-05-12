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

        private static bool CheckIsStaticType(Type T)
        {
            if (T.GetConstructor(Type.EmptyTypes) == null && T.IsAbstract && T.IsSealed)
            {
                return true;
            }
            return false;
        }

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
