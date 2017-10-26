using CyberArk.WebApi.Container;
using CyberArk.WebApi.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SafeResult Add_PASSafe(string SafeName, int NumberOfVersionsRetention, int NumberOfDaysRetention
            , string Description = null, bool? OLACEnabled = null, string ManagingCPM = null)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = WebURI + URI;

            //Create Safe Add Object
            Add_Safe safeadd                         = new Add_Safe();
            safeadd.safe.SafeName                   = SafeName;
            safeadd.safe.NumberOfVersionsRetention  = NumberOfVersionsRetention;
            safeadd.safe.NumberOfDaysRetention      = NumberOfDaysRetention;
            safeadd.safe.Description                = Description;
            safeadd.safe.OLACEnabled                = OLACEnabled;
            safeadd.safe.ManagingCPM                = ManagingCPM;

            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);
            Add_SafeResult result = sendRequest<Add_Safe, Add_SafeResult>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE,SessionToken, safeadd);


            //Get Result
            SafeResult presult = createApiResults<SafeResult>();
            if (result != null)
            {
                onNewMessage(string.Format("Safe '{0}' successfully created", SafeName), LogMessageType.Info);

                //Set Results
                copyProperties(result.safe, presult);               
            }
            else
                onNewMessage(string.Format("Unable to create safe '{0}'", SafeName), LogMessageType.Info);

            return presult;
        }


        public SafeResult Get_PASSafe(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri = WebURI + URI + "/" + SafeName;

            //Create Safe Add Object
            NullableInput  safeadd = new NullableInput();
            

            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);
            Get_SafeResult result = sendRequest<NullableInput, Get_SafeResult>(uri, VERB_METHOD_GET, JSON_CONTENT_TYPE, SessionToken, safeadd);


            //Get Result
            SafeResult presult = null;
            if (result != null)
            {

                onNewMessage(string.Format("Safe '{0}' successfully created", SafeName), LogMessageType.Info);

                //Get Results
                presult = createApiResults<SafeResult>();
                copyProperties(result.GetSafeResult, presult);
            }
            else
                onNewMessage(string.Format("Unable to create safe '{0}'", SafeName), LogMessageType.Info);

            return presult;
        }

    }
}
