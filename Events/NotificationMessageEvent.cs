using CommunityToolkit.Mvvm.Messaging.Messages;
using Pulse_MAUI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Events
{
    public class NotificationMessageEvent : ValueChangedMessage<NotifyType>
    {
        public NotificationMessageEvent(NotifyType value) : base(value)
        {
        }
    }
}
