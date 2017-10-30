using System.Collections;

namespace CyberArk.WebApi.Container
{
    #region API Result

    /// <summary>
    /// Result class for RestAPI List Safe Members Function
    /// </summary>
    class ListSafeMembers_Result : RestApiResult
    {
        public Member[] members
        { get; set; }
            = new Member[] { };       
    }

    #endregion

    #region Members

    /// <summary>
    /// Class describing a member and its save permissions
    /// </summary>
    public class Member : RestApiMember
    {
        public string UserName
        { get; set; }
        = string.Empty;

        public PermissionList Permissions
        { get; set; }
        = new PermissionList();

        //Additional Sessioninformation. Required for Powershell
        #region SessionInformation
        public Hashtable sessionToken
        { get; set; }

        /// <summary>
        /// Returns the Complete Uri String
        /// </summary>
        public string WebURI
        {
            get
            {
                return BaseURI + "/" + PVWAAppName;
            }
        }

        /// <summary>
        /// The VaultName
        /// </summary>
        public string PVWAAppName
        { get; set; }

        /// <summary>
        /// WebAccessUri
        /// </summary>
        public string BaseURI
        { get; set; }
        #endregion
    }

    /// <summary>
    /// List of avaiable permissions
    /// </summary>
    public class PermissionList : RestApiMember
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
    #endregion

    #region Powershell Result
    public class PSSafeMembersResult : PSApiResult
    {
        public Member[] members
        { get; set; }
            = new Member[] { };
    }
    #endregion

}
