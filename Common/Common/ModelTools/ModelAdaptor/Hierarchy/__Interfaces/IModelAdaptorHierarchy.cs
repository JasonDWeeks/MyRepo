using System.ComponentModel;
using System.Windows.Input;

namespace Common.ModelTools.ModelAdaptor.Hierarchy
{
    public interface IModelAdaptorHierarchy : INotifyPropertyChanged
    {
        object Parent { get; set; }
        IModelAdaptor Adaptor { get; }
        dynamic Self { get; }

        ICommand SaveCommand { get; }
        ICommand RevertCommand { get; }

        void Save();
        void Revert();
    }
}
