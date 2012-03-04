using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Speaker element.
	/// on iPhone, pushes via MT.D
	/// on iPad, sends view to SplitViewController
	/// </summary>
    public class SpeakerElement : BaseElement
    {
		static NSString cellId = new NSString ("SpeakerElement");
		SpeakerListItemViewModel speaker;

        public SpeakerElement(SpeakerListItemViewModel speaker)
            : base(speaker.Name)
		{
            this.speaker = speaker;
		}
		
		static int count;
        protected override UITableViewCell GetCellImpl(UITableView tv)
		{
			var cell = tv.DequeueReusableCell (cellId);
			count++;
			if (cell == null)
				cell = new SpeakerCell (UITableViewCellStyle.Subtitle, cellId, speaker);
			else
				((SpeakerCell)cell).UpdateCell (speaker);

			return cell;
		}

		/// <summary>Implement MT.D search on name and company properties</summary>
		public override bool Matches (string text)
		{
			return (speaker.Name + " " + speaker.Company).ToLower ().IndexOf (text.ToLower ()) >= 0;
		}

		/// <summary>
		/// Behaves differently depending on iPhone or iPad
		/// </summary>
		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
		    speaker.ShowDetailCommand.Execute();
		}
	}
}