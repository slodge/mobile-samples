using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.Converters.Visibility;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Core.Mvvm.Converters
{
    public class AllConverters
    {
        public readonly LowerDayNameConverter LowerDayName = new LowerDayNameConverter();
        public readonly LowerShortMonthNameConverter LowerShortMonth = new LowerShortMonthNameConverter();
        public readonly DateToTextConverter DateToText = new DateToTextConverter();
        public readonly DateToUpperTextConverter DateToUpperText = new DateToUpperTextConverter();
        public readonly MvxVisibilityConverter Visibility = new MvxVisibilityConverter();
        public readonly MvxInvertedVisibilityConverter InvertedVisibility = new MvxInvertedVisibilityConverter();
        public readonly HiddenIfNullOrEmptyConverter HiddenIfNullOrEmpty = new HiddenIfNullOrEmptyConverter();
        public readonly SpeakerListGroupedToSimpleListConverter SpeakerList = new SpeakerListGroupedToSimpleListConverter();
        public readonly ExhibitorListGroupedToSimpleListConverter ExhibitorList = new ExhibitorListGroupedToSimpleListConverter();
        
        public readonly SessionListGroupedToSeparatedListConverter SessionList = new SessionListGroupedToSeparatedListConverter();
        public readonly ParameterIfNullOrEmptyConverter ParameterIfNullOrEmpty = new ParameterIfNullOrEmptyConverter();
    }

    public class ParameterIfNullOrEmptyConverter : MvxBaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value as string))
                return parameter;

            return value;
        }
    }

    public class HiddenIfNullOrEmptyConverter :MvxBaseVisibilityConverter
    {
        public override MvxVisibility ConvertToMvxVisibility(object value, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value as string))
                return MvxVisibility.Collapsed;
            return MvxVisibility.Visible;
        }
    }

    public class LowerDayNameConverter : MvxBaseValueConverter
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format (culture, "{0:dddd}", value).ToLowerInvariant ();
        }
    }

    public class LowerShortMonthNameConverter : MvxBaseValueConverter
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format (culture, "{0:MMM}", value).ToLowerInvariant ();
        }
    }

    public class DateToTextConverter : MvxBaseValueConverter
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            var style = parameter == null ? "dd:MMM:yyyy" : parameter.ToString();
            return date.ToString(style);
        }
    }

    public class DateToUpperTextConverter : DateToTextConverter 
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.Convert(value, targetType, parameter, culture).ToString().ToUpper();
        }
    }

    public class GroupedToSimpleListConverter<TItem, TItemViewModel>
        : MvxBaseValueConverter
        where TItemViewModel : GroupedListItemViewModel<TItem>, new()
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var grouped = value as ObservableCollection<GroupedListGroupViewModel<TItem, TItemViewModel>>;
            if (grouped == null)
                return null;

            return grouped.SelectMany(x => x.Items).ToList();
        }
    }

    public class SpeakerListGroupedToSimpleListConverter
        : GroupedToSimpleListConverter<Speaker, SpeakerListItemViewModel>
    {
    }

    public class ExhibitorListGroupedToSimpleListConverter
        : GroupedToSimpleListConverter<Exhibitor, ExhibitorListItemViewModel>
    {
    }

    public class GroupedToSeparatedListConverter<TItem, TItemViewModel>
        : MvxBaseValueConverter
        where TItemViewModel : GroupedListItemViewModel<TItem>, new()
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var grouped = value as ObservableCollection<GroupedListGroupViewModel<TItem, TItemViewModel>>;
            if (grouped == null)
                return null;

            var list = new List<object>();
            foreach (var group in grouped)
            {
                list.Add(group.Title);
                list.AddRange(group.Items.Select(x => x as object));
            }
            return list;
        }
    }

    public class SessionListGroupedToSeparatedListConverter
        : GroupedToSeparatedListConverter<Session, SessionListItemViewModel>
    {
    }
}
