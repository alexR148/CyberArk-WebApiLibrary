

namespace CyberArk.WebApi.Container
{
    #region Parameter

    class AddSafe_Parameter : RestApiParameter
    {      
        public SafeMembers safe
        { get; set; }
        = new SafeMembers();          
    }


    #endregion

    #region API Result
    class AddSafe_Result : RestApiResult
    {
        public SafeMembers safe
        { get; set; }
        = new SafeMembers();
    }

    class GetSafe_Result : RestApiResult
    {
        public SafeMembers GetSafeResult
        { get; set; }
        = new SafeMembers();
    }

    #endregion

    #region Members
    class SafeMembers : RestApiMember 
    {
        public string SafeName
        { get; set; }
        public string Description
        { get; set; }
        public bool? OLACEnabled
        { get; set; }
        public string ManagingCPM
        { get; set; }
        public int? NumberOfVersionsRetention
        { get; set; }
        public int? NumberOfDaysRetention
        { get; set; }
    }
    #endregion

    #region Powershell Result
    public class PSSafeResult : PSApiResult
    {
        public string SafeName
        { get; set; }
        public string Description
        { get; set; }
        public bool? OLACEnabled
        { get; set; }
        public string ManagingCPM
        { get; set; }
        public int? NumberOfVersionsRetention
        { get; set; }
        public int? NumberOfDaysRetention
        { get; set; }
    }
    #endregion
}
