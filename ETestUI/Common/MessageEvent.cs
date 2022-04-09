using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Common
{
    public class MessageEvent : PubSubEvent<MessageItem>
    {

    }
    public class MessageItem
    {
        public object Sender { get; set; }
        public string Message { get; set; }
    }
}
