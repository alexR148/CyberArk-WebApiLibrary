using System;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security;

namespace CyberArk.WebApi.Extensions
{
    public static class WebServicesExtensions
    {
        /// <summary>
        /// Creates SecureString from String
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string password)
        {
            if (password == null)
                throw new ArgumentException("password");

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;                     
                }
            }
                                 
            //SecureString s = new SecureString();
            //char[] chars   = input.ToCharArray();

            //foreach (char c in chars)
            //    s.AppendChar(c);

            //s.MakeReadOnly();
            //return s;
        }

        /// <summary>
        /// Creates String from SecureString
        /// </summary>
        /// <param name="securePW"></param>
        /// <returns></returns>
        public static string ToUnsecureString(this SecureString securePW)
        {
            if (securePW == null)
                throw new ArgumentException("securePW");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePW);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

       
       


     

    }   
}
