﻿using System;
using CyberArk.WebApi;
using System.Security;
using CyberArk.WebApi.Extensions;
using CyberArk.WebApi.Logging;

namespace CyberArk.Cmd
{
    class Program
    {
        static bool verbose = true;
        static bool debug   = false;

        static void Main(string[] args)
        {
            WebServices ws   = new WebServices(@"Enter Server here ");
            ws.NewLogMessage += Ws_NewMessage;           
            //Console.WriteLine("Enter User:");
            string a = "user";
            //Console.WriteLine("Enter Password:");
            SecureString s = "blabla".ToSecureString();
            ws.LogOn(a,s);
            //ws.LogOn();
            ws.Get_PASSafe("Safe1");
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
