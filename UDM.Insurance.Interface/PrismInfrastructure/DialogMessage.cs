using Embriant.Framework;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface.PrismInfrastructure
{
    public class DialogMessage
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public ShowMessageType Type { get; set; }
    }

    public class UpdateUIMessage
    {
        public string ControlName { get; set; }

        public string Property { get; set;}

        public string Value { get; set;}
    }
}
