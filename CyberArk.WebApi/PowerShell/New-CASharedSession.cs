using System.Management.Automation;

namespace CyberArk.WebApi.PowerShell
{

    [Cmdlet(VerbsCommon.New, "CASharedSession", SupportsShouldProcess = false)]
    [OutputType (typeof(CyberArk.WebApi.WebServices))]
    public class New_CASharedSession : Cmdlet
    {

        #region Parameters
        
        [Parameter (Position                = 0,
            Mandatory                       = true,
            ValueFromPipeline               = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage                     = @"A Web URI to the Password Vault WebAccess. For Example 'https://FTPSIBAT020.adminlan.izb'")]
        public string BaseURI
        { get; set; }

        [Parameter(Position = 1,
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = @"The name of the password vault")]
        public string PVWAAppName
        { get; set; }
        = "PasswordVault";

        #endregion

        #region Processing

        protected override void BeginProcessing()
        {
            base.BeginProcessing();            
        }

        

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            
            WebServices ws = new WebServices(BaseURI,PVWAAppName);            
            WriteObject(ws.LogOn());
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
