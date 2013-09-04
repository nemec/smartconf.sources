using System;
using System.Linq;
using System.Reflection;

namespace Environment
{
    internal static class Net45Extensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return GetCustomAttribute<T>(member, false);
        }

        public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit) 
            where T : Attribute
        {
            return (T)member.GetCustomAttributes(typeof (T), inherit)
                .FirstOrDefault();
        }
    }
}
