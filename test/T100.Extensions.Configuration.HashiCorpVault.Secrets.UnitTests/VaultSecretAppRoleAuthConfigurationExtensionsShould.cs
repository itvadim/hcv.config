namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

using AuthProviders;
using Microsoft.Extensions.Configuration;
using Moq;

public class VaultSecretAppRoleAuthConfigurationExtensionsShould
{
	private const string VaultHost = "https://vault-server";
	private const string MountPoint = "secrets";
	private const string Prefix = "my-app:";
	private const string SecretPath1 = "path1";

	[Fact]
	public void ThrowInvalidOperationExceptionWhenBuilderTypeIsInvalid()
	{
		// Arrange
		var invalidBuilderMock = new Mock<IVaultSecretConfigurationBuilder>();
		const string roleId = "role-id";
		const string secretId = "secret-id";

		// Act
		Action act = () => invalidBuilderMock.Object.UseAppRoleAuth(roleId, secretId);

		// Assert
		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void AddVaultSecretConfigurationSourceSuccessfully()
	{
		// Arrange
		var configurationBuilderMock = new Mock<IConfigurationBuilder>();
		configurationBuilderMock
			.Setup(x => x.Add(It.IsAny<VaultSecretConfigurationSource>()))
			.Returns(configurationBuilderMock.Object);

		var builder = new VaultSecretConfigurationBuilder
		(
			configurationBuilderMock.Object,
			VaultHost,
			MountPoint,
			Prefix,
			[SecretPath1]
		);
        
		const string roleId = "valid-role-id";
		const string secretId = "valid-secret-id";

		// Act
		var result = builder.UseAppRoleAuth(roleId, secretId);

		// Assert
		result.Should().Be(configurationBuilderMock.Object);

		configurationBuilderMock.Verify
		(
			x => x.Add
			(
				It.Is<VaultSecretConfigurationSource>
				(
					s => 
					s.VaultHost == VaultHost &&
					s.MountPoint == MountPoint &&
					s.Prefix == Prefix &&
					s.SecretPath.SequenceEqual(new[] { SecretPath1 }) &&
					s.AuthProvider.GetType() == typeof(AppRoleVaultAuthProvider))
			),
			Times.Once
		);
	}
}