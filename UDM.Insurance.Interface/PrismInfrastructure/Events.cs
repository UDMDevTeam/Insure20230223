using Prism.Events;

namespace UDM.Insurance.Interface.PrismInfrastructure
{
    public class CloseDialogEvent : PubSubEvent
    {
    }

    public class CloseDocumentEvent : PubSubEvent
    {
    }

    public class SendDialogMessageEvent : PubSubEvent<DialogMessage>
    {
    }

    public class UpdateUIMessageEvent : PubSubEvent<UpdateUIMessage>
    {
    }
}
