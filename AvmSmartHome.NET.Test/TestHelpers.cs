using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestHelpers
    {
        [TestMethod]
        public void ChallengeHashing()
        {
            string challenge = "1234567z";
            string password = "äbc";
            string expected = "9e224a41eeefa284df7bb0f26c2913e2";
            string actual = Helpers.ComputeMD5($"{challenge}-{password}", Encoding.Unicode);
            Assert.AreEqual(expected, actual);
        }
    }
}
