using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CyberArk.WebApi.Tests
{
    [TestClass()]
    public class WebServicesTests
    {
        const string PVWS = @"http://172.20.139.40/PasswordVault";




        [TestMethod()]
        public void LogOn_BuiltInAccount_LiveTest()
        {
            //WebServices ws = new WebServices(PVWS);
            //ws.LogOn("ghz", @"fgdfg");

            //bool connected = !string.IsNullOrWhiteSpace(ws.SessionToken);

            //Assert.IsTrue(connected);
        }

        
    }
}