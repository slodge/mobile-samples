using Cirrious.MvvmCross.Touch.Interfaces;

namespace MWC.iOS
{
	public interface IMWCTabBarPresenterHost
	{
		IMWCTabBarPresenter TabBarPresenter { get; set; } 
	}
	
    public interface IMWCTabBarPresenter
    {
        bool GoBack();
        bool ShowView(IMvxTouchView view);
    }
}