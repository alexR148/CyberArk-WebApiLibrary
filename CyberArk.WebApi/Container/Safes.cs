

namespace CyberArk.WebApi.Container
{
    #region Input

    class Add_Safe : RestApiInputParameter
    {      
        public SafeMembers safe
        { get; set; }
        = new SafeMembers();          
    }


    #endregion

    #region Output

    class Add_SafeResult : RestApiResult
    {
        public SafeMembers safe
        { get; set; }
        = new SafeMembers();
    }

    class Get_SafeResult : RestApiResult
    {
        public SafeMembers GetSafeResult
        { get; set; }
        = new SafeMembers();
    }

    #endregion

    #region Subitems
    class SafeMembers
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

    #region Public
    public class SafeResult : PublicApiResult
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
