using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestErros
    {        
        [TestMethod]
        public async Task InvalidCredentials()
        {
            SessionInfo session = new SessionInfo("nsa", "masterpassword", TestCredentials.HOSTNAME);
            await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(async delegate
            {
                await session.AuthenticateAsync();
            });

            // wait because other tests would fail due to the brute-force protection
            await Task.Delay(5000);
        }

        [TestMethod]
        public async Task InvalidAIN()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();
            
            string ain = "012345678910";

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async delegate
            {
                bool? state = await session.GetSwitchStateAsync(ain);
            });
            
        }
        
    }
}
