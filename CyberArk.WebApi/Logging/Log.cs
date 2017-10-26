using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberArk.WebApi.Logging
{
    class Log
    {

        public static string GetLogString(string message, LogMessageType messageType)
        {
            string messagetype = (messageType.ToString() + ":").PadRight(10);
            return null; 
        }

        public static string GetLogString(MessageArgs message)
        {
            return GetLogString(message.Message, message.MessageType);
        }
    }
}
