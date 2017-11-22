using CyberArk.WebApi.Container;
using CyberArk.WebApi.Extensions;
using CyberArk.WebApi.Logging;
using System;
using System.Collections;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {
        /// <summary>
        /// List all Safemembers of a Safe
        /// </summary>
        /// <param name="SafeName">The name of a Safe</param>
        /// <returns></returns>
        public PSSafeMembers_Result List_PASSafeMembers(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = System.Uri.EscapeUriString(string.Format ("{0}{1}/{2}/Members", WebURI,URI,SafeName));

            //Create Safe Add Object
            NullableInput memberParameters = new NullableInput();
            onNewMessage(string.Format("List members of safe '{0}'", SafeName), LogMessageType.Info);

            //Do Api Call            
            WebResponseResult wrResult;
            ListSafeMembers_Result result = sendRequest<NullableInput, ListSafeMembers_Result>(uri, VERB_METHOD_GET, JSON_CONTENT_TYPE, SessionToken, memberParameters,out wrResult);

            //apply sessioninformation to result (neccessary for Powershell)
            foreach (SafeMember_Parameter r in result.members)
                applySessionInfo(r);

            //Get Result
            PSSafeMembers_Result psResult = null;
            if (result != null)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafeMembers_Result>(result);                
                onNewMessage(string.Format("Safe '{0}' successfully queried", SafeName), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to query safe '{0}'", SafeName), LogMessageType.Error);

            return psResult;
        }


        public PSAddSafeMembers_Result Add_PASSafeMember(string SafeName, string MemberName, string SearchIn = null, DateTime? MembershipExpirationDate = null, bool? UseAccounts = null,
            bool? RetrieveAccounts = null, bool? ListAccounts = null, bool? AddAccounts = null, bool? UpdateAccountContent = null, bool? UpdateAccountProperties = null,
            bool? InitiateCPMAccountManagementOperations = null, bool? SpecifyNextAccountContent = null, bool? RenameAccounts = null, bool? DeleteAccounts = null, bool? UnlockAccounts = null,
            bool? ManageSafe = null, bool? ManageSafeMembers = null, bool? BackupSafe = null, bool? ViewAuditLog = null, bool? ViewSafeMembers = null, int? RequestsAuthorizationLevel = null,
            bool? AccessWithoutConfirmation = null, bool? CreateFolders = null, bool? DeleteFolders = null, bool? MoveAccountsAndFolders = null)
        {
            
            //Create Permission Hashtable
            HashtableHelper hh = new HashtableHelper();            
            hh.AddMember(() => UseAccounts);
            hh.AddMember(() => RetrieveAccounts);
            hh.AddMember(() => ListAccounts);
            hh.AddMember(() => AddAccounts);
            hh.AddMember(() => UpdateAccountContent);
            hh.AddMember(() => UpdateAccountProperties);
            hh.AddMember(() => InitiateCPMAccountManagementOperations);
            hh.AddMember(() => SpecifyNextAccountContent);
            hh.AddMember(() => RenameAccounts);
            hh.AddMember(() => DeleteAccounts);
            hh.AddMember(() => UnlockAccounts);
            hh.AddMember(() => ManageSafe);
            hh.AddMember(() => ManageSafeMembers);
            hh.AddMember(() => BackupSafe);
            hh.AddMember(() => ViewAuditLog);
            hh.AddMember(() => ViewSafeMembers);
            hh.AddMember(() => RequestsAuthorizationLevel);
            hh.AddMember(() => AccessWithoutConfirmation);
            hh.AddMember(() => CreateFolders);
            hh.AddMember(() => DeleteFolders);
            hh.AddMember(() => MoveAccountsAndFolders);


            return Add_PASSafeMember(SafeName, MemberName, SearchIn, MembershipExpirationDate, hh.Result);
        }

        public PSAddSafeMembers_Result Add_PASSafeMember(string SafeName, string MemberName, string SearchIn, DateTime? MembershipExpirationDate, Hashtable permissions)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri = System.Uri.EscapeUriString(string.Format("{0}{1}/{2}/Members", WebURI, URI, SafeName));
          
            //Create Safe Add Object
            AddSafeMember_Method memberParameters = new AddSafeMember_Method();
            onNewMessage(string.Format("Add member to a safe '{0}'", SafeName), LogMessageType.Info);

            //Add properties
            memberParameters.member.MemberName = MemberName;
            memberParameters.member.SearchIn = SearchIn; 
            
            //Check time
            if (MembershipExpirationDate != null)           
                memberParameters.member.MembershipExpirationDate = string.Format("{0:MM\\/dd\\/yy}", MembershipExpirationDate).Replace("/","\\");

            //Add Hashtable
            memberParameters.member.Permissions = permissions;

            //Do Api Call            
            WebResponseResult wrResult;
            AddSafeMember_Result result = sendRequest<AddSafeMember_Method, AddSafeMember_Result>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE, SessionToken, memberParameters, out wrResult);


            //Get Result
            PSAddSafeMembers_Result psResult = null;
            if (result != null)
            {
                //Create PSResult
                psResult = createPSApiResults<PSAddSafeMembers_Result>(result.member);
                psResult.SafeName = SafeName; 
                onNewMessage(string.Format("Member {1} successfully added to safe '{0}'", SafeName,MemberName), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to add member {1} to safe '{0}'", SafeName,MemberName), LogMessageType.Error);

            return psResult;



        }


    }
}
