using CyberArk.WebApi.Internal;
using CyberArk.WebApi.Logging;
using CyberArk.WebApi.Container;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;
using System;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {

        private WebResponseResult sendRequest(string uri, string method, string contenttype, Hashtable header, string bodyAsJson,out string ResultAsJson) 
        {
            //Only allow TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Create WebRequest
            WebRequest restRequest  = WebRequest.Create(uri);
            restRequest.Method      = method;
            restRequest.ContentType = contenttype;
            //restRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            //Apply Header
            if (header != null && header.Count > 0)
            {
                foreach (DictionaryEntry de in header)
                {
                    string fieldName = de.Key.ToString();
                    string value     = de.Value.ToString();
                    restRequest.Headers[fieldName] = value;
                }
            }

            //Create WebResponse Result
            WebResponseResult wrResult = null;

            //Assign null to result object
            ResultAsJson = null;
            try
            {
                //Only send request for Post method
                if (restRequest.Method == VERB_METHOD_POST)
                {
                    //Send Webrequest
                    using (Stream requestStream = restRequest.GetRequestStream())
                    {
                        //Get Bytes from json 
                        byte[] inputStringBytes = Encoding.UTF8.GetBytes(bodyAsJson);

                        //Write to stream
                        requestStream.Write(inputStringBytes, 0, inputStringBytes.Length);
                    }
                }
                //Receive Response
                WebResponse restResponse;

                using (restResponse = restRequest.GetResponse())
                {
                    using (Stream responseStream = restResponse.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                        ResultAsJson = sr.ReadToEnd();                                                                                 
                    }
                    HttpWebResponse res = ((HttpWebResponse)(restResponse));
                    wrResult            = new WebResponseResult() { StatusCode = res.StatusCode, StatusDescription = res.StatusDescription };
                    onNewMessage(string.Format("{0},{1} - {2}", (int)res.StatusCode, res.StatusCode, res.StatusDescription), LogMessageType.Debug);
                }               
                onNewMessage(string.Format("Api Call successfully done."), LogMessageType.Debug);
               
            }
            catch (WebException ex)
            {
                HttpWebResponse res = ((HttpWebResponse)(ex.Response));
                wrResult = new WebResponseResult() { StatusCode = res.StatusCode, StatusDescription = res.StatusDescription };
                onNewMessage(string.Format("{0},{1} - {2} {3}", (int)res.StatusCode, res.StatusCode, ex.Message, res.StatusDescription), LogMessageType.Error);
            }
            //Return the result
            return wrResult;
        }

        /// <summary>
        /// Sends a WebRequest to the URI and receive a result
        /// </summary>
        /// <typeparam name="T">InputParameter Type</typeparam>
        /// <typeparam name="U">OutputParameter Type</typeparam>
        /// <param name="uri">Unified Ressource Identifier</param>
        /// <param name="method">Web Method, Post, Get</param>
        /// <param name="contenttype">The content type</param>
        /// <param name="sessiontoken">The sessiontoken; can be null</param>
        /// <param name="inputParameter">InputParameter Value of Type T</param>
        /// <param name="Result">The result of WebResponse</param>
        /// <returns>OutputParameter of Type U</returns>
        private WebResponseResult sendRequest<T,U>(string uri, string method, string contenttype, Hashtable sessiontoken, T inputParameter, out U Result) where T : RestApiMethod where U : RestApiResult
        {
            onNewMessage(string.Format("Do sendRequest Api Call: {0},{1},{2}.",uri,method,contenttype), LogMessageType.Debug);
            
            //Create JavaScript Serializer
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.RegisterConverters(new JavaScriptConverter[] { new NullPropertiesConverter() });

           
            //Create Json
            string json;
            if (inputParameter != null)
                json = js.Serialize(inputParameter);
            else
                json = @"{}"; //Use empty json

            //Set null value to Result
            Result = default(U);

            //create output variable
            string rawResult;

            //send webrequest
            WebResponseResult wrResult =  sendRequest(uri, method, contenttype, sessiontoken, json, out rawResult);

            //deserialize result
            try
            {
                //Check if result has value
                if (rawResult != null)
                {
                    //Do deserialization
                    JavaScriptSerializer ds = new JavaScriptSerializer();
                    Result                  = ds.Deserialize<U>(rawResult);
                }
            }
            catch (Exception ex)
            {
                onNewMessage(string.Format("Unable to deserialize JsonResultString to object of type {0}. {1}",typeof(U),ex.Message), LogMessageType.Error);
            }
            
            //Return Webresponse Result
            return wrResult;
        }

        /// <summary>
        /// Sends a WebRequest to the URI and receive a result (without Sessiontoken; only neccessary for logon)
        /// </summary>
        /// <typeparam name="T">InputParameter Type</typeparam>
        /// <typeparam name="U">OutputParameter Type</typeparam>
        /// <param name="uri">Unified Ressource Identifier</param>
        /// <param name="method">Web Method, Post, Get</param>
        /// <param name="contenttype">The content type</param>        
        /// <param name="inputParameter">InputParameter Value of Type T</param>
        /// <returns>OutputParameter of Type U</returns>
        private WebResponseResult sendRequest<T,U>(string uri, string method, string contenttype, T inputParameter, out U Result) where T : RestApiMethod where U : RestApiResult
        {
            return sendRequest(uri, method, contenttype, null, inputParameter, out Result);
        }


        /// <summary>
        /// Test Deserialization of a json string 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T TestDeserialization<T>(string json)
        {
            //Deserialize Result
            JavaScriptSerializer ds = new JavaScriptSerializer();
            return ds.Deserialize<T>(json);
        }       
    }


    /// <summary>
    /// Web Response Result
    /// </summary>
    class WebResponseResult
    {
        /// <summary>
        /// The ErrorCode a WebServer returns
        /// </summary>
        public int StatusCodeNumber
        {
            get
            {
                return (int)StatusCode;
            }
        }

        /// <summary>
        /// The statuscode a WebServer returns
        /// </summary>
        public HttpStatusCode StatusCode
        { get; set; }

        /// <summary>
        /// The statusdesription a WebServer returns
        /// </summary>
        public string StatusDescription
        { get; set; }
    }

}



