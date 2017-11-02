using System.Collections;

namespace CyberArk.WebApi.Container
{
    #region Abstract Classes

    
    
    /// <summary>
    /// The Base Class of all Containers
    /// </summary>
    public abstract class CAContainer
    { }

    /// <summary>
    /// Base Parameter which always have to be used for input
    /// </summary>
    abstract class RestApiParameter : CAContainer
    {}

    /// <summary>
    /// Base result Parameters which always will be returned. Neccessary for Powershell
    /// </summary>
    abstract class RestApiResult : CAContainer
    { }

    /// <summary>
    /// Defines a Base Rest Api Member
    /// </summary>
    public abstract class RestApiMember : CAContainer
    { }

    /// <summary>
    /// Defines an Api Member containing sessioninformation
    /// </summary>
    public class RestApiMemberSessionInfo : RestApiMember, ISessionInformation
    {
        public Hashtable sessionToken
        { get; set; }

        /// <summary>
        /// Returns the Complete Uri String
        /// </summary>
        public string WebURI
        {
            get
            {
                return commonFunctions.Get_WebUri(BaseURI, PVWAAppName);
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
    }

    #endregion

    #region Nullable Classes

    class NullableInput : RestApiParameter
    {}

    class NullableOutput : RestApiResult
    {}
    #endregion

    #region Powershell Result Base Class


    public abstract class PSApiResult : CAContainer
    { }
    
    /// <summary>
    /// A Public result Object containing sessioninformation. Neccessary for Powershell
    /// </summary>
    public class PSApiResultSessionInfo : PSApiResult, ISessionInformation
    {
        public Hashtable sessionToken
        { get; set; }

        /// <summary>
        /// Returns the Complete Uri String
        /// </summary>
        public string WebURI
        {
            get
            {                
                return commonFunctions.Get_WebUri(BaseURI, PVWAAppName);
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
    }
    #endregion

    #region static classes
    internal static class commonFunctions
    {
        public static string Get_WebUri(string BaseURI, string PVWAAppName)
        {
            if (string.IsNullOrWhiteSpace(BaseURI) || string.IsNullOrWhiteSpace(PVWAAppName))
                return string.Empty;
            return BaseURI + "/" + PVWAAppName;
        }
    }
    
    #endregion


}
