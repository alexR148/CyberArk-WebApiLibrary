using System.Collections;

namespace CyberArk.WebApi.Container
{
    #region API Result

    /// <summary>
    /// Result class for RestAPI List Safe Members Function
    /// </summary>
    class ListSafeMembers_Result : RestApiResult
    {
        public SafeMember_Result[] members
        { get; set; }
            = new SafeMember_Result[] { };       
    }

    #endregion

    #region Members

    /// <summary>
    /// Class describing a member and its save permissions
    /// </summary>
    public class SafeMember_Result : RestApiMemberSessionInfo
    {
        public string UserName
        { get; set; }
        = string.Empty;

        public ListSafeMemberPermission_Result Permissions
        { get; set; }
        = new ListSafeMemberPermission_Result();      
    }

    /// <summary>
    /// List of avaiable permissions returned by function List-Safemembers
    /// </summary>
    public class ListSafeMemberPermission_Result : RestApiMember
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

    /// <summary>
    /// List of Permissions for function Add-SafeMember or Update-SafeMember
    /// </summary>
    public class AddSafeMemberPermissions : RestApiMember
    {
        public bool UseAccounts
        { get; set; }
        public bool RetrieveAccounts
        { get; set; }
        public bool ListAccounts
        { get; set; }
        public bool AddAccounts
        { get; set; }
        public bool UpdateAccountContent
        { get; set; }
        public bool UpdateAccountProperties
        { get; set; }
        public bool InitiateCPMAccountManagementOperations
        { get; set; }
        public bool SpecifyNextAccountContent
        { get; set; }
        public bool RenameAccounts
        { get; set; }
        public bool DeleteAccounts
        { get; set; }
        public bool UnlockAccounts
        { get; set; }
        public bool ManageSafe
        { get; set; }
        public bool ManageSafeMembers
        { get; set; }
        public bool BackupSafe
        { get; set; }
        public bool ViewAuditLog
        { get; set; }
        public bool ViewSafeMembers
        { get; set; }
        public int RequestsAuthorizationLevel
        { get; set; }
        public bool AccessWithoutConfirmation
        { get; set; }
        public bool CreateFolders
        { get; set; }
        public bool DeleteFolders
        { get; set; }
        public bool MoveAccountsAndFolders
        { get; set; }
    }   
    #endregion

    #region Powershell Result
    public class PSSafeMembersResult : PSApiResult
    {
        public SafeMember_Result[] members
        { get; set; }
            = new SafeMember_Result[] { };
    }
    #endregion

}
