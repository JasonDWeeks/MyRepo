namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public delegate TReturn Delegate_Ma_Val_Pn<T, TReturn>(IModelAdaptor modelAdaptor, T value, string propertyName);
}
