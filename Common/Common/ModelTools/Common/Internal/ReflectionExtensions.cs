using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.ModelTools.Common.Internal
{
    static class ReflectionExtensions
    {
        private static BindingFlags _flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static T GetMethod<T>(this object source, string name)
            where T : MulticastDelegate
        {
            var type = typeof(T);
            var paramters = new List<ParameterExpression>();
            int count = 1;
            foreach (var gType in type.GenericTypeArguments)
            {
                if (count != type.GenericTypeArguments.Count() || !type.Name.Contains("Func"))
                    paramters.Add(Expression.Parameter(gType, count.ToString()));
                count++;
            }

            var methodInfo = source.GetType().GetMethod(name, _flag);
            var constant = Expression.Constant(source);
            var methodCall = Expression.Call(constant, methodInfo, paramters);

            return Expression.Lambda<T>(methodCall, paramters).Compile();
        }

        public static void RaiseEvent(this object source, string eventName, object eventArgs = null)
        {
            var eventDelegate = (MulticastDelegate)source.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(source);
            if (eventDelegate != null)
            {
                foreach (var handler in eventDelegate.GetInvocationList())
                {
                    handler.Method.Invoke(handler.Target, new object[] { source, eventArgs ?? EventArgs.Empty });
                }
            }
        }

        public static void OnPropertyChanged(this INotifyPropertyChanged source, string propertyName)
        {
            source.RaiseEvent(nameof(source.PropertyChanged), new PropertyChangedEventArgs(propertyName));
        }

        public static PropertyInfo GetByName(this IEnumerable<PropertyInfo> items, string name)
        {
            return items.Where(x => x.Name == name).FirstOrDefault();
        }
    }
}
