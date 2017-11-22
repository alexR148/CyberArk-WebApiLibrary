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



            string test = "{ \"member\":{ \"MemberName\":\"svcsdk\",\"MembershipExpirationDate\":\"\",\"Permissions\":[{\"Key\":\"UseAccounts\",\"Value\":true},{\"Key\":\"RetrieveAccounts\",\"Value\":true},{\"Key\":\"ListAccounts\",\"Value\":true},{\"Key\":\"AddAccounts\",\"Value\":false},{\"Key\":\"UpdateAccountContent\",\"Value\":false},{\"Key\":\"UpdateAccountProperties\",\"Value\":false},{\"Key\":\"InitiateCPMAccountManagementOperations\",\"Value\":false},{\"Key\":\"SpecifyNextAccountContent\",\"Value\":false},{\"Key\":\"RenameAccounts\",\"Value\":false},{\"Key\":\"DeleteAccounts\",\"Value\":false},{\"Key\":\"UnlockAccounts\",\"Value\":false},{\"Key\":\"ManageSafe\",\"Value\":false},{\"Key\":\"ManageSafeMembers\",\"Value\":false},{\"Key\":\"BackupSafe\",\"Value\":false},{\"Key\":\"ViewAuditLog\",\"Value\":true},{\"Key\":\"ViewSafeMembers\",\"Value\":true},{\"Key\":\"AccessWithoutConfirmation\",\"Value\":false},{\"Key\":\"CreateFolders\",\"Value\":false},{\"Key\":\"DeleteFolders\",\"Value\":false},{\"Key\":\"MoveAccountsAndFolders\",\"Value\":false},{\"Key\":\"RequestsAuthorizationLevel\",\"Value\":0}],\"SearchIn\":\"Vault\"}}";
            var bla = ws.TestDeserialization<AddSafeMember_Result>(test);
            SafeMemberPermissions_Parameter h = ws.objectArrayToHashtabe< SafeMemberPermissions_Parameter>(bla.member.Permissions); 
            return;

            ws.LogOn(PD.GetUser(),PD.GetPW().ToSecureString());
            ws.Add_PASSafe("75054PU", 12, 0, "75054PU");
            ws.Add_PASSafeMember("75054PU", "svcsdk", "Vault",
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
