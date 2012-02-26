using System;
using Cirrious.MvvmCross.ViewModels;
using MWC.Core.Mvvm.JsonFileStore;

namespace MWC.Core.Mvvm.ApplicationSettings
{
    public class ApplicationSettings
        : MvxNotifyPropertyChanged
          , IApplicationSettings
          , IJsonStoreHelperConsumer
    {
        private readonly ApplicationSettingsState _state;
        public ApplicationSettings()
        {
            var state = this.LoadFromRepository<ApplicationSettingsState>(ApplicationSettingsConstants.ApplicationSettingsFileName);
            if (state == null)
                state = new ApplicationSettingsState();
            _state = state;
        }

        public bool IsDataSeeded
        {
            get { return _state.IsDataSeeded; }
            set
            {
                if (_state.IsDataSeeded == value)
                    return;

                _state.IsDataSeeded = value;
                Save();
                this.FirePropertyChanged("IsDataSeeded");
            }
        }

        public DateTime NextConferenceUpdateTimeUtc
        {
            get { return _state.NextConferenceUpdateTimeUtc; }
            set
            {
                if (_state.NextConferenceUpdateTimeUtc == value)
                    return;

                _state.NextConferenceUpdateTimeUtc = value;
                Save();
                this.FirePropertyChanged("NextConferenceUpdateTimeUtc");
            }
        }

        public DateTime NextExhibitorsUpdateTimeUtc
        {
            get { return _state.NextExhibitorsUpdateTimeUtc; }
            set
            {
                if (_state.NextExhibitorsUpdateTimeUtc == value)
                    return;

                _state.NextExhibitorsUpdateTimeUtc = value;
                Save();
                this.FirePropertyChanged("NextExhibitorsUpdateTimeUtc");
            }
        }


        private void Save()
        {
            this.SaveToRepository<ApplicationSettingsState>(_state, ApplicationSettingsConstants.ApplicationSettingsFileName);
        }
    }

    public class ApplicationSettingsConstants
    {
        public const string ApplicationSettingsFileName = "ApplicationSettings.json";
    }

    public interface IApplicationSettings
    {
        bool IsDataSeeded { get; set; }
        DateTime NextConferenceUpdateTimeUtc { get; set; }
        DateTime NextExhibitorsUpdateTimeUtc { get; set; }
    }

    public class ApplicationSettingsState
    {
        public bool IsDataSeeded { get; set; }
        public DateTime NextConferenceUpdateTimeUtc { get; set; }
        public DateTime NextExhibitorsUpdateTimeUtc { get; set; }

        public ApplicationSettingsState()
        {
            IsDataSeeded = false;
            NextConferenceUpdateTimeUtc = DateTime.UtcNow;
            NextExhibitorsUpdateTimeUtc = DateTime.UtcNow;
        }
    }
}
