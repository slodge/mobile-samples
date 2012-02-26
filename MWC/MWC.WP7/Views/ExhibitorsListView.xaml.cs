using System;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows.Controls;
using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.Core.Mvvm.ViewModels;
using MWC.BL.Managers;

namespace MWC.WP7.Views
{
    public partial class BaseExhibitorsListView : MvxPhonePage<ExhibitorsListViewModel>
    {}

    public partial class ExhibitorsListView : BaseExhibitorsListView
    {
        public ExhibitorsListView()
        {
            InitializeComponent ();
        }
    }
}
