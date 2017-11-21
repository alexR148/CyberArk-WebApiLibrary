using CyberArk.WebApi.Container;
using CyberArk.WebApi.Logging;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {
        /// <summary>
        /// List all Safemembers of a Safe
        /// </summary>
        /// <param name="SafeName">The name of a Safe</param>
        /// <returns></returns>
        public PSSafeMembers_Result List_PASSafeMembers(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = System.Uri.EscapeUriString(string.Format ("{0}{1}/{2}/Members", WebURI,URI,SafeName));

            //Create Safe Add Object
            NullableInput memberParameters = new NullableInput();
            onNewMessage(string.Format("List members of safe '{0}'", SafeName), LogMessageType.Info);

            //Do Api Call            
            WebResponseResult wrResult;
            ListSafeMembers_Result result = sendRequest<NullableInput, ListSafeMembers_Result>(uri, VERB_METHOD_GET, JSON_CONTENT_TYPE, SessionToken, memberParameters,out wrResult);

            //apply sessioninformation to result (neccessary for Powershell)
            foreach (SafeMember_Result r in result.members)
                applySessionInfo(r);

            //Get Result
            PSSafeMembers_Result psResult = null;
            if (result != null)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafeMembers_Result>(result);                
                onNewMessage(string.Format("Safe '{0}' successfully queried", SafeName), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to query safe '{0}'", SafeName), LogMessageType.Error);

            return psResult;
        }
    }
}
