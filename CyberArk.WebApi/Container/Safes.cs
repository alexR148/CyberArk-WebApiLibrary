

namespace CyberArk.WebApi.Container
{
    #region Parameter

    class AddSafe_Method : RestApiMethod
    {      
        public SafeMembers safe
        { get; set; }
        = new SafeMembers();          
    }


    #endregion

    #region API Result
    class AddSafe_Result : RestApiResult
    {
        public SafeMembers AddSafeResult
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
    class SafeMembers : RestApiParameter 
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
    public class PSSafe_Result : PSApiResultSessionInfo
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
