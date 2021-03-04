using System;
using System.Windows;

namespace UDM.Insurance.Interface.Data
{
    internal class AttachedProperties : DependencyObject
    {
        public static readonly DependencyProperty IsValueChangedProperty = DependencyProperty.RegisterAttached("IsValueChanged", typeof(Boolean), typeof(AttachedProperties),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsValueChangedPropertyChanged));

        public static Boolean GetIsValueChanged(UIElement element)
        {
            return (Boolean)element.GetValue(IsValueChangedProperty);
        }
        public static void SetIsValueChanged(UIElement element, Boolean value)
        {
            element.SetValue(IsValueChangedProperty, value);
        }

        public static void OnIsValueChangedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }





        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.RegisterAttached("Description", typeof(object), typeof(AttachedProperties), new UIPropertyMetadata(null));

        public static object GetDescription(DependencyObject obj)
        {
            return obj.GetValue(DescriptionProperty);
        }
        public static void SetDescription(DependencyObject obj, object value)
        {
            obj.SetValue(DescriptionProperty, value);
        }





        public static readonly DependencyProperty HistoryValueProperty = DependencyProperty.RegisterAttached("HistoryValue", typeof(object), typeof(AttachedProperties), new UIPropertyMetadata(null));

        public static object GetHistoryValue(DependencyObject obj)
        {
            return obj.GetValue(IsValueChangedProperty);
        }
        public static void SetHistoryValue(DependencyObject obj, object value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }
    }
}
