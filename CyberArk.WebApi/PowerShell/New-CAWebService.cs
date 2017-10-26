using System.Management.Automation;

namespace CyberArk.WebApi.PowerShell
{

    [Cmdlet(VerbsCommon.New,"CAWebService",SupportsShouldProcess = false)]
    [OutputType (typeof(WebServices))]
    public class New_CAWebService : Cmdlet
    {

        #region Parameters
        
        [Parameter (Position                = 0,
            Mandatory                       = true,
            ValueFromPipeline               = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage                     = @"A Web URI to the Password Vault WebAccess. For Example 'https://PVWAServer.testdomain.com'")]
        public string BaseURI
        { get; set; }

        [Parameter(Position = 1,
            Mandatory                       = false,
            ValueFromPipeline               = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage                     = @"The name of the password vault")]
        public string PVWAAppName
        { get; set; }

        #endregion

        #region Processing

        protected override void BeginProcessing()
        {
            base.BeginProcessing();            
        }

        

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            
            WebServices ws = new WebApi.WebServices(BaseURI,PVWAAppName);
            //ws.NewMessage += _ws_NewMessage;
            WriteObject(ws);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        #endregion       
        
    }
}
