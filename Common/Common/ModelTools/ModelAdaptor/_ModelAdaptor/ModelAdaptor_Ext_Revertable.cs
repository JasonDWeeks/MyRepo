using Common.ModelTools.ModelAdaptor.Internal;
using System;

namespace Common.ModelTools.ModelAdaptor
{
    public static partial class ModelAdaptor_Ext_Revertable
    {
        #region SetAsOriginal
        public static void SetAsOriginal(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            modelAdaptor.DuringSetAsOriginal.Item = true;

            modelAdaptor.Original.Item = Activator.CreateInstance(modelAdaptor.Original.Item.GetType());

            QuickMapper.QuickMapper.Map(modelAdaptor.Model, modelAdaptor.Original.Item, modelAdaptor.ExcludeModelProperties.ToArray());

            modelAdaptor.OnPropertyChanged_Properties_Active();
            modelAdaptor.OnPropertyChanged_IsEdited_IsValid();
            modelAdaptor.OnPropertyChanged_Extensions_Active();
            modelAdaptor.StateModified_Raise(null);

            modelAdaptor.DuringSetAsOriginal.Item = false;
        }
        #endregion

        #region Revert
        public static void Revert(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            modelAdaptor.DuringRevert.Item = true;

            QuickMapper.QuickMapper.MapTo(modelAdaptor.Original.Item, modelAdaptor.Model, modelAdaptor.ExcludeModelProperties.ToArray());

            modelAdaptor.ClearText_Properties_Active();
            modelAdaptor.OnPropertyChanged_Properties_Active();
            modelAdaptor.OnPropertyChanged_IsEdited_IsValid();
            modelAdaptor.OnPropertyChanged_Extensions_Active();
            modelAdaptor.StateModified_Raise(null);

            modelAdaptor.DuringRevert.Item = false;
        }
        #endregion
    }
}
