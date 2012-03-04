using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using MonoTouch.UIKit;

namespace MWC.iOS.UI.CustomElements
{
    public class BaseElement : Element
    {
        public BaseElement(string caption) : base(caption)
        {
        }

        protected override void UpdateCaptionDisplay(UITableViewCell cell)
        {
            // do nothing
        }
    }
}