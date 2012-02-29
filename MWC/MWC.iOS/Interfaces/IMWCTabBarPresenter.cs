using Cirrious.MvvmCross.Touch.Interfaces;

namespace MWC.iOS.Interfaces
{
    public interface IMWCTabBarPresenter
    {
        bool GoBack();
        bool ShowView(IMvxTouchView view);
    }
}