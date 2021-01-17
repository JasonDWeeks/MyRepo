namespace Common.ModelTools.ModelAdaptor
{
    public interface IPropertyChangeInfo
    {
        object OldValue { get; }
        object NewValue { get; }
        string PropertyName { get; }
    }
}
