namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

using Extensions;
using Microsoft.Extensions.Configuration;
using Moq;

public class VaultSecretTokenAuthConfigurationExtensionsShould
{
    private const string VaultHost = "https://vault-server";
    private const string MountPoint = "secrets";
    private const string Prefix = "my-app:";
    private const string SecretPath1 = "path1";
    
	[Fact]
    public void ConfigureTokenAuthSuccessfully()
    {
        // Arrange
        var configurationBuilderMock = new Mock<IConfigurationBuilder>();
        
        configurationBuilderMock.Setup(x => x.Add(It.IsAny<VaultSecretConfigurationSource>())).Returns(configurationBuilderMock.Object);
        
        var builder = new VaultSecretConfigurationBuilder
        (
            configurationBuilderMock.Object,
            VaultHost,
            MountPoint,
            Prefix,
            [SecretPath1]
        );
        
        const string token = "TestToken";

        // Act
        var result = builder.UseTokenAuth(token);

        // Assert
        result.Should().Be(configurationBuilderMock.Object);
        configurationBuilderMock.Verify(x => x.Add(It.IsNotNull<VaultSecretConfigurationSource>()), Times.Once);
    }

    [Fact]
    public void ThrowInvalidOperationExceptionIfBuilderTypeIsIncorrect()
    {
        // Arrange
        var invalidBuilderMock = new Mock<IVaultSecretConfigurationBuilder>();
        const string token = "TestToken";

        // Act
        Action act = () => invalidBuilderMock.Object.UseTokenAuth(token);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddVaultSecretConfigurationSourceWithCorrectValues()
    {
        // Arrange
        var configurationBuilderMock = new Mock<IConfigurationBuilder>();
        var builder = new VaultSecretConfigurationBuilder
        (
            configurationBuilderMock.Object,
            VaultHost,
            MountPoint,
            Prefix,
            [SecretPath1]
        );
        
        const string token = "valid-token";

        VaultSecretConfigurationSource? capturedSource = null;

        configurationBuilderMock
            .Setup(x => x.Add(It.IsAny<VaultSecretConfigurationSource>()))
            .Callback<IConfigurationSource>(source =>
            {
                capturedSource = source as VaultSecretConfigurationSource;
            });

        // Act
        builder.UseTokenAuth(token);

        // Assert
        capturedSource.Should().NotBeNull();
        capturedSource.VaultHost.Should().Be(VaultHost);
        capturedSource.MountPoint.Should().Be(MountPoint);
        capturedSource.Prefix.Should().Be(Prefix);
        capturedSource.SecretPath.Should().BeEquivalentTo([SecretPath1]);
    }

}