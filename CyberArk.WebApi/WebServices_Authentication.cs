using CyberArk.WebApi.Internal;
using CyberArk.WebApi.Logging;
using CyberArk.WebApi.Container;
using System.Security;

namespace CyberArk.WebApi
{
    public partial class WebServices
    {                     
        /// <summary>
        /// Logon using CyberArk Authentication
        /// </summary>
        /// <param name="username">The name of the user who will logon to the Vault.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="newPassword">The new password of the user. This parameter is optional, and enables you to change a password.</param>
        /// <param name="useRadiusAuthentication">Whether or not users will be authenticated via a RADIUS server. Valid values: true/false</param>
        /// <param name="connectionNumber">In order to allow more than one connection for the same user simultaneously, each request should be sent with different 'connectionNumber'. Valid values: 0-100</param>
        public PSApiResult LogOn(string username, SecureString password, SecureString newPassword = null,bool? useRadiusAuthentication = null, int connectionNumber = 0) 
        {
            const string URI     = @"/WebServices/auth/Cyberark/CyberArkAuthenticationService.svc/Logon";
            string uri           = WebURI + URI;
          
            onNewMessage(string.Format("LogOn to PasswordVault using CyberArk Authentication with user '{0}'",username), LogMessageType.Info);

            //Set ConnectionNumber
            if (connectionNumber != 0)
                ConnectionNumber = connectionNumber.ToString();

            //Create Authentication Logon Object
            AuthLogon logonParameter                 = new AuthLogon();
            logonParameter.username                  = username;
            logonParameter.password                  = password;
            logonParameter.newPassword               = newPassword;
            logonParameter.useRadiusAuthentication   = useRadiusAuthentication;
            logonParameter.connectionNumber          = connectionNumber;
            onNewMessage(string.Format("Authentication Logon Object successfully created."), LogMessageType.Verbose);

            //Do Api Call
            onNewMessage(string.Format("Sending LogOn request to '{0}' with Method '{1}' and Content '{2}'",uri, VERB_METHOD_POST,JSON_CONTENT_TYPE), LogMessageType.Debug);
            AuthLogonResult result = sendRequest<AuthLogon, AuthLogonResult>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE, logonParameter);
            
            //Get Result
            if (result != null)
            {
                onNewMessage(string.Format("LogOn request successfully sent to '{0}'", uri), LogMessageType.Verbose);
                SessionToken.Add(SESSION_TOKEN_HEADER, result.CyberArkLogonResult);
                Authenticationtype  = AuthenticationType.CyberArk;
                onNewMessage(string.Format("User '{0}' successfully connected to PasswordVault", username), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to connect user '{0}' to PasswordVault", username), LogMessageType.Error);

            //Return result
            return createPSApiResults<PSApiResult>();                        
        }

        /// <summary>
        /// Logoff Session depending on Authenticationtype
        /// </summary>
        public void LogOff()
        {
            const string URI_CYBERARK = @"/WebServices/auth/Cyberark/CyberArkAuthenticationService.svc/Logoff";
            const string URI_SHARED   = @"/WebServices/auth/Shared/RestfulAuthenticationService.svc/Logoff";
            string uri;

            onNewMessage(string.Format("Disconnect from session."), LogMessageType.Info);

            //Set Logoff Method depending on LogonType
            switch (Authenticationtype)
            {
                case AuthenticationType.CyberArk:
                    uri   = WebURI + URI_CYBERARK;                   
                    break;
                case AuthenticationType.SharedLogon:
                    uri   = WebURI + URI_SHARED;                  
                    break;
                default:
                    return;  
            }

            //Do Api Call
            onNewMessage(string.Format("Sending LogOff request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);
            sendRequest<NullableInput, NullableOutput>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE,SessionToken, new NullableInput());

            //Reset Token
            SessionToken.Clear();
            ConnectionNumber   = string.Empty;
            Authenticationtype = AuthenticationType.None;
            onNewMessage(string.Format("Session successfully disconnected. Sessiontoken successfully resetted."), LogMessageType.Info);
        }

        /// <summary>
        /// Logon with Shared Authentication
        /// Shared authentication is based on a user credential file that is stored in the PVWA web
        ///server.During shared authentication, only the user defined in the credential file can logon
        ///to the PVWA, but multiple users can use the logon token.
        ///This type of authentication requires the application using the REST services to manage
        ///the users as the Vault can't identify which specific user performs each action.
        ///Multiple concurrent connections can be created using the same token, without affecting
        ///each other.
        ///The shared user is defined in a user credential file, whose location is specified in the
        ///WSCredentialFile parameter, in the appsettings section of the PVWA web.config file.       
        ///Make sure that this user can access the the PVWA interface.
        ///Make sure the user only has the permissions in the Vault that they require.
        /// </summary>
        public PSApiResult LogOn()
        {
            const string URI = @"/WebServices/auth/Shared/RestfulAuthenticationService.svc/Logon";
            string uri       = WebURI + URI;

            onNewMessage(string.Format("LogOn to PasswordVault using CyberArk Shared Authentication"), LogMessageType.Info);

            //Create Authentication Logon Object
            NullableInput logonParameter = new NullableInput();
            onNewMessage(string.Format("Authentication Logon Object successfully created."), LogMessageType.Verbose);

            //Do Api Call
            onNewMessage(string.Format("Sending Shared LogOn request to '{0}' with Method '{1}' and Content '{2}'", uri, VERB_METHOD_POST, JSON_CONTENT_TYPE), LogMessageType.Debug);
            SharedAuthLogonResult result = sendRequest<NullableInput, SharedAuthLogonResult>(uri, VERB_METHOD_POST, JSON_CONTENT_TYPE, logonParameter);

            //Validate result
            if (result != null)
            {                       
                Authenticationtype  = AuthenticationType.SharedLogon;
                onNewMessage(string.Format("Successfully connected to PasswordVault"), LogMessageType.Info);
            }
            else
                onNewMessage(string.Format("Unable to connect to PasswordVault using Shared Authentication"), LogMessageType.Error);
            
            //Get Result
            SessionToken.Add(SESSION_TOKEN_HEADER, result.LogonResult);
            Authenticationtype = AuthenticationType.SharedLogon;

            //Return result
            return createPSApiResults<PSApiResult>();
        }
    }
}
