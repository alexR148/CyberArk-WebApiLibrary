using CyberArk.WebApi.Container;
using CyberArk.WebApi.Logging;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {

        /// <summary>
        /// This method adds a new Safe to the Vault. The user who runs this web service requires the following permission in the Vault: Add Safes
        /// </summary>
        /// <param name="SafeName"></param>
        /// <param name="NumberOfVersionsRetention"></param>
        /// <param name="NumberOfDaysRetention"></param>
        /// <param name="Description"></param>
        /// <param name="OLACEnabled"></param>
        /// <param name="ManagingCPM"></param>
        public PSSafe_Result Add_PASSafe(string SafeName, int NumberOfVersionsRetention, int NumberOfDaysRetention
            , string Description = null, bool? OLACEnabled = null, string ManagingCPM = null)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = WebURI + URI;

            //Create Safe Add Object
            AddSafe_Method safeParameters               = new AddSafe_Method();
            safeParameters.safe.SafeName                   = SafeName;
            safeParameters.safe.NumberOfVersionsRetention  = NumberOfVersionsRetention;
            safeParameters.safe.NumberOfDaysRetention      = NumberOfDaysRetention;
            safeParameters.safe.Description                = Description;
            safeParameters.safe.OLACEnabled                = OLACEnabled;
            safeParameters.safe.ManagingCPM                = ManagingCPM;

            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);            
            WebResponseResult<AddSafe_Result> wrResult = sendRequest<AddSafe_Result>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE,SessionToken, safeParameters);


            //Get Result
            PSSafe_Result psResult = null; 
            if (wrResult != null && wrResult.Data != null && wrResult.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafe_Result>(wrResult.Data.AddSafeResult);                
                onNewMessage(string.Format("Safe '{0}' successfully created", SafeName), LogMessageType.Info);                           
            }
            else
                onNewMessage(string.Format("Unable to create safe '{0}'", SafeName), LogMessageType.Error);

            return psResult;
        }

        /// <summary>
        /// Get properties of a specified safe 
        /// </summary>
        /// <param name="SafeName"></param>
        /// <returns></returns>
        public PSSafe_Result Get_PASSafe(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = System.Uri.EscapeUriString( WebURI + URI + "/" + SafeName);
            

            //Create Safe Add Object
            NullableInput  safeParameters = new NullableInput();
            
            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);           
            WebResponseResult<GetSafe_Result> wrResult = sendRequest<GetSafe_Result>(uri, VERB_METHOD_GET, JSON_CONTENT_TYPE, SessionToken, safeParameters);

            //Get Result
            PSSafe_Result psResult = null;
            if (wrResult != null && wrResult.Data != null && wrResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafe_Result>(wrResult.Data.GetSafeResult);                
                onNewMessage(string.Format("Safe '{0}' successfully queried", SafeName), LogMessageType.Info);              
            }
            else
                onNewMessage(string.Format("Unable to query safe '{0}'", SafeName), LogMessageType.Error);

            return psResult;
        }

        /// <summary>
        /// Removes a Safe
        /// </summary>
        /// <param name="SafeName">the name of the safe which has to be removed</param>
        /// <returns></returns>
        public NullableOutput Remove_PASSafe(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri = System.Uri.EscapeUriString(WebURI + URI + "/" + SafeName);
          
            //Do Api Call
            onNewMessage(string.Format("Sending RemoveSafe request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_DELETE, JSON_CONTENT_TYPE), LogMessageType.Debug);          
            WebResponseResult<NullableOutput> wrResult = sendRequest<NullableOutput>(uri, VERB_METHOD_DELETE, JSON_CONTENT_TYPE, SessionToken, new NullableInput());

            //Get Result          
            if (wrResult != null && wrResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Create PSResult               
                onNewMessage(string.Format("Safe '{0}' successfully deleted", SafeName), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to delete safe '{0}'", SafeName), LogMessageType.Error);

            return new NullableOutput();
        }

    }
}
