using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.ModelTools.ModelAdaptor
{
    public class DelegateConverter<TDelgate, TResult>
        where TDelgate : MulticastDelegate
    {
        private DelegateConverter()
        {}

        public DelegateConverter(params object[] args)
        {
            Arguments = args;
        }

        private TDelgate Get_Delegate { get; set; }
        private object[] Arguments { get; set; }
        public void SetArguments(params object[] args)
        {
            Arguments = args;
        }

        public void Set(TDelgate func)
        {
            Get_Delegate = func;
        }

        public TResult Get
        {
            get
            {
                return (TResult)(Get_Delegate?.DynamicInvoke(Arguments) ?? default(TResult));
            }
        }

        public override string ToString()
        {
            if (typeof(TResult) == typeof(string))
                return this.Get as string;
            return base.ToString();
        }

        #region Cast
        public static implicit operator TResult(DelegateConverter<TDelgate, TResult> conveter)
        {
            if (conveter == null)
                return default(TResult);
            return conveter.Get;
        }

        public static implicit operator DelegateConverter<TDelgate, TResult>(TResult value)
        {
            var converter = new DelegateConverter<TDelgate, TResult>();

            var type = typeof(TDelgate);
            var paramters = new List<ParameterExpression>();

            int count = 1;
            foreach (var gType in type.GenericTypeArguments)
            {
                if (count != type.GenericTypeArguments.Count() || !type.Name.Contains("Func"))
                    paramters.Add(Expression.Parameter(gType, count.ToString()));
                count++;
            }            

            converter.Get_Delegate = Expression.Lambda<TDelgate>(Expression.Constant(value), paramters).Compile();
            return converter;
        }

        public static implicit operator DelegateConverter<TDelgate, TResult>(TDelgate delgate)
        {
            var converter = new DelegateConverter<TDelgate, TResult>();
            converter.Get_Delegate = delgate;
            return converter;
        }

        public static implicit operator TDelgate(DelegateConverter<TDelgate, TResult> conveter)
        {
            return conveter?.Get_Delegate;
        }
        #endregion
    }
}
