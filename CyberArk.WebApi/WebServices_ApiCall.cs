using CyberArk.WebApi.Internal;
using CyberArk.WebApi.Logging;
using CyberArk.WebApi.Container;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {
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
        /// <param name="wrResult">The result of WebResponse</param>
        /// <returns>OutputParameter of Type U</returns>
        private U sendRequest<T, U>(string uri, string method, string contenttype, Hashtable sessiontoken, T inputParameter, out WebResponseResult wrResult) where T : RestApiMethod where U : RestApiResult
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

            //Only allow TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            //Create WebRequest
            WebRequest restRequest  = WebRequest.Create(uri);
            restRequest.Method      = method;
            restRequest.ContentType = contenttype;
            

            //Apply Sessiontoken
            if (sessiontoken != null && sessiontoken.Count > 0)
                restRequest.Headers[SESSION_TOKEN_HEADER] = sessiontoken[SESSION_TOKEN_HEADER].ToString();

            U result;
            try
            {
                //Only send request for Post method
                if (restRequest.Method == VERB_METHOD_POST)
                {
                    //Send Webrequest
                    using (Stream requestStream = restRequest.GetRequestStream())
                    {
                        //Get Bytes from json 
                        byte[] inputStringBytes = Encoding.UTF8.GetBytes(json);

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
                        StreamReader sr  = new StreamReader(responseStream, Encoding.UTF8);
                        string rawResult = sr.ReadToEnd();

                        //Deserialize Result
                        JavaScriptSerializer ds = new JavaScriptSerializer();
                        result                  = ds.Deserialize<U>(rawResult);                                                                 
                    }
                    HttpWebResponse res = ((HttpWebResponse)(restResponse));
                    onNewMessage(string.Format("{0},{1} - {2}", (int)res.StatusCode,res.StatusCode,res.StatusDescription), LogMessageType.Debug);
                    wrResult = new WebResponseResult() { StatusCode = res.StatusCode, StatusDescription = res.StatusDescription };

                }
                //Return result
                onNewMessage(string.Format("Api Call successfully done."), LogMessageType.Debug);
                return result;
            }
            catch (WebException ex)
            {
                HttpWebResponse res = ((HttpWebResponse)(ex.Response));
                onNewMessage(string.Format("{0},{1} - {2} {3}", (int)res.StatusCode, res.StatusCode,ex.Message,res.StatusDescription  ), LogMessageType.Error);
                wrResult = new WebResponseResult() { StatusCode = res.StatusCode, StatusDescription = res.StatusDescription };
            }
            return default(U);
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
        private U sendRequest<T, U>(string uri, string method, string contenttype, T inputParameter,out WebResponseResult wrResult) where T : RestApiMethod where U : RestApiResult
        {
            return sendRequest<T, U>(uri, method, contenttype, null, inputParameter,out wrResult);
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



