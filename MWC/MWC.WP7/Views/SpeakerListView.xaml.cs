using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.WP7.Views
{
    public partial class BaseSpeakerListView : MvxPhonePage<SpeakerListViewModel>
    {
        
    }
    public partial class SpeakerListView : BaseSpeakerListView
    {
        public SpeakerListView()
        {
            InitializeComponent ();
        }
    }
}
