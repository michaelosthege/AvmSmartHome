using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestSession
    {        
        [TestMethod]
        public async Task SessionCreation()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();
        }

        [TestMethod]
        public async Task GetSwitchList()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();
            string[] switches = await session.GetSwitchesAsync();
        }

        [TestMethod]
        public async Task GetSwitchTemperatures()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();
            string[] switches = await session.GetSwitchesAsync();
            foreach (string ain in switches)
            {
                string name = await session.GetSwitchNameAsync(ain);
                double power = await session.GetSwitchPowerAsync(ain);
                double energy = await session.GetSwitchEnergyAsync(ain);
                double temp = await session.GetSwitchTemperatureAsync(ain);

                string message = $"Switch {ain}" +
                    $"\r\n name   = {name}" +
                    $"\r\n P [mW] = {power}" +
                    $"\r\n E [Wh] = {energy}" +
                    $"\r\n T [°C] = {temp}";
                Debug.WriteLine(message);
            }
        }

        [TestMethod]
        public async Task TestActuator()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();
            string ain = "087610251884";

            bool? initial_state = await session.GetSwitchStateAsync(TestCredentials.LampAIN);

            switch (initial_state)
            {
                case true: Debug.WriteLine($"{ain} ist AN"); break;
                case false: Debug.WriteLine($"{ain} ist AUS"); break;
                case null: Debug.WriteLine($"{ain} ist UNBEKANNT"); break;
            }

            Assert.IsNotNull(initial_state);

            await Task.Delay(500);

            bool temp_state = await session.SetSwitchAsync(ain, initial_state == false);
            await Task.Delay(1500);

            bool restored_state = await session.ToggleSwitchAsync(ain);
            await Task.Delay(1500);

            bool? check_state = await session.GetSwitchStateAsync(ain);

            Assert.IsTrue(initial_state == check_state);
        }

        [TestMethod]
        public async Task TestDeviceListInfos()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();
            
            var result = await session.GetDeviceListInfosAsync();

            Assert.IsTrue(result.Device.Count > 0);
        }

        [TestMethod]
        public async Task TestSetSimpleOnOffAsync()
        {
            SessionInfo session = new SessionInfo(TestCredentials.USERNAME, TestCredentials.PASSWORD, TestCredentials.HOSTNAME);
            await session.AuthenticateAsync();

            var deviceList = await session.GetDeviceListInfosAsync();
            var compatibleDevice = deviceList.Device.FirstOrDefault(d => d.SimpleOnOff != null);

            if (compatibleDevice == null)
            {
                Assert.Fail("No compatible device found!");
            }
            else
            {
                var result = await session.SetSimpleOnOffAsync(compatibleDevice.Identifier, SimpleOnOffStates.Toggle);
            }
        }
    }
}
