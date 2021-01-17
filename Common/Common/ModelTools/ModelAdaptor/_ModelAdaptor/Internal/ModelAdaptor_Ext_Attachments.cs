using Common.ModelTools.ModelAdaptor.Modifier;
using System.Collections.Generic;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Attachments
    {
        #region SetAsOriginal_Attachments
        public static void SetAsOriginal_Attachments(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.Attachment_SetAsOriginal_COUNT.Increment();
            modelAdaptor.Attachment_SetAsOriginal_SkipList.Add(propertyName);

            List<AttachmentInfo> list;
            modelAdaptor.AttachmentDictionary.TryGetValue(propertyName, out list);

            if (list != null)
            {
                list
                    .Where(
                        x =>
                            !modelAdaptor.Attachment_SetAsOriginal_SkipList.Contains(x.PropertyName) &&
                            (x.Mode & AttachmentMode.SetAsOriginal) != 0
                    )
                    .ToList()
                    .ForEach(x => modelAdaptor.Property(x.PropertyName).Value.SetAsOriginal());
            }

            modelAdaptor.Attachment_SetAsOriginal_COUNT.Decrement();

            if (modelAdaptor.Attachment_SetAsOriginal_COUNT <= 0)
            {
                modelAdaptor.Attachment_SetAsOriginal_COUNT.Item = 0;
                modelAdaptor.Attachment_SetAsOriginal_SkipList.Clear();
            }
        }
        #endregion

        #region Revert_Attachments
        public static void Revert_Attachments(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.Attachment_Revert_COUNT.Increment();
            modelAdaptor.Attachment_Revert_SkipList.Add(propertyName);

            List<AttachmentInfo> list;
            modelAdaptor.AttachmentDictionary.TryGetValue(propertyName, out list);
            if (list != null)
            {
                list
                    .Where(
                        x =>
                            !modelAdaptor.Attachment_Revert_SkipList.Contains(x.PropertyName) &&
                            (x.Mode & AttachmentMode.Revert) != 0
                    )
                    .ToList()
                    .ForEach(x => modelAdaptor.Property(x.PropertyName).Value.Revert());
            }

            modelAdaptor.Attachment_Revert_COUNT.Decrement();

            if (modelAdaptor.Attachment_Revert_COUNT <= 0)
            {
                modelAdaptor.Attachment_Revert_COUNT.Item = 0;
                modelAdaptor.Attachment_Revert_SkipList.Clear();
            }
        }
        #endregion

        #region OnPropertyChanged_Property_Attachments
        public static void OnPropertyChanged_Property_Attachments(this IModelAdaptor modelAdaptor, string propertyName)
        {
            List<AttachmentInfo> list;
            modelAdaptor.AttachmentDictionary.TryGetValue(propertyName, out list);
            if (list != null)
                list.Where(x => (x.Mode & AttachmentMode.Notify) != 0).ToList().ForEach(x => modelAdaptor.OnPropertyChanged_Property(x.PropertyName));
        }
        #endregion
    }
}
