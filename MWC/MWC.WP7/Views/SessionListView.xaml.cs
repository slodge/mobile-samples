using System;
using System.Windows.Controls;
using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.WP7.Views
{
    public class BaseSessionListView : MvxPhonePage<SessionListViewModel>
    {        
    }

    public partial class SessionListView : BaseSessionListView
    {
        public SessionListView()
        {
            InitializeComponent ();
        }
    }
}
