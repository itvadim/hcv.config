namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

using Microsoft.Extensions.Logging.Abstractions;

public class VaultSecretConfigurationOptionalShould
{
	[Fact]
	public void InitializeWithDefaultLoggerInstance()
	{
		// Arrange & Act
		var optional = new VaultSecretConfigurationOptional();

		// Assert
		optional.Logger.Should().BeOfType<NullLogger>();
	}

	[Fact]
	public void AllowChangingLoggerInstance()
	{
		// Arrange
		var optional = new VaultSecretConfigurationOptional();
		var customLogger = new NullLogger<string>();

		// Act
		optional.Logger = customLogger;

		// Assert
		optional.Logger.Should().Be(customLogger);
	}

	[Fact]
	public void HaveDefaultKeyDelimiterSetToDoubleDash()
	{
		// Arrange & Act
		var optional = new VaultSecretConfigurationOptional();

		// Assert
		optional.KeyDelimiter.Should().Be("--");
	}

	[Fact]
	public void AllowChangingTheKeyDelimiter()
	{
		// Arrange
		var optional = new VaultSecretConfigurationOptional();
		var newDelimiter = "::";

		// Act
		optional.KeyDelimiter = newDelimiter;

		// Assert
		optional.KeyDelimiter.Should().Be(newDelimiter);
	}
}