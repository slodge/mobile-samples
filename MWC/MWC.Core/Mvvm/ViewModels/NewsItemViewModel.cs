using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class NewsItemViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public string Content { get; set; }

        public NewsItemViewModel (string id)
        {
            var item = NewsManager.Get(int.Parse(id));
            Update(item);
        }

        public NewsItemViewModel (RSSEntry item)
        {
            Update (item);
        }

        public void Update (RSSEntry item)
        {
            ID = item.ID;
            Url = item.Url;
            Title = item.Title;
            Published = item.Published;
            Content = item.Content;
        }


        public IMvxCommand ShowDetailCommand
        {
            get { return new MvxRelayCommand(() => this.RequestNavigate<NewsItemViewModel>(new { id = ID })); }
        }
    }
}
