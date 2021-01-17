using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Base;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public partial class TextProperty<T> : PropertyModifierBase
    {
        #region Constructors
        private TextProperty()
        {
        }

        public TextProperty(IModelAdaptor adaptor, PropertyInfo propertyInfo)
        {
            Adaptor = adaptor ?? throw new ArgumentNullException(nameof(adaptor));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
        #endregion

        #region Get
        public string Get
        {
            get
            {
                if (PropertyInfo == null)
                    return _Input;

                string text = Text;
                if (text == null)
                    text = Text_IfNull ?? Adaptor.Property(PropertyInfo.Name).Value.Get?.ToString();
                return text;
            }
        }
        #endregion

        #region Set
        public string Set
        {
            set
            {
                Text = value;
                Adaptor.OnPropertyChanged(PropertyInfo.Name + nameof(ModifierProperty._Text));
                if (Has_UpdateSource)
                    Adaptor.Property(PropertyInfo.Name).Value.Set = UpdateSource(Adaptor, value, PropertyInfo.Name);
            }
        }
        #endregion

        #region Text
        private string Text
        {
            get
            {
                string text;
                if (Adaptor.GetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._Text), out text))
                    return text;


                Adaptor.TextDictionary.TryGetValue(PropertyInfo.Name, out text);
                Adaptor.SetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._Text), text);

                return text;
            }
            set
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                Adaptor.TextDictionary[PropertyInfo.Name] = value;
            }
        }
        #endregion

        #region Text_IfNull
        private string Text_IfNull
        {
            get
            {
                string text;
                if (Adaptor.GetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._Text) + nameof(Text_IfNull), out text))
                    return text;

                text = IfNull?.Invoke(Adaptor, PropertyInfo.GetValue(Adaptor.Model), PropertyInfo.Name);
                Adaptor.SetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._Text), text);

                return text;
            }
        }
        #endregion

        #region Has_IfNull
        public bool Has_IfNull
        {
            get
            {
                return IfNull != null;
            }
        }
        #endregion

        #region Set_IfNull
        public void Set_IfNull(Delegate_Ma_Val_Pn<T, string> ifNullDelegate)
        {
            if (ifNullDelegate == null)
                throw new ArgumentNullException(nameof(ifNullDelegate));

            IfNull = (a, p, pn) => ifNullDelegate(a, (T)p, pn);
        }
        #endregion

        #region Remove_IfNull
        public void Remove_IfNull()
        {
            IfNull = null;
        }
        #endregion

        #region IfNull
        private Delegate_Ma_Val_Pn<object, string> IfNull
        {
            get
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                Delegate_Ma_Val_Pn<object, string> ifNullDelegate;
                Adaptor.Text_IfNullDictionary.TryGetValue(PropertyInfo.Name, out ifNullDelegate);
                return ifNullDelegate;
            }
            set
            {
                if (value == null)
                    Adaptor.Text_IfNullDictionary.Remove(PropertyInfo.Name);
                else
                    Adaptor.Text_IfNullDictionary[PropertyInfo.Name] = value;
            }
        }
        #endregion

        #region Has_UpdateSource
        public bool Has_UpdateSource
        {
            get
            {
                return UpdateSource != null;
            }
        }
        #endregion

        #region Set_UpdateSource
        public void Set_UpdateSource(Delegate_Ma_Val_Pn<string, T> updateSourceDelegate)
        {
            UpdateSource = (a, p, pn) => updateSourceDelegate(a, p, pn);
        }
        #endregion

        #region Remove_UpdateSource
        public void Remove_UpdateSource()
        {
            UpdateSource = null;
        }
        #endregion

        #region UpdateSource
        private Delegate_Ma_Val_Pn<string, object> UpdateSource
        {
            get
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                Delegate_Ma_Val_Pn<string, object> updateSourceDelegate;
                Adaptor.Text_UpdateSourceDictionary.TryGetValue(PropertyInfo.Name, out updateSourceDelegate);
                return updateSourceDelegate;
            }
            set
            {
                if (PropertyInfo == null)
                    throw new InvalidOperationException();

                if (value == null)
                    Adaptor.Text_UpdateSourceDictionary.Remove(PropertyInfo.Name);
                else
                    Adaptor.Text_UpdateSourceDictionary[PropertyInfo.Name] = value;
            }
        }
        #endregion

        #region Clear
        public void Clear()
        {
            if (Has_IfNull)
                Text = null;
        }
        #endregion

        #region Cast
        public static implicit operator string(TextProperty<T> textProp)
        {
            if (textProp == null)
                return null;
            return textProp.Get;
        }

        public static implicit operator TextProperty<T>(string str)
        {
            var textProp = new TextProperty<T>();
            textProp._Input = str;
            return textProp;
        }
        #endregion

        #region Private
        private string _Input;
        #endregion
    }
}
