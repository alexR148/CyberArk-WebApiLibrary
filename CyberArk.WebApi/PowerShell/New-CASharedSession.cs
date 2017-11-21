using System;
using System.Management.Automation;

namespace CyberArk.WebApi.PowerShell
{

    [Cmdlet(VerbsCommon.New, "CASharedSession", SupportsShouldProcess = false)]
    [OutputType (typeof(CyberArk.WebApi.WebServices))]
    public class New_CASharedSession : PSCmdlet
    {

        #region Parameters
        
        [Parameter (Position                = 0,
            Mandatory                       = true,
            ValueFromPipeline               = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage                     = @"A Web URI to the Password Vault WebAccess. For Example 'https://server.abc'")]
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

        WebServices _ws;
        System.Management.Automation.ActionPreference _VerbosePreference;
        System.Management.Automation.ActionPreference _DebugPreference;


        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            _ws = new WebApi.WebServices(BaseURI, PVWAAppName);
            _ws.NewLogMessage += Ws_NewLogMessage;

            _VerbosePreference = (System.Management.Automation.ActionPreference)this.SessionState.PSVariable.GetValue("VerbosePreference");
            _DebugPreference   = (System.Management.Automation.ActionPreference)this.SessionState.PSVariable.GetValue("DebugPreference");
        }

        private void Ws_NewLogMessage(object sender, Logging.MessageArgs e)
        {
            if (e.MessageType == Logging.LogMessageType.Verbose && _VerbosePreference != ActionPreference.SilentlyContinue)
                Host.UI.WriteLine(ConsoleColor.Cyan, Host.UI.RawUI.BackgroundColor, e.ToString());
            else if (e.MessageType == Logging.LogMessageType.Debug && _DebugPreference != ActionPreference.SilentlyContinue)
                Host.UI.WriteLine(ConsoleColor.Gray, Host.UI.RawUI.BackgroundColor, e.ToString());
            else if (e.MessageType == Logging.LogMessageType.Warning)
                Host.UI.WriteLine(ConsoleColor.Yellow, Host.UI.RawUI.BackgroundColor, e.ToString());
            else if (e.MessageType == Logging.LogMessageType.Error)
                Host.UI.WriteLine(ConsoleColor.Red, Host.UI.RawUI.BackgroundColor, e.ToString());
            else if (e.MessageType == Logging.LogMessageType.Info)
                Host.UI.WriteLine(ConsoleColor.Green, Host.UI.RawUI.BackgroundColor, e.ToString());
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
