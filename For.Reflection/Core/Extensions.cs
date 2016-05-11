using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace For.Reflection
{
    internal static class Extensions
    {
        internal static string TypesToStringName(this Type[] typs)
        {
            string result = "";
            if (typs != null)
            {
                foreach (var type in typs)
                {
                    result += type.Name;
                }
            }
            return result;
        }
    }
}
