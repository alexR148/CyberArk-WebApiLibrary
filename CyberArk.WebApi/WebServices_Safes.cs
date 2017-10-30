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
        public PSSafeResult Add_PASSafe(string SafeName, int NumberOfVersionsRetention, int NumberOfDaysRetention
            , string Description = null, bool? OLACEnabled = null, string ManagingCPM = null)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = WebURI + URI;

            //Create Safe Add Object
            AddSafe_Parameter safeParameters               = new AddSafe_Parameter();
            safeParameters.safe.SafeName                   = SafeName;
            safeParameters.safe.NumberOfVersionsRetention  = NumberOfVersionsRetention;
            safeParameters.safe.NumberOfDaysRetention      = NumberOfDaysRetention;
            safeParameters.safe.Description                = Description;
            safeParameters.safe.OLACEnabled                = OLACEnabled;
            safeParameters.safe.ManagingCPM                = ManagingCPM;

            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);
            AddSafe_Result result = sendRequest<AddSafe_Parameter, AddSafe_Result>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE,SessionToken, safeParameters);


            //Get Result
            PSSafeResult psResult = null; 
            if (result != null)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafeResult>();
                copyProperties(result.safe, psResult);
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
        public PSSafeResult Get_PASSafe(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = WebURI + URI + "/" + SafeName;

            //Create Safe Add Object
            NullableInput  safeParameters = new NullableInput();
            
            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);
            GetSafe_Result result = sendRequest<NullableInput, GetSafe_Result>(uri, VERB_METHOD_GET, JSON_CONTENT_TYPE, SessionToken, safeParameters);

            //Get Result
            PSSafeResult psResult = null;
            if (result != null)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafeResult>();
                copyProperties(result.GetSafeResult, psResult);
                onNewMessage(string.Format("Safe '{0}' successfully queried", SafeName), LogMessageType.Info);              
            }
            else
                onNewMessage(string.Format("Unable to query safe '{0}'", SafeName), LogMessageType.Error);

            return psResult;
        }

    }
}
