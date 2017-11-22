using System.Security;

namespace CyberArk.WebApi.Container
{
    #region Parameter
    class AuthLogon : RestApiMethod
    {
        public string username
        { get; set; }
        public SecureString password
        { get; set; }
        public SecureString newPassword
        { get; set; }
        public bool? useRadiusAuthentication
        { get; set; }
        public int connectionNumber
        { get; set; }       
    }
    #endregion

    #region result
    class AuthLogonResult : RestApiResult
    {
        public string CyberArkLogonResult
        { get; set; }
    }
    

    class SharedAuthLogonResult : RestApiResult
    {
        public string LogonResult
        { get; set; }
    }
    #endregion
}
