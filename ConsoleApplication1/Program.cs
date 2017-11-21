using System;
using CyberArk.WebApi;
using CyberArk.WebApi.Logging;
using System.Security;
using CyberArk.WebApi.Extensions;
using CyberArk.WebApi.other;

namespace CyberArk.Cmd
{
    class Program
    {
        static bool verbose = true;
        static bool debug   = false;

        static void Main(string[] args)
        {
            WebServices ws   = new WebServices(PD.GetServer());
            ws.NewLogMessage += Ws_NewMessage;
           
            
           
            ws.LogOn(PD.GetUser(),PD.GetPW().ToSecureString());
            ws.Add_PASSafe("75054PU", 12, 0, "75054PU");

            ws.Remove_PASSafe("75054PU");
            

            ws.LogOff(); 
        }

        private static void Ws_NewMessage(object sender, MessageArgs e)
        {
            //Abort Verbose if disabled
            if (e.MessageType == LogMessageType.Verbose && !verbose)
                return;

            //Abort Debug if disabled
            if (e.MessageType == LogMessageType.Debug && !debug)
                return;

            Console.WriteLine(e.ToString());
        }
    }
}
