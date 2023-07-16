using Microsoft.Toolkit.Mvvm.ComponentModel;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace Market.ViewModel
{
    public partial class ViewModelBase : ObservableObject
    {
        [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        public bool isBusy;

        [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
        public string title;

        public bool IsNotBusy => !IsBusy;
    }
}
