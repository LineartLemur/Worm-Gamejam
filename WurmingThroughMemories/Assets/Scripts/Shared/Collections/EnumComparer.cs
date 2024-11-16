using System;
using System.Collections.Generic;
using System.Globalization;

struct EnumComparer<TEnum> : IEqualityComparer<TEnum> 
    where TEnum : System.Enum,IConvertible
{
    static class BoxAvoidance
    {
        static readonly Func<TEnum, int> _wrapper;

        public static int ToInt(TEnum enu)
        {
            return enu.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        }
    }

    public bool Equals(TEnum firstEnum, TEnum secondEnum)
    {
        return BoxAvoidance.ToInt(firstEnum) == 
               BoxAvoidance.ToInt(secondEnum);
    }

    public int GetHashCode(TEnum firstEnum)
    {
        return BoxAvoidance.ToInt(firstEnum);
    }
}