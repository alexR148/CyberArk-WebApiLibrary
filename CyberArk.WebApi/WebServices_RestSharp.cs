using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;
using CyberArk.WebApi.Internal;

namespace CyberArk.WebApi
{
    partial class WebServices
    {
        private IRestResponse<T> sendRequestRestSharp<T>(string BaseUri, string resource, Method method, string contenttype, Hashtable header, object body) where T :  new()
        {
            var client = new RestClient(BaseUri);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            //var request = new RestRequest("resource/{id}", method);
            var request = new RestRequest(resource, method);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource
            request.AddHeader("ContentType", contenttype);
            request.RequestFormat = DataFormat.Json;
            // add parameters for all properties on an object
            //request.AddObject(body);

            //Create JavaScript Serializer
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.RegisterConverters(new JavaScriptConverter[] { new NullPropertiesConverter() });

            //Create Json
            string json;
            if (body != null)
                json = js.Serialize(body);
            else
                json = @"{}"; //Use empty json

            request.AddJsonBody(json);

            // or just whitelisted properties
            //request.AddObject(object, "PersonId", "Name", ...);

            // easily add HTTP Headers
            if (header != null && header.Count > 0)
            {
                
                foreach (DictionaryEntry de in header)
                {
                    string fieldName = de.Key.ToString();
                    string value = de.Value.ToString();
                    request.AddHeader(fieldName, value);
                }                             
            }

            // add files to upload (works with compatible verbs)
            //request.AddFile("file", path);

            // execute the request
            //IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            IRestResponse<T> response2 = client.Execute<T>(request);
            //var name = response2.Data.Name;

            // or download and save file to disk
            //client.DownloadData(request).SaveAs(path);

            // easy async support
            //await client.ExecuteAsync(request);
            //dynamic result = null;
            
            // async with deserialization
            //var asyncHandle = client.ExecuteAsync<T>(request, response => {
            //    result = response;
            //});
            
            // abort the request on demand
            //asyncHandle.Abort();

            return response2;

        }


       




    }


    //class RestSharpResult<T> : IRestResponse<T>
    //{
    //    public string Content
    //    {
    //         get; set; 
            
    //    }

    //    public string ContentEncoding
    //    {
    //        get; set;
    //    }

    //    public long ContentLength
    //    {
    //        get; set;
    //    }

    //    public string ContentType
    //    {
    //        get; set;
    //    }

    //    public IList<RestResponseCookie> Cookies
    //    {
    //        get; set;
    //    }

        
    //    public T Data
    //    {
    //        get; set;
    //    }

    //    public Exception ErrorException
    //    {
    //        get; set;
    //    }

    //    public string ErrorMessage
    //    {
    //        get; set;
    //    }

    //    public IList<Parameter> Headers
    //    {
    //        get; 
    //    }

    //    public bool IsSuccessful
    //    {
    //        get; 
    //    }

    //    public Version ProtocolVersion
    //    {
    //        get; set;
    //    }

    //    public byte[] RawBytes
    //    {
    //        get; set;
    //    }

    //    public IRestRequest Request
    //    {
    //        get; set;
    //    }

    //    public ResponseStatus ResponseStatus
    //    {
    //        get; set;
    //    }

    //    public Uri ResponseUri
    //    {
    //        get; set;
    //    }

    //    public string Server
    //    {
    //        get; set;
    //    }

    //    public HttpStatusCode StatusCode
    //    {
    //        get; set;
    //    }

    //    public string StatusDescription
    //    {
    //        get; set;
    //    }
    //}

}
