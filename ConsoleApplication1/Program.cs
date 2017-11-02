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
            //Console.WriteLine("Enter User:");
            string a = PD.GetUser();
            //Console.WriteLine("Enter Password:");
            //SecureString s = ;
            //ws.LogOn(a,s);

            PD.CreateRoleJSON(); 

            //Use Shared Auth.
            //ws.LogOn();
            //var r  = ws.List_PASSafeMembers("75052");
            //ws.LogOff(); 
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
