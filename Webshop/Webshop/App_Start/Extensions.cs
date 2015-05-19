using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.App_Start
{
    public static class  Extensions
    {
        public static T GetObject<T>(this Dictionary<string, object> dict)
        {
            try
            {
                Type type = typeof(T);
                var obj = Activator.CreateInstance(type);

                foreach (var kv in dict)
                {
                    type.GetProperty(kv.Key).SetValue(obj, kv.Value ?? GetDefaultValue(type.GetProperty(kv.Key).PropertyType));
                }
                return (T)obj;
            }
            catch
            {
                throw new Exception("Table variables and class variables does not match");
            }
           // return default(T);
        }

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                propertyInfo.SetValue(x, value, null);
            }
            return (T)x;
        } 
    }
}
