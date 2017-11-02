using CyberArk.WebApi.Container;
using CyberArk.WebApi.Logging;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {

        public PSSafeMembersResult List_PASSafeMembers(string SafeName)
        {
            const string URI = @"/WebServices/PIMServices.svc/Safes";
            string uri       = string.Format ("{0}{1}/{2}/Members", WebURI,URI,SafeName);

            //Create Safe Add Object
            NullableInput memberParameters = new NullableInput();
            onNewMessage(string.Format("List members of safe '{0}'", SafeName), LogMessageType.Info);

            //Do Api Call            
            ListSafeMembers_Result result = sendRequest<NullableInput, ListSafeMembers_Result>(uri, VERB_METHOD_GET, JSON_CONTENT_TYPE, SessionToken, memberParameters);

            //apply sessioninformation to result (neccessary for Powershell)
            foreach (SafeMember_Result r in result.members)
                applySessionInfo(r);

            //Get Result
            PSSafeMembersResult psResult = null;
            if (result != null)
            {
                //Create PSResult
                psResult = createPSApiResults<PSSafeMembersResult>(result);                
                onNewMessage(string.Format("Safe '{0}' successfully queried", SafeName), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to query safe '{0}'", SafeName), LogMessageType.Error);

            return psResult;




        }




    }
}
