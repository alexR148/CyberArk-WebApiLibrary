using System.Security;

namespace CyberArk.WebApi.Container
{
    class AuthLogon : RestApiInputParameter
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
}
