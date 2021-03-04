using System.Windows;

namespace UDM.Insurance.Interface.Data
{
    internal class TriggerManager : DependencyObject
    {
        public static bool GetFlag1(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag1Property);
        }
        public static void SetFlag1(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag1Property, value);
        }
        public static readonly DependencyProperty Flag1Property = DependencyProperty.RegisterAttached("Flag1", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));
        

        public static bool GetFlag2(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag2Property);
        }
        public static void SetFlag2(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag2Property, value);
        }
        public static readonly DependencyProperty Flag2Property = DependencyProperty.RegisterAttached("Flag2", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));

        
        public static bool GetFlag3(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag3Property);
        }
        public static void SetFlag3(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag3Property, value);
        }
        public static readonly DependencyProperty Flag3Property = DependencyProperty.RegisterAttached("Flag3", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));
        

        public static bool GetFlag4(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag4Property);
        }
        public static void SetFlag4(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag4Property, value);
        }
        public static readonly DependencyProperty Flag4Property = DependencyProperty.RegisterAttached("Flag4", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlag5(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag5Property);
        }
        public static void SetFlag5(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag5Property, value);
        }
        public static readonly DependencyProperty Flag5Property = DependencyProperty.RegisterAttached("Flag5", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlag6(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag6Property);
        }
        public static void SetFlag6(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag6Property, value);
        }
        public static readonly DependencyProperty Flag6Property = DependencyProperty.RegisterAttached("Flag6", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlag7(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag7Property);
        }
        public static void SetFlag7(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag7Property, value);
        }
        public static readonly DependencyProperty Flag7Property = DependencyProperty.RegisterAttached("Flag7", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlag8(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag8Property);
        }
        public static void SetFlag8(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag8Property, value);
        }
        public static readonly DependencyProperty Flag8Property = DependencyProperty.RegisterAttached("Flag8", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlag9(DependencyObject obj)
        {
            return (bool)obj.GetValue(Flag9Property);
        }
        public static void SetFlag9(DependencyObject obj, bool value)
        {
            obj.SetValue(Flag9Property, value);
        }
        public static readonly DependencyProperty Flag9Property = DependencyProperty.RegisterAttached("Flag9", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagA(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagAProperty);
        }
        public static void SetFlagA(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagAProperty, value);
        }
        public static readonly DependencyProperty FlagAProperty = DependencyProperty.RegisterAttached("FlagA", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagB(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagBProperty);
        }
        public static void SetFlagB(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagBProperty, value);
        }
        public static readonly DependencyProperty FlagBProperty = DependencyProperty.RegisterAttached("FlagB", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagC(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagCProperty);
        }
        public static void SetFlagC(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagCProperty, value);
        }
        public static readonly DependencyProperty FlagCProperty = DependencyProperty.RegisterAttached("FlagC", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagD(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagDProperty);
        }
        public static void SetFlagD(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagDProperty, value);
        }
        public static readonly DependencyProperty FlagDProperty = DependencyProperty.RegisterAttached("FlagD", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagE(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagEProperty);
        }
        public static void SetFlagE(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagEProperty, value);
        }
        public static readonly DependencyProperty FlagEProperty = DependencyProperty.RegisterAttached("FlagE", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagF(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagFProperty);
        }
        public static void SetFlagF(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagFProperty, value);
        }
        public static readonly DependencyProperty FlagFProperty = DependencyProperty.RegisterAttached("FlagF", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagG(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagGProperty);
        }
        public static void SetFlagG(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagGProperty, value);
        }
        public static readonly DependencyProperty FlagGProperty = DependencyProperty.RegisterAttached("FlagG", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagH(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagHProperty);
        }
        public static void SetFlagH(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagHProperty, value);
        }
        public static readonly DependencyProperty FlagHProperty = DependencyProperty.RegisterAttached("FlagH", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagI(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagIProperty);
        }
        public static void SetFlagI(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagIProperty, value);
        }
        public static readonly DependencyProperty FlagIProperty = DependencyProperty.RegisterAttached("FlagI", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagJ(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagJProperty);
        }
        public static void SetFlagJ(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagJProperty, value);
        }
        public static readonly DependencyProperty FlagJProperty = DependencyProperty.RegisterAttached("FlagJ", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));

        public static bool GetFlagK(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagKProperty);
        }
        public static void SetFlagK(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagKProperty, value);
        }
        public static readonly DependencyProperty FlagKProperty = DependencyProperty.RegisterAttached("FlagK", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));

        public static bool GetFlagL(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagLProperty);
        }
        public static void SetFlagL(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagLProperty, value);
        }
        public static readonly DependencyProperty FlagLProperty = DependencyProperty.RegisterAttached("FlagL", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));

        public static bool GetFlagM(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagMProperty);
        }
        public static void SetFlagM(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagLProperty, value);
        }
        public static readonly DependencyProperty FlagMProperty = DependencyProperty.RegisterAttached("FlagM", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));

        public static bool GetFlagN(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagNProperty);
        }
        public static void SetFlagN(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagNProperty, value);
        }
        public static readonly DependencyProperty FlagNProperty = DependencyProperty.RegisterAttached("FlagN", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        //public static bool GetFlagO(DependencyObject obj)
        //{
        //    return (bool)obj.GetValue(FlagOProperty);
        //}
        //public static void SetFlagO(DependencyObject obj, bool value)
        //{
        //    obj.SetValue(FlagOProperty, value);
        //}
        //public static readonly DependencyProperty FlagOProperty = DependencyProperty.RegisterAttached("FlagO", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));







        public static bool GetFlagX(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagXProperty);
        }
        public static void SetFlagX(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagXProperty, value);
        }
        public static readonly DependencyProperty FlagXProperty = DependencyProperty.RegisterAttached("FlagX", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagY(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagYProperty);
        }
        public static void SetFlagY(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagYProperty, value);
        }
        public static readonly DependencyProperty FlagYProperty = DependencyProperty.RegisterAttached("FlagY", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));


        public static bool GetFlagZ(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlagZProperty);
        }
        public static void SetFlagZ(DependencyObject obj, bool value)
        {
            obj.SetValue(FlagZProperty, value);
        }
        public static readonly DependencyProperty FlagZProperty = DependencyProperty.RegisterAttached("FlagZ", typeof(bool), typeof(TriggerManager), new UIPropertyMetadata(false));

    }
}