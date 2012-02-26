using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class ExhibitorDetailsViewModel : ViewModelBase
    {
        public ExhibitorDetailsViewModel(string id = null, string name = null)
        {
            var exhibitor = default(Exhibitor);

            if (id != null)
            {
                exhibitor = ExhibitorManager.GetExhibitor(int.Parse(id));
            }
            else if (name != null)
            {
                exhibitor = ExhibitorManager.GetExhibitorWithName(name);
            }

            if (exhibitor != null)
            {
                Update(exhibitor);
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Locations { get; set; }
        public bool IsFeatured { get; set; }
        public string Overview { get; set; }
        public string Tags { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ImageUrl { get; set; }

        public void Update (Exhibitor exhibitor)
        {
            ID = exhibitor.ID;
            Name = exhibitor.Name;
            City = exhibitor.City;
            Country = exhibitor.Country;
            Locations = exhibitor.Locations;
            IsFeatured = exhibitor.IsFeatured;
            Overview = CleanupPlainTextDocument (exhibitor.Overview);
            Tags = exhibitor.Tags;
            Email = exhibitor.Email;
            Address = exhibitor.Address;
            Phone = exhibitor.Phone;
            Fax = exhibitor.Fax;
            ImageUrl = exhibitor.ImageUrl;

            if (string.IsNullOrWhiteSpace (Overview)) {
                Overview = "No background information available.";
            }
        }
    }
}
