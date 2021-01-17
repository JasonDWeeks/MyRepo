using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Base;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public partial class ValueProperty<T> : PropertyModifierBase
    {
        #region Contructor
        private ValueProperty()
        {
        }

        public ValueProperty(IModelAdaptor adaptor, PropertyInfo propertyInfo)
        {
            Adaptor = adaptor ?? throw new ArgumentNullException(nameof(adaptor));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

            EventHandler<PropertyChangeEventArgs> changing;
            Adaptor.Value_ChangingDictionary.TryGetValue(PropertyInfo.Name, out changing);
            if (changing == null)
            {
                Adaptor.Value_ChangingDictionary[PropertyInfo.Name] = _placeHolder;
                changing = Adaptor.Value_ChangingDictionary[PropertyInfo.Name];
            }
            changing.GetInvocationList().ToList().ForEach(x =>
            {
                _Changing += (EventHandler<PropertyChangeEventArgs>)x;
            });
            Adaptor.Value_ChangingDictionary[PropertyInfo.Name] -= _placeHolder;
            _Changing -= _placeHolder;

            EventHandler<PropertyChangeEventArgs> changed;
            Adaptor.Value_ChangedDictionary.TryGetValue(PropertyInfo.Name, out changed);
            if (changed == null)
            {
                Adaptor.Value_ChangedDictionary[PropertyInfo.Name] = _placeHolder;
                changed = Adaptor.Value_ChangedDictionary[PropertyInfo.Name];
            }
            changed.GetInvocationList().ToList().ForEach(x =>
            {
                _Changed += (EventHandler<PropertyChangeEventArgs>)x;
            }); ;
            Adaptor.Value_ChangedDictionary[PropertyInfo.Name] -= _placeHolder;
            _Changed -= _placeHolder;
        }

        private void _placeHolder(object source, PropertyChangeEventArgs arg)
        { }
        #endregion

        #region Get
        public T Get
        {
            get
            {
                if (PropertyInfo == null)
                    return _Input;

                T value;
                if (Adaptor.GetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._Value), out value))
                    return value;

                object val;
                Adaptor.GetMember(PropertyInfo.Name, out val);
                Adaptor.SetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._Value), val);
                return (T)val;
            }
        }
        #endregion

        #region Get_Original
        public T Get_Original
        {
            get
            {
                if (PropertyInfo == null)
                    return _Input;

                return (T)Adaptor.ModelProperties.GetByName(PropertyInfo.Name).GetValue(Adaptor.Original.Item);
            }
        }
        #endregion

        #region Set
        public T Set
        {
            set
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                Adaptor.SetMember(PropertyInfo.Name, value);
            }
        }
        #endregion

        #region Changed
        private EventHandler<PropertyChangeEventArgs> _Changed;
        public event EventHandler<PropertyChangeEventArgs> Changed
        {
            add
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                _Changed += value;
                Adaptor.Value_ChangedDictionary[PropertyInfo.Name] += value;
            }
            remove
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                _Changed -= value;
                Adaptor.Value_ChangedDictionary[PropertyInfo.Name] -= value;
            }
        }
        #endregion

        #region Changed_Raise
        internal void Changed_Raise()
        {
            if (PropertyInfo == null)
                throw new InvalidOperationException();

            var args = new PropertyChangeEventArgs(Get_Original, Get, PropertyInfo.Name);
            _Changed?.Invoke(Adaptor, args);
        }
        #endregion

        #region Changing
        private EventHandler<PropertyChangeEventArgs> _Changing;
        public event EventHandler<PropertyChangeEventArgs> Changing
        {
            add
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                _Changing += value;
                Adaptor.Value_ChangingDictionary[PropertyInfo.Name] += value;
            }
            remove
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                _Changing -= value;
                Adaptor.Value_ChangingDictionary[PropertyInfo.Name] -= value;
            }
        }
        #endregion

        #region Changing_Raise
        internal void Changing_Raise()
        {
            if (PropertyInfo == null)
                throw new InvalidOperationException();

            var args = new PropertyChangeEventArgs(Get_Original, Get, PropertyInfo.Name);
            _Changing?.Invoke(Adaptor, args);
        }
        #endregion

        #region Revert_Command
        private Command _Revert_Command;
        public ICommand Revert_Command
        {
            get
            {
                if (_Revert_Command == null)
                    _Revert_Command = new Command(Revert);
                return _Revert_Command;
            }
        }
        #endregion

        #region Revert
        public void Revert()
        {
            if (PropertyInfo == null)
                throw new InvalidOperationException();

            Adaptor.Value_DuringRevertDictionary[PropertyInfo.Name] = true;

            Adaptor.Property(PropertyInfo.Name).Text.Clear();
            Set = Get_Original;
            Adaptor.Revert_Attachments(PropertyInfo.Name);

            Adaptor.Value_DuringRevertDictionary[PropertyInfo.Name] = false;
        }
        #endregion

        #region DuringPropertyRevert
        public bool DuringPropertyRevert
        {
            get
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                bool ret;
                Adaptor.Value_DuringRevertDictionary.TryGetValue(PropertyInfo.Name, out ret);
                return ret;
            }
        }
        #endregion

        #region SetAsOriginal_Command
        private Command _SetAsOriginal_Command;
        public ICommand SetAsOriginal_Command
        {
            get
            {
                if (_SetAsOriginal_Command == null)
                    _SetAsOriginal_Command = new Command(SetAsOriginal);
                return _SetAsOriginal_Command;
            }
        }
        #endregion

        #region SetAsOriginal
        public void SetAsOriginal()
        {
            if (PropertyInfo == null)
                throw new InvalidOperationException();

            Adaptor.Original.GetType().GetProperty(PropertyInfo.Name).SetValue(Adaptor.Original, Get);
            Set = Get;
            Adaptor.SetAsOriginal_Attachments(PropertyInfo.Name);
        }
        #endregion

        #region DuringPropertySetAsOriginal
        public bool DuringPropertySetAsOriginal
        {
            get
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();
                bool ret;
                Adaptor.Value_DuringSetAsOriginalDictionary.TryGetValue(PropertyInfo.Name, out ret);
                return ret;
            }
        }
        #endregion

        #region Cast
        public static implicit operator T(ValueProperty<T> valueProp)
        {
            if (valueProp == null)
                return default(T);
            return valueProp.Get;
        }

        public static implicit operator ValueProperty<T>(T value)
        {
            var valueProp = new ValueProperty<T>();
            valueProp._Input = value;
            return valueProp;
        }
        #endregion

        #region Private Fields
        private T _Input;
        #endregion
    }
}
