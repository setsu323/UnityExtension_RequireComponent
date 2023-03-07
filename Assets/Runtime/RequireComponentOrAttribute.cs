using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireComponentOrAttribute : Attribute, IRequireComponentAttribute
    {
        readonly Type l, r;

        public string Note => $"Requires only one of the following components: [{l.Name},{r.Name}]";

        public bool IsValid(Component[] components)
        {
            var cnt = 0;
            foreach (var cmp in components)
            {
                var type = cmp.GetType();
                if (type == l) cnt++;
                if (type == r) cnt++;
            }
            return cnt == 1;
        }

        public RequireComponentOrAttribute(Type l, Type r)
        {
            this.l = l;
            this.r = r;
        }
        RequireComponentOrAttribute()
        {
        }
    }
}
