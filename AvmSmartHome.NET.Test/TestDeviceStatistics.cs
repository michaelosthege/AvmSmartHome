using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestDeviceStatistics
    {
        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Deserialize_should_read_response()
        {
            // arrange
            string response = getResourceContent("getbasicdevicestats.xml");

            // act
            DeviceStatistics actual = Helpers.DeserializeXML<DeviceStatistics>(response);

            // assert
            DateTime referenceTime = DateTime.Now;
            Assert.IsNotNull(actual);
            Assert.AreEqual(96, actual.Temperature.Stats.Count);
            Assert.AreEqual(900, actual.Temperature.Stats.Grid);
            Assert.AreEqual(96, actual.Temperature.Stats.Values.Count);
            Assert.AreEqual(24.5, actual.Temperature.Stats.Values[95].Value.Value);
            Assert.IsTrue(referenceTime.Subtract(actual.Temperature.Stats.Values[95].Time).TotalSeconds < 1);
            Assert.AreEqual(double.NaN, actual.Temperature.Stats.Values[0].Value.Value);
            Assert.IsTrue(referenceTime.Subtract(actual.Temperature.Stats.Values[0].Time).TotalSeconds >= 900 * 95);

            Assert.AreEqual(360, actual.Voltage.Stats.Count);
            Assert.AreEqual(10, actual.Voltage.Stats.Grid);
            Assert.AreEqual(360, actual.Voltage.Stats.Values.Count);
            Assert.AreEqual(228.615, actual.Voltage.Stats.Values[359].Value.Value);
            Assert.IsTrue(referenceTime.Subtract(actual.Voltage.Stats.Values[359].Time).TotalSeconds < 1);
            Assert.AreEqual(0, actual.Voltage.Stats.Values[0].Value.Value);
            Assert.IsTrue(referenceTime.Subtract(actual.Voltage.Stats.Values[0].Time).TotalSeconds >= 10 * 359);

            Assert.AreEqual(360, actual.Power.Stats.Count);
            Assert.AreEqual(10, actual.Power.Stats.Grid);
            Assert.AreEqual(238, actual.Power.Stats.Values.Count);
            Assert.AreEqual(10.67, actual.Power.Stats.Values[237].Value.Value);
            Assert.IsTrue(referenceTime.Subtract(actual.Power.Stats.Values[237].Time).TotalSeconds < 1);
            Assert.AreEqual(0, actual.Power.Stats.Values[0].Value.Value);
            Assert.IsTrue(referenceTime.Subtract(actual.Power.Stats.Values[0].Time).TotalSeconds >= 10 * 237);

        }

        private static string getResourceContent(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream resourceStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".TestResources." + resourceName);
            StreamReader reader = new StreamReader(resourceStream);
            string response = reader.ReadToEnd();
            return response;
        }
    }
}
