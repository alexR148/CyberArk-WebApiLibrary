using CyberArk.WebApi.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CyberArk.WebApi.Container
{
    #region API Parameter

    /// <summary>
    /// Required for RestAPI Method Add Safe Member
    /// </summary>
    class AddSafeMember_Method : RestApiMethod
    {
        public AddSafeMember_Parameter member
        { get; set; }
        = new AddSafeMember_Parameter();
    }

    #endregion

    #region API Result

    /// <summary>
    /// Result class for RestAPI List Safe Members Method
    /// </summary>
    class ListSafeMembers_Result : RestApiResult
    {
        public SafeMember_Parameter[] members
        { get; set; }
            = new SafeMember_Parameter[] { };       
    }

    class AddSafeMember_Result : RestApiResult
    {
        public AddSafeMember2_Parameter member
        { get; set; }
        = new AddSafeMember2_Parameter();
    }

    #endregion

    #region Members

    class AddSafeMember_Parameter : RestApiParameter
    {
        public string MemberName
        { get; set; }

        public string SearchIn
        { get; set; }

        public string MembershipExpirationDate
        { get; set; }

        public Hashtable Permissions
        { get; set; }
        
    }


    class AddSafeMember2_Parameter : RestApiParameter
    {
        public string MemberName
        { get; set; }
       
        public string MembershipExpirationDate
        { get; set; }

        public string SearchIn
        { get; set; }
              
        public object[] Permissions
        { get; set; }
        
    }


    /// <summary>
    /// Class describing a member and its save permissions
    /// </summary>
    public class SafeMember_Parameter : RestApiParameterSessionInfo
    {
        public string UserName
        { get; set; }
        = string.Empty;

        public ListSafeMemberPermission_Parameter Permissions
        { get; set; }
        = new ListSafeMemberPermission_Parameter();      
    }

    /// <summary>
    /// List of avaiable permissions returned by function List-Safemembers
    /// </summary>
    public class ListSafeMemberPermission_Parameter : RestApiParameter
    {
        public bool Add
        { get; set; }
        public bool AddRenameFolder
        { get; set; }
        public bool BackupSafe
        { get; set; }
        public bool Delete
        { get; set; }
        public bool DeleteFolder
        { get; set; }
        public bool ListContent
        { get; set; }
        public bool ManageSafe
        { get; set; }
        public bool ManageSafeMembers
        { get; set; }
        public bool MoveFilesAndFolders
        { get; set; }
        public bool Rename
        { get; set; }
        public bool RestrictedRetrieve
        { get; set; }
        public bool Retrieve
        { get; set; }
        public bool Unlock
        { get; set; }
        public bool Update
        { get; set; }
        public bool UpdateMetadata
        { get; set; }
        public bool ValidateSafeContent
        { get; set; }
        public bool ViewAudit
        { get; set; }
        public bool ViewMembers
        { get; set; }
    }

    public class SafeMemberPermissions_Parameter : RestApiParameter
    {
        public bool UseAccounts
        {
            get; set;
        }
        public bool RetrieveAccounts
        {
            get; set;
        }
        public bool ListAccounts
        {
            get; set;
        }
        public bool AddAccounts
        {
            get; set;
        }
        public bool UpdateAccountContent
        {
            get; set;
        }
        public bool UpdateAccountProperties
        {
            get; set;
        }
        public bool InitiateCPMAccountManagementOperations
        {
            get; set;
        }
        public bool SpecifyNextAccountContent
        {
            get; set;
        }
        public bool RenameAccounts
        {
            get; set;
        }
        public bool DeleteAccounts
        {
            get; set;
        }
        public bool UnlockAccounts
        {
            get; set;
        }
        public bool ManageSafe
        {
            get; set;
        }
        public bool ManageSafeMembers
        {
            get; set;
        }
        public bool BackupSafe
        {
            get; set;
        }
        public bool ViewAuditLog
        {
            get; set;
        }
        public bool ViewSafeMembers
        {
            get; set;
        }
        public int RequestsAuthorizationLevel
        {
            get; set;
        }
        public bool AccessWithoutConfirmation
        {
            get; set;
        }
        public bool CreateFolders
        {
            get; set;
        }
        public bool DeleteFolders
        {
            get; set;
        }
        public bool MoveAccountsAndFolders
        {
            get; set;
        }
    }

    #endregion

    #region Powershell Result
    public class PSSafeMembers_Result : PSApiResult
    {
        public SafeMember_Parameter[] members
        { get; set; }
            = new SafeMember_Parameter[] { };
    }

    public class PSAddSafeMembers_Result : PSApiResultSessionInfo
    {
        public string SafeName
        { get; set; }

        public string MemberName
        { get; set; }

        public string MembershipExpirationDate
        { get; set; }

        public Hashtable Permissions
        { get; set; }
        
    }
    #endregion

}
