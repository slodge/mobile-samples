using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class SpeakerDetailsViewModel : ViewModelBase
    {
        public SpeakerDetailsViewModel(string id=null, string key=null)
        {
            var speaker = default(Speaker);

            if (id != null)
            {
                speaker = SpeakerManager.GetSpeaker(int.Parse(id));
            }
            else if (key != null)
            {
                speaker = SpeakerManager.GetSpeakerWithKey(key);
            }

            if (speaker != null)
            {
                Update(speaker);
            }
        }

        public int ID { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string ImageUrl { get; set; }
        public string Bio { get; set; }

        public void Update (Speaker speaker)
        {
            ID = speaker.ID;
            Key = speaker.Key;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;
            Bio = CleanupPlainTextDocument (speaker.Bio);

            if (string.IsNullOrWhiteSpace (Bio)) {
                Bio = "No biographical information available.";
            }
        }
    }
}
