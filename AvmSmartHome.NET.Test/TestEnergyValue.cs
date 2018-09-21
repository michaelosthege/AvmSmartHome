using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvmSmartHome.NET.Test
{
    [TestClass]
    public class TestEnergyValue
    {
        [TestMethod]
        public void Convert_should_not_use_a_factor()
        {
            // Arrange
            double rawValue = 1;
            var actual = new EnergyValue();

            // Act
            actual.Convert(rawValue);

            // Assert
            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod]
        public void Constructor_with_double_parameter_should_not_use_a_factor()
        {
            // Arrange
            double rawValue = 1;

            // Act
            var actual = new EnergyValue(rawValue);

            // Assert
            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod]
        public void Constructor_with_string_parameter_should_not_use_a_factor()
        {
            // Arrange
            string rawValue = "1";

            // Act
            var actual = new EnergyValue(rawValue);

            // Assert
            Assert.AreEqual(1, actual.Value);
        }

        [TestMethod]
        public void Constructor_with_string_parameter_should_handle_invalid_value()
        {
            // Arrange
            string rawValue = "-";

            // Act
            var actual = new EnergyValue(rawValue);

            // Assert
            Assert.AreEqual(double.NaN, actual.Value);
        }
    }
}
