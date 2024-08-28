using System;
using System.Linq;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireComponentAnyAttribute : Attribute, IRequireComponentAttribute
    {
        readonly Type[] types;

        public string Note => $"Requires one of the following components: [{string.Join(",", types.Select(x => x.Name))}]";

        public bool IsValid(Component[] components)
        {
            return components.Select(x => x.GetType()).Any(x => types.Any(y => x == y));
        }

        public RequireComponentAnyAttribute(Type type1, Type type2)
        {
            types = new Type[] { type1, type2, };
        }
        public RequireComponentAnyAttribute(Type type1, Type type2, Type type3)
        {
            types = new Type[] { type1, type2, type3, };
        }
        RequireComponentAnyAttribute()
        {
        }
    }
}
