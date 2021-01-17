namespace Common.ModelTools.ModelAdaptor
{
    public enum AttachmentMode
    {
        Notify = 2,
        Revert = 4,
        SetAsOriginal = 8,
        All = Notify | Revert | SetAsOriginal
    }
}
