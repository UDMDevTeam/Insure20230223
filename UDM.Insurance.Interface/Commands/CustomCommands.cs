using System.Windows.Input;

namespace UDM.Insurance.Interface.Commands
{
    class CustomCommands
    {
        public static readonly RoutedUICommand Save = new RoutedUICommand
        (
            "Save",
            "Save",
            typeof(CustomCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.S, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand Report = new RoutedUICommand
        (
            "Report",
            "Report",
            typeof(CustomCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.R, ModifierKeys.Alt)
            }
        );

        //Define more commands here, just like the one above



    }
}
