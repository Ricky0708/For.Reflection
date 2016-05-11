using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal enum CacheType
{
    Type,
    Create,
    MethodInfo,
    MethodCall,
    SetFieldValue,
    GetFieldValue,
    SetPropertyValue,
    GetPropertyValue
}

