using System.Collections;

namespace CyberArk.WebApi.Container
{
    #region Abstract Classes

    /// <summary>
    /// Default Parameter which always have to be used for input
    /// </summary>
    abstract class RestApiParameter
    {}

    /// <summary>
    /// Default result Parameters which always will be returned. Neccessary for Powershell
    /// </summary>
    abstract class RestApiResult
    {}       
    
    //Defines a Rest Api Member
    public abstract class RestApiMember
    {}

    #endregion

    #region Nullable Classes

    class NullableInput : RestApiParameter
    {}

    class NullableOutput : RestApiResult
    {}
    #endregion

    #region Powershell Result Base Class
    /// <summary>
    /// A Public result Object. Neccessary for Powershell
    /// </summary>
    public class PSApiResult
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
    }
    #endregion
}
