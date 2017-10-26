using System;

namespace CyberArk.WebApi.Logging
{
    public class MessageArgs : EventArgs
    {
        public DateTime Timestamp
        { get; set; }

        public string Message
        { get; set; }

        public LogMessageType MessageType
        { get; set; }

        /// <summary>
        /// Returns a string containing DateTime MessageInfo and the MessageText
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {            
            return string.Format("{0:HH:mm:ss:fff}  {1}{2}", Timestamp, (MessageType.ToString() + ":").PadRight(9), Message);          
        }
    }
}
