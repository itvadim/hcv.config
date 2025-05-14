namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

using System.Security.Cryptography.X509Certificates;
using AuthProviders;
using Extensions;
using Microsoft.Extensions.Configuration;
using Moq;

public class VaultSecretCertificateAuthConfigurationExtensionsShould
{
	private const string VaultHost = "https://vault-server";
	private const string MountPoint = "secrets";
	private const string Prefix = "app:";
	private const string SecretPath1 = "path1";

	[Fact]
	public void ThrowInvalidOperationExceptionWhenBuilderTypeIsIncorrect()
	{
		// Arrange
		var invalidBuilderMock = new Mock<IVaultSecretConfigurationBuilder>();
		var certificateMock = new Mock<X509Certificate2>();

		// Act
		Action act = () => invalidBuilderMock.Object.UseCertificateAuth(certificateMock.Object);

		// Assert
		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void AddVaultSecretConfigurationSourceUsingCertificateSuccessfully()
	{
		// Arrange
		var configurationBuilderMock = new Mock<IConfigurationBuilder>();
		configurationBuilderMock
			.Setup(x => x.Add(It.IsAny<VaultSecretConfigurationSource>()))
			.Returns(configurationBuilderMock.Object);

		var vaultSecretBuilder = new VaultSecretConfigurationBuilder
		(
			configurationBuilderMock.Object,
			VaultHost,
			MountPoint,
			Prefix,
			[SecretPath1]
		);

		var certificateMock = new Mock<X509Certificate2>();

		// Act
		var result = vaultSecretBuilder.UseCertificateAuth(certificateMock.Object);

		// Assert
		result.Should().Be(configurationBuilderMock.Object);

		configurationBuilderMock.Verify(
			x => x.Add(It.Is<VaultSecretConfigurationSource>(s =>
				s.VaultHost == VaultHost &&
				s.MountPoint == MountPoint &&
				s.Prefix == Prefix &&
				s.SecretPath.SequenceEqual(new[] { SecretPath1 }) &&
				s.AuthProvider.GetType() == typeof(CertificateVaultAuthProvider))),
			Times.Once
		);
	}
	
	[Fact]
	public void ThrowExceptionWhenCertificateWithThumbprintNotFound()
	{
		// Arrange
		var configurationBuilder = new ConfigurationBuilder();
		const string thumbprint = "invalidThumbprint";
		
		var configurationBuilderMock = new VaultSecretConfigurationBuilder
		(
			configurationBuilder,
			VaultHost,
			MountPoint,
			Prefix,
			[SecretPath1]
		);

		// Act
		Action act = () => configurationBuilderMock.UseCertificateAuth(thumbprint);

		// Assert
		act.Should().Throw<Exception>().WithMessage($"*{thumbprint}*");
	}
	
}