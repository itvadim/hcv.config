namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

public class VaultSecretSecretConfigurationBuilderShould
{
    private const string VaultHost = "https://vault-server";
    private const string MountPoint = "secrets";
    private const string Prefix = "my-app:";
    private const string SecretPath1 = "path1";
    private const string SecretPath2 = "path2";
    
	[Fact]
    public void InitializeWithCorrectBaseProperties()
    {
        // Arrange
        var mockBuilder = new ConfigurationBuilder();

        // Act
        var builder = new VaultSecretConfigurationBuilder(mockBuilder, VaultHost, MountPoint, Prefix, [SecretPath1,SecretPath2]);

        // Assert
        builder.VaultHost.Should().Be(VaultHost);
        builder.MountPoint.Should().Be(MountPoint);
        builder.Prefix.Should().Be(Prefix);
        builder.SecretPath.Should().BeEquivalentTo([SecretPath1,SecretPath2]);
    }

    [Fact]
    public void InitializeWithDefaultOptionalSettings()
    {
        // Arrange
        var mockBuilder = new ConfigurationBuilder();

        // Act
        var builder = new VaultSecretConfigurationBuilder(
            mockBuilder,
            VaultHost,
            MountPoint,
            Prefix,
            [SecretPath1]
        );

        // Assert
        builder.Optional.Should().NotBeNull();
        builder.Optional.Logger.Should().BeOfType<NullLogger>();
        builder.Optional.KeyDelimiter.Should().Be("--");
    }
}