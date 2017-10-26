using System.Collections;

namespace CyberArk.WebApi.Container
{
    #region Private Abstract Classes

    /// <summary>
    /// Default Parameter which always have to be used for input
    /// </summary>
    abstract class RestApiInputParameter
    {}

    /// <summary>
    /// Default result Parameters which always will be returned. Neccessary for Powershell
    /// </summary>
    abstract class RestApiResult
    {
       
    }

    #endregion

    #region Private Nullable Classes

    class NullableInput : RestApiInputParameter
    {}

    class NullableOutput : RestApiResult
    {}
    #endregion

    #region Public
    /// <summary>
    /// A Public result Object. Neccessary for Powershell
    /// </summary>
    public class PublicApiResult
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
