using System.Collections;

namespace CyberArk.WebApi.Container
{
    public interface ISessionInformation
    {
        string BaseURI { get; set; }
        string PVWAAppName { get; set; }
        Hashtable sessionToken { get; set; }
        string WebURI { get; }
    }
}