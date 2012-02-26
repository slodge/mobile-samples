using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace MWC.Core.Mvvm.ViewModels
{
    public class ScheduleViewModel
        : ViewModelBase
    {
        public class DateTimeWithCommand
        {
            private readonly ScheduleViewModel _parent;

            public DateTimeWithCommand(ScheduleViewModel parent, DateTime when)
            {
                _parent = parent;
                When = when;
            }

            public DateTime When { get; private set; }
            public IMvxCommand Command
            {
                get
                {
                    return new MvxRelayCommand(() => _parent.RequestNavigate<SessionListViewModel>(new { listKey = SessionListViewModel.DayOfWeekKey(When) }));
                }
            }
        }

        public IMvxCommand DayChosenCommand
        {
            get
            {
                return new MvxRelayCommand<DateTimeWithCommand>((chosen) => chosen.Command.Execute());
            }
        }

        public DateTimeWithCommand Monday { get; private set; }
        public DateTimeWithCommand Tuesday { get; private set; }
        public DateTimeWithCommand Wednesday { get; private set; }
        public DateTimeWithCommand Thursday { get; private set; }
        public IList<DateTimeWithCommand> Days { get; private set; }

        public ScheduleViewModel()
        {
            Monday = new DateTimeWithCommand(this, new DateTime(2012, 2, 27));
            Tuesday = new DateTimeWithCommand(this, new DateTime(2012, 2, 28));
            Wednesday = new DateTimeWithCommand(this, new DateTime(2012, 2, 29));
            Thursday = new DateTimeWithCommand(this, new DateTime(2012, 3, 1));

            Days = new List<DateTimeWithCommand>()
                       {
                           Monday, Tuesday, Wednesday, Thursday
                       };
        }
        
    }
}