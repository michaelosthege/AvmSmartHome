using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestPowerValue
    {
        [TestMethod]
        public void Convert_should_use_a_factor()
        {
            // Arrange
            double rawValue = 100;
            var actual = new PowerValue();

            // Act
            actual.Convert(rawValue);

            // Assert
            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod]
        public void Constructor_with_double_parameter_should_use_a_factor()
        {
            // Arrange
            double rawValue = 100;

            // Act
            var actual = new PowerValue(rawValue);

            // Assert
            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod]
        public void Constructor_with_string_parameter_should_use_a_factor()
        {
            // Arrange
            string rawValue = "100";

            // Act
            var actual = new PowerValue(rawValue);

            // Assert
            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod]
        public void Constructor_with_string_parameter_should_handle_invalid_value()
        {
            // Arrange
            string rawValue = "-";

            // Act
            var actual = new PowerValue(rawValue);

            // Assert
            Assert.AreEqual(double.NaN, actual.Value);
        }
    }
}
