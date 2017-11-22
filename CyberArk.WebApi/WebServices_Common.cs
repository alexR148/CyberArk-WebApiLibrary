﻿using CyberArk.WebApi.Container;
using CyberArk.WebApi.Internal;
using CyberArk.WebApi.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {
        const string JSON_CONTENT_TYPE    = "application/json";

        const string VERB_METHOD_POST     = "POST";
        const string VERB_METHOD_GET      = "GET";
        const string VERB_METHOD_DELETE   = "DELETE";

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
        /// <param name="apiresult">apply an apiresult to do copy its values to a flat object</param>
        /// <returns></returns>
        private T createPSApiResults<T>(params object[] objectsToCopy)  where T : PSApiResult, new()
        {
            //Create PSResult Class of Type T
            T result = new T();

            //If Object implements ISessionInformation, then add information to object
            applySessionInfo(result);

            if (objectsToCopy != null)
            {
                foreach (object o in objectsToCopy)
                    copyValuesToPSApiResult(o, result);
            }
            
            //Only copy if apiresult is not null
            //if (apiresult != null)
            //    createPSApiResult_toFlat_recurse(apiresult, result);

                //Return the result
            return result; 
        }
        private T createPSApiResults<T>() where T : PSApiResult, new()
        {
            return createPSApiResults<T>(null);
        }


        ///// <summary>
        ///// Copies property values from a hierarchic object to a flat object. Flat objects are neccessary for Powershell Piping 
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="dest"></param>
        //private void createPSApiResult_toFlat_recurse(object source, object dest)
        //{
        //    //Abort if one of the two object has null values
        //    if (source == null || dest == null) return;

            //    //Get type of both
            //    Type TSource = source.GetType();
            //    Type TDest   = dest.GetType();

            //    //Get all properties of source object
            //    PropertyInfo[] propertiesSource = TSource.GetProperties();

            //    //Iterate all properties
            //    foreach (PropertyInfo prop in propertiesSource)
            //    {              
            //        //Get value
            //        var value = prop.GetValue(source);

            //        //Check value. Do nothing if result is null
            //        if (value != null)
            //        {
            //            //Get type of result
            //            Type Tvalue = value.GetType();

            //            //if type is a APIMember then copy Properties. For PS it is neccessary that the result is flat
            //            if (Tvalue.IsSubclassOf(typeof(RestApiMember)))                    
            //                createPSApiResult_toFlat_recurse(value, dest);                   
            //            else
            //            {
            //                //Get dest property
            //                PropertyInfo destProp = TDest.GetProperty(prop.Name);

            //                //Set value to dest property
            //                if (destProp != null)
            //                    destProp.SetValue(dest, value);
            //            }
            //        }                
            //    }         
            //}

        private void applySessionInfo(object source)
        {
            ISessionInformation resultTmp = source as ISessionInformation;
            if (resultTmp != null)
            {
                //Save default Values
                resultTmp.BaseURI      = this.BaseURI;
                resultTmp.PVWAAppName  = this.PVWAAppName;
                resultTmp.sessionToken = this.SessionToken;
            }
        }

        private void copyValuesToPSApiResult(object source, object dest)
        { 
            if (source == null || dest == null) return; 

            Type TSource = source.GetType(); 
            Type TDest = dest.GetType(); 
 
            PropertyInfo[] propertiesSource = TSource.GetProperties();             
            foreach (PropertyInfo prop in propertiesSource) 
            {                              
                //Get value 
                var value               = prop.GetValue(source);  
                PropertyInfo destProp   = TDest.GetProperty(prop.Name);

                //Set value to des 
                if (destProp != null) 
                {
                    if (prop.PropertyType == destProp.PropertyType)
                        destProp.SetValue(dest, value);                   
                }
                                            
            }          
        }

        public T objectArrayToHashtabe<T>(object[] oarr) where T : new()
        {
            T result     = new T();
            Type Tresult = result.GetType();


            foreach (var b in oarr)
            {
                Dictionary<string, object> ds = b as Dictionary<string, object>;
                if (ds != null)
                {
                    object name;
                    object value;
                    ds.TryGetValue("Key", out name);
                    ds.TryGetValue("Value", out value);

                    PropertyInfo Prop = Tresult.GetProperty(name.ToString());

                    //Set value to des 
                    if (Prop != null)
                    {
                        if (Prop.PropertyType == value.GetType())
                            Prop.SetValue(result, value);
                    }

                    //Clean
                    name  = null;
                    value = null;
                    Prop  = null;
                }  
            }         
            return result;
        }
                 
    }
}