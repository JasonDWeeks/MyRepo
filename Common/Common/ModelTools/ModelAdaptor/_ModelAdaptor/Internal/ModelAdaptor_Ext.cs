using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    public static partial class ModelAdaptor_Ext
    {
        #region GetModifierProperties
        public static IEnumerable<string> GetModifierProperties(this IModelAdaptor modelAdaptor)
        {
            return Enum.GetValues(typeof(ModifierProperty)).Cast<ModifierProperty>().Select(x => x.ToString());
        }
        #endregion

        #region OnPropertyChanged_Property
        public static void OnPropertyChanged_Property(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.OnPropertyChanged(propertyName);
            modelAdaptor.GetModifierProperties().ToList().ForEach(modifierPropertyName =>
            {
                modelAdaptor.OnPropertyChanged(propertyName + modifierPropertyName);
            });
        }
        #endregion
    }
}
