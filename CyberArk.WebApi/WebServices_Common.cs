using CyberArk.WebApi.Container;
using CyberArk.WebApi.Internal;
using CyberArk.WebApi.Logging;
using System;
using System.Collections;
using System.Reflection;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {
        const string JSON_CONTENT_TYPE = "application/json";

        const string VERB_METHOD_POST     = "POST";
        const string VERB_METHOD_GET      = "GET";

        const string DEFAULT_VAULT        = "passwordVault";
        const string SESSION_TOKEN_HEADER = "Authorization";

        #region Properties
        /// <summary>
        /// WebAccessUri
        /// </summary>
        public string BaseURI
        { get; set; }

        /// <summary>
        /// The VaultName
        /// </summary>
        public string PVWAAppName
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
        /// The Sessiontoken
        /// </summary>
        public Hashtable SessionToken
        { get; set; }
        = new Hashtable();

        /// <summary>
        /// The connection number
        /// </summary>
        public string ConnectionNumber
        { get; private set; }
        = string.Empty;

        /// <summary>
        /// The used Authenticationmethod
        /// </summary>
        private AuthenticationType Authenticationtype
        { get;  set; }
        = AuthenticationType.None;
        #endregion


        #region Events
        public event EventHandler<MessageArgs> NewLogMessage;

        private void onNewMessage(string message, LogMessageType messagetype)
        {
            NewLogMessage?.Invoke(this, new MessageArgs() { Timestamp = DateTime.Now, Message = message, MessageType = messagetype });
        }
        #endregion

        /// <summary>
        /// A PVWA WebAccess Uri; for example https://myServ.org.com
        /// </summary>
        /// <param name="pvwa_base_uri"></param>
        public WebServices(string pvwa_base_uri)
        {
            BaseURI     = pvwa_base_uri;
            PVWAAppName = DEFAULT_VAULT;
        }

        /// <summary>
        /// A PVWA WebAccess Uri and the VaultName
        /// </summary>
        /// <param name="pvwa_base_uri"></param>
        /// <param name="VaultAppName"></param>
        public WebServices(string pvwa_base_uri,string VaultAppName)
        {
            BaseURI     = pvwa_base_uri.Trim();

            if (string.IsNullOrWhiteSpace(VaultAppName))
                PVWAAppName = DEFAULT_VAULT;
            else
                PVWAAppName = VaultAppName.Trim();
        }

        /// <summary>
        /// Creates an API Result with the Base Settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T createApiResults<T>()  where T : PublicApiResult, new()
        {
            T result = new T();
            result.BaseURI      = this.BaseURI;
            result.PVWAAppName  = this.PVWAAppName;
            result.sessionToken = this.SessionToken;

            return result; 
        }

        private void copyProperties(object source, object dest)
        {
            if (source == null || dest == null) return;

            Type TSource = source.GetType();
            Type TDest = dest.GetType();

            PropertyInfo[] propertiesSource = TSource.GetProperties();
           

            foreach (PropertyInfo prop in propertiesSource)
            {
               


                //Get value
                var value   = prop.GetValue(source);

                //PropertyInfo[] propertiesOfValue = null;

                //if (value != null)
                //{
                //    Type Tvalue = value.GetType();
                //    propertiesOfValue = Tvalue.GetProperties(BindingFlags.Instance |  BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty);
                //}

                //if (propertiesOfValue != null && propertiesOfValue.Length > 0)
                //    copyProperties(value, dest);
                //else
                //{
                    //Get name
                    string name = prop.Name;

                    PropertyInfo destProp = TDest.GetProperty(name);

                    //Set value to des
                    if (destProp != null)
                        destProp.SetValue(dest, value);
                //}
            }         
        }

    }
}