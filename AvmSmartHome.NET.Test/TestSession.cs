using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestSession
    {
        string USERNAME = "testuser";
        string PASSWORD = "testpassword";
        string HOSTNAME = "fritz.box";

        [TestMethod]
        public void ChallengeHashing()
        {
            string challenge = "1234567z";
            string password = "äbc";
            string expected = "9e224a41eeefa284df7bb0f26c2913e2";
            string actual = Helpers.ComputeMD5($"{challenge}-{password}", Encoding.Unicode);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task SessionCreation()
        {
            SessionInfo session = new SessionInfo(USERNAME, PASSWORD, HOSTNAME);
            await session.AuthenticateAsync();
        }

        [TestMethod]
        public async Task GetSwitchList()
        {
            SessionInfo session = new SessionInfo(USERNAME, PASSWORD, HOSTNAME);
            await session.AuthenticateAsync();
            string[] switches = await session.GetSwitchesAsync();
        }

        [TestMethod]
        public async Task GetSwitchTemperatures()
        {
            SessionInfo session = new SessionInfo(USERNAME, PASSWORD, HOSTNAME);
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
            SessionInfo session = new SessionInfo(USERNAME, PASSWORD, HOSTNAME);
            await session.AuthenticateAsync();
            string ain = "087610251884";

            bool? initial_state = await session.GetSwitchStateAsync(ain);

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

    }
}
