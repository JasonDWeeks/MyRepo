using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public class ModifierDictionaryConverter<TDelgate, TResult>
        where TDelgate : MulticastDelegate
    {
        private ModifierDictionaryConverter()
        { }

        public ModifierDictionaryConverter(Dictionary<string, DelegateConverter<TDelgate, TResult>> dictionary, IModelAdaptor adaptor, PropertyInfo propertyInfo, string modifierPropertyName, params object[] args)
        {
            Dictionary = dictionary;
            Adaptor = adaptor;
            PropertyInfo = propertyInfo;
            ModifierPropertyName = modifierPropertyName;
            Arguments = args;
        }

        public string ModifierPropertyName { get; }
        internal IModelAdaptor Adaptor { get; set; }
        internal PropertyInfo PropertyInfo { get; set; }
        internal DelegateConverter<TDelgate, TResult> Input { get; set; }
        private Dictionary<string, DelegateConverter<TDelgate, TResult>> Dictionary { get; set; }

        private object[] Arguments { get; set; }

        public TResult Get
        {
            get
            {
                if (PropertyInfo == null)
                    return default(TResult);

                TResult value;
                if (Adaptor.GetCacheValue(PropertyInfo.Name, ModifierPropertyName, out value))
                    return value;

                DelegateConverter<TDelgate, TResult> ret = null;
                Dictionary?.TryGetValue(PropertyInfo.Name, out ret);

                value = ret ?? Input;
                Adaptor.SetCacheValue(PropertyInfo.Name, ModifierPropertyName, value);
                return value;
            }
        }

        public void Set(TDelgate delegateObj)
        {
            var converter = new DelegateConverter<TDelgate, TResult>(Arguments);
            converter.Set(delegateObj);
            Input = converter;
            if (Dictionary != null && PropertyInfo != null && Adaptor != null)
            {
                Dictionary[PropertyInfo.Name] = converter;
                Adaptor.OnPropertyChanged(PropertyInfo.Name + ModifierPropertyName);
            }
        }

        public override string ToString()
        {
            if (typeof(TResult) == typeof(string))
                return this.Get as string;
            return base.ToString();
        }

        #region Cast
        public static implicit operator TResult(ModifierDictionaryConverter<TDelgate, TResult> conveter)
        {
            if (conveter == null)
                return default(TResult);
            return conveter.Get;
        }

        public static implicit operator ModifierDictionaryConverter<TDelgate, TResult>(TResult value)
        {
            var converter = new ModifierDictionaryConverter<TDelgate, TResult>();

            var type = typeof(TDelgate);

            var method = type.GetMethod("Invoke");

            var parameters = method.GetParameters().ToList()
                .Select(
                    param =>
                        Expression.Parameter(param.ParameterType, param.Name) as ParameterExpression
                )
            .ToList();

            var lambda = Expression.Lambda<TDelgate>(Expression.Constant(value), parameters);
            converter.Set(lambda.Compile());
            return converter;
        }

        public static implicit operator ModifierDictionaryConverter<TDelgate, TResult>(TDelgate delgate)
        {
            var converter = new ModifierDictionaryConverter<TDelgate, TResult>();
            converter.Set(delgate);
            return converter;
        }
        #endregion
    }

}
