using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Library
{
    public class IntToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(parameter as string)
            {
                case "lkpINLeadStatus":
                    if (value != null)
                    {
                        return  (lkpINLeadStatus)(long)value;
                    }
                    return null;

                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
