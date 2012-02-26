using System;
using System.Net;
using Cirrious.MvvmCross.ViewModels;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Core.Mvvm.ViewModelLocators
{
    public class SingletonViewModelLocator : MvxViewModelLocator
    {
        private MainViewModel _mainViewModel;
        private MainViewModel Main
        {
            get
            {
                if (_mainViewModel == null)
                    _mainViewModel = new MainViewModel();
                return _mainViewModel;
            }
        }

        public MainViewModel GetMain()
        {
            return Main;
        }

        public TwitterViewModel GetTwitter()
        {
            return Main.Twitter;
        }

        public NewsListViewModel GetNews()
        {
            return Main.News;
        }
    }
}
