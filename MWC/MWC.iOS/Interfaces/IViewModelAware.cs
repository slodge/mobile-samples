using Cirrious.MvvmCross.Touch.Interfaces;

namespace MWC.iOS.Interfaces
{
    public interface IViewModelAware
    {
        bool CanShow(IMvxTouchView view);
    }
}