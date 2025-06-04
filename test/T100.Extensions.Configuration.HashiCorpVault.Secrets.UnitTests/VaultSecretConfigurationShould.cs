namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

public class VaultSecretConfigurationShould
{
	[Fact]
	public void InitializeWithCorrectValues()
	{
		// Arrange
		const string expectedVaultHost = "https://vault.example.com";
		const string expectedMountPoint = "secret";
		const string expectedPrefix = "app:";
		string[] expectedSecretPaths = ["path1", "path2"];

		// Act
		var configuration = new VaultSecretConfiguration(expectedVaultHost, expectedMountPoint, expectedPrefix, expectedSecretPaths);

		// Assert
		configuration.VaultHost.Should().Be(expectedVaultHost);
		configuration.MountPoint.Should().Be(expectedMountPoint);
		configuration.Prefix.Should().Be(expectedPrefix);
		configuration.SecretPath.Should().BeEquivalentTo(expectedSecretPaths);
	}
}