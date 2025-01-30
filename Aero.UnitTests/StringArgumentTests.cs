using Aero.Application.Workers;
using Microsoft.Extensions.Configuration;

namespace Aero.UnitTests;

public class StringArgumentTests
{
        [Test]
        public void GetValue_ConfigHasKey_ReturnsConfiguredValue()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "TestArg", "TestArgValue" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var argument = new StringArgument("TestArg", isRequired: false, "Description");

            // Act
            var value = argument.GetValue(configuration);

            // Assert
            Assert.That(value, Is.EqualTo("TestArgValue"), "Expected to retrieve the configured value from IConfiguration.");
        }

        [Test]
        public void GetValue_ConfigMissingKeyButHasDefault_ReturnsDefaultValue()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>()) // empty
                .Build();

            var argument = new StringArgument("TestArg", isRequired: false, defaultValue: "DefaultArgValue", description: "Description");

            // Act
            var value = argument.GetValue(configuration);

            // Assert
            Assert.That(value, Is.EqualTo("DefaultArgValue"), "Expected to use the default value since the key was missing and HasDefaultValue is true.");
        }

        [Test]
        public void GetValue_ConfigMissingKeyNoDefaultNotRequired_ReturnsNull()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>()) // empty
                .Build();

            var argument = new StringArgument("TestArg", isRequired: false, description: "Description");

            // Act
            var value = argument.GetValue(configuration);

            // Assert
            Assert.That(value, Is.Null, "Expected null (default for string) because the argument is not required and has no default value.");
        }

        [Test]
        public void GetValue_ConfigMissingKeyButRequired_ThrowsArgumentException()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>()) //empty
                .Build();

            var argument = new StringArgument("TestArg", isRequired: true, description: "Description");

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => argument.GetValue(configuration),
                "Expected an ArgumentException because the argument is required but not present in configuration."
            );
        }

        [Test]
        public void Constructor_RequiredWithNoDefault_HasDefaultValueIsFalse()
        {
            // Arrange & Act
            var argument = new StringArgument("TestArg", isRequired: true, description: "Description");

            // Assert
            Assert.IsFalse(argument.HasDefaultValue, "A constructor without a default value should set HasDefaultValue to false.");
        }

        [Test]
        public void Constructor_RequiredWithDefault_HasDefaultValueIsTrue()
        {
            // Arrange & Act
            var argument = new StringArgument("TestArg", isRequired: true, defaultValue: "default", description: "Description");

            // Assert
            Assert.IsTrue(argument.HasDefaultValue, "A constructor that provides a default value should set HasDefaultValue to true.");
        }
}