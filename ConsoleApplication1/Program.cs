using System;
using CyberArk.WebApi;
using CyberArk.WebApi.Logging;
using System.Security;
using CyberArk.WebApi.Extensions;
using CyberArk.WebApi.other;
using CyberArk.WebApi.Container;

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



           

            ws.LogOn(PD.GetUser(), PD.GetPW () );
            ws.Add_PASSafe("bla", 12, 0);
            
            
            dynamic bla = ws.Add_PASSafeMember("bla", "svcsdk", "Vault",
                UseAccounts: false,
                RetrieveAccounts: false,
                ListAccounts: false,
                AddAccounts: true,
                UpdateAccountContent: true,
                UpdateAccountProperties: true,
                InitiateCPMAccountManagementOperations: false,
                SpecifyNextAccountContent: false,
                RenameAccounts: true,
                DeleteAccounts: true,
                UnlockAccounts: true,
                ManageSafe: true,
                ManageSafeMembers: true,
                BackupSafe: false,
                ViewAuditLog: false,
                ViewSafeMembers: true,
                RequestsAuthorizationLevel: 0,
                AccessWithoutConfirmation: true,
                CreateFolders: false,
                DeleteFolders: false,
                MoveAccountsAndFolders: false);


            ws.Remove_PASSafe("bla");
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
