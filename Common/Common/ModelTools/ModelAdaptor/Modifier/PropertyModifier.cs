using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Base;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using Common.ModelTools.ModelAdaptor;
using System.Linq;
using System.Linq.Expressions;
using Common.ModelTools.ModelAdaptor.Internal;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public partial class PropertyModifier<T, PT> : PropertyModifierBase
        where PT : class, new()
    {
        #region Constructor
        public PropertyModifier(IModelAdaptor adaptor, PropertyInfo propertyInfo)
        {
            Adaptor = adaptor ?? throw new ArgumentNullException(nameof(adaptor));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
        #endregion

        #region IsEdited
        private IsEditedProperty<T> _IsEdited;
        public IsEditedProperty<T> IsEdited
        {
            get
            {
                if (_IsEdited == null)
                    _IsEdited = new IsEditedProperty<T>(Adaptor, PropertyInfo);
                return _IsEdited;
            }
        }
        #endregion

        #region IsValid
        private IsValidProperty<T> _IsValid;
        public IsValidProperty<T> IsValid
        {
            get
            {
                if (_IsValid == null)
                    _IsValid = new IsValidProperty<T>(Adaptor, PropertyInfo);
                return _IsValid;
            }
        }
        #endregion

        #region Value
        private ValueProperty<T> _Value;
        public ValueProperty<T> Value
        {
            get
            {
                if (_Value == null)
                    _Value = new ValueProperty<T>(Adaptor, PropertyInfo);
                return _Value;
            }
            set
            {
                if (value == null)
                    Value.Set = default(T);
                else
                    Value.Set = value.Get;
            }
        }
        #endregion

        #region Text
        private TextProperty<T> _Text;
        public TextProperty<T> Text
        {
            get
            {
                if (_Text == null)
                    _Text = new TextProperty<T>(Adaptor, PropertyInfo);
                return _Text;
            }
            set
            {
                if (value == null)
                    Text.Set = null;
                else
                    Text.Set = value.Get;
            }
        }
        #endregion

        #region Label
        private ModifierDictionaryConverter<Delegate_Ma_Pn<string>, string> _Label;
        public ModifierDictionaryConverter<Delegate_Ma_Pn<string>, string> Label
        {
            get
            {
                if (_Label == null)
                    _Label = new ModifierDictionaryConverter<Delegate_Ma_Pn<string>, string>(
                        Adaptor.LabelDictionary.ToDictionary(),
                        Adaptor,
                        PropertyInfo,
                        nameof(ModifierProperty._Label),
                        Adaptor,
                        PropertyInfo?.Name
                    );
                return _Label;
            }
            set
            {
                if (value == null)
                    Label.Set(null);
                else
                    Label.Set(value.Input);
            }
        }
        #endregion

        #region Message
        private ModifierDictionaryConverter<Delegate_Ma_Pn<string>, string> _Message;
        public ModifierDictionaryConverter<Delegate_Ma_Pn<string>, string> Message
        {
            get
            {
                if (_Message == null)
                    _Message = new ModifierDictionaryConverter<Delegate_Ma_Pn<string>, string>(
                        Adaptor.MessageDictionary.ToDictionary(),
                        Adaptor, 
                        PropertyInfo,
                        nameof(ModifierProperty._Message),
                        Adaptor,
                        PropertyInfo?.Name
                    );
                return _Message;
            }
            set
            {
                if (value == null)
                    Message.Set(null);
                else
                    Message.Set(value.Input);
            }
        }
        #endregion

        #region ReadOnly
        private ModifierDictionaryConverter<Delegate_Ma_Pn<bool>, bool> _ReadOnly;
        public ModifierDictionaryConverter<Delegate_Ma_Pn<bool>, bool> ReadOnly
        {
            get
            {
                if (_ReadOnly == null)
                    _ReadOnly = new ModifierDictionaryConverter<Delegate_Ma_Pn<bool>, bool>(
                        Adaptor.ReadOnlyDictionary.ToDictionary(),
                        Adaptor,
                        PropertyInfo,
                        nameof(ModifierProperty._ReadOnly),
                        Adaptor,
                        PropertyInfo?.Name
                    );
                return _ReadOnly;
            }
            set
            {
                if (value == null)
                    ReadOnly.Set(null);
                else
                    ReadOnly.Set(value.Input);
            }
        }
        #endregion

        #region Activate
        public void Activate()
        {
            if (!Adaptor.ActiveModelProperties.Any(x => x == PropertyInfo.Name))
                Adaptor.ActiveModelProperties.Add(PropertyInfo.Name);
        }
        #endregion

        #region ForceNotify
        public void ForceNotify()
        {
            Value.Changed_Raise();
            Adaptor.OnPropertyChanged_DynamicProperty(PropertyInfo.Name);
        }
        #endregion

        #region Attachment

        #region Attach
        public void Attach(AttachmentMode mode, params Expression<Func<PT, object>>[] properties)
        {
            _AddProperties(mode, properties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null));
        }

        public void Attach(params Expression<Func<PT, object>>[] properties)
        {
            _AddProperties(AttachmentMode.Notify, properties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null));
        }

        public void Detach(params Expression<Func<PT, object>>[] properties)
        {
            _RemoveProperties(properties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null));
        }

        public void Attach(AttachmentMode mode, params string[] properties)
        {
            _AddProperties(mode, properties);
        }

        public void Attach(params string[] properties)
        {
            _AddProperties(AttachmentMode.Notify, properties);
        }

        public void Detach(params string[] properties)
        {
            _RemoveProperties(properties);
        }

        public void ClearAttachments()
        {
            Attachments.Clear();
        }
        #endregion

        #region Couple
        public void Couple(AttachmentMode mode, params Expression<Func<PT, object>>[] properties)
        {
            _AddProperties(mode, properties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null), true);
        }

        public void Couple(params Expression<Func<PT, object>>[] properties)
        {
            _AddProperties(AttachmentMode.All, properties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null), true);
        }

        public void Decouple(params Expression<Func<PT, object>>[] properties)
        {
            _RemoveProperties(properties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null), true);
        }

        public void Couple(AttachmentMode mode, params string[] properties)
        {
            _AddProperties(mode, properties, true);
        }

        public void Couple(params string[] properties)
        {
            _AddProperties(AttachmentMode.All, properties, true);
        }

        public void Decouple(params string[] properties)
        {
            _RemoveProperties(properties, true);
        }

        public void ClearCouplings()
        {
            Attachments.Clear();
            foreach (var pair in Adaptor.AttachmentDictionary.Where(x => x.Value != null && x.Value.Select(y => y.PropertyName).Contains(PropertyInfo.Name)))
            {
                pair.Value.RemoveAll(x => x.PropertyName == PropertyInfo.Name);
            }
        }
        #endregion

        #region Private
        private List<AttachmentInfo> Attachments
        {
            get
            {
                List<AttachmentInfo> attachments;
                Adaptor.AttachmentDictionary.TryGetValue(PropertyInfo.Name, out attachments);
                if (attachments == null)
                {
                    attachments = new List<AttachmentInfo>();
                    Adaptor.AttachmentDictionary[PropertyInfo.Name] = attachments;
                }
                return attachments;
            }
        }
        private void _AddProperties(AttachmentMode mode, IEnumerable<string> properties, bool couple = false)
        {
            foreach (var prop in properties)
            {
                if (!Attachments.Select(x => x.PropertyName).Contains(prop) && prop != PropertyInfo.Name)
                    Attachments.Add(new AttachmentInfo { PropertyName = prop, Mode = mode });
                if (couple)
                {
                    Adaptor.Property(prop).Attach(
                        mode,
                        properties
                            .Except(new[] { prop })
                            .Concat(new[] { PropertyInfo.Name })
                            .ToArray()
                   );
                }
            }
        }

        private void _RemoveProperties(IEnumerable<string> properties, bool couple = false)
        {
            foreach (var prop in properties)
            {
                if (Attachments.Select(x => x.PropertyName).Contains(prop))
                    Attachments.RemoveAll(x => x.PropertyName == prop);
                if (couple)
                {
                    Adaptor.Property(prop).Detach(
                        properties
                            .Except(new[] { prop })
                            .Concat(new[] { PropertyInfo.Name })
                            .ToArray()
                    );
                }
            }
        }
        #endregion

        #endregion
    }
}
