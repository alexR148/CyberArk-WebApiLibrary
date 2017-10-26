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
        /// <returns>OutputParameter of Type U</returns>
        private U sendRequest<T, U>(string uri, string method, string contenttype, Hashtable sessiontoken, T inputParameter) where T : RestApiInputParameter where U : RestApiResult
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

                        //Add additional Info 
                        //result.PVWAAppName   = this.PVWAAppName;
                        //result.sessionToken  = this.SessionToken;
                        //result.BaseURI       = this.BaseURI;
                    }
                }
                //Return result
                onNewMessage(string.Format("Api Call successfully done."), LogMessageType.Debug);
                return result;
            }
            catch (WebException ex)
            {
                HttpWebResponse res = ((HttpWebResponse)(ex.Response));
                onNewMessage(string.Format("{0} - {1}", ex.Message,res.StatusDescription  ), LogMessageType.Error);
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
        private U sendRequest<T, U>(string uri, string method, string contenttype, T inputParameter) where T : RestApiInputParameter where U : RestApiResult
        {
            return sendRequest<T, U>(uri, method, contenttype, null, inputParameter);
        }

    }



}



