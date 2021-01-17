using System;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public class PropertyChangeEventArgs : EventArgs, IPropertyChangeInfo
    {
        public PropertyChangeEventArgs(object oldValue, object newValue, string propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;
        }

        public PropertyChangeEventArgs(object oldValue, string propertyName)
        {
            OldValue = oldValue;
            NewValue = oldValue;
            PropertyName = propertyName;
        }

        public object OldValue { get; }
        public object NewValue { get; }
        public string PropertyName { get; }
    }
}
