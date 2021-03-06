using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;

namespace MWC.iOS.Screens.Common.Speakers {
#if false
	/// <summary>
	/// Speaker details implemented with UIKit and XIB file
	/// </summary>
	[Obsolete("See MT.D implementation in iPhone folder; although this may be re-instated later")]
	public partial class SpeakersScreen : UIViewController {

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SpeakersScreen ()
			: base (UserInterfaceIdiomIsPhone ? "SpeakersScreen_iPhone" : "SpeakersScreen_iPad", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			//any additional setup after loading the view, typically from a nib.
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
	}
#endif
}