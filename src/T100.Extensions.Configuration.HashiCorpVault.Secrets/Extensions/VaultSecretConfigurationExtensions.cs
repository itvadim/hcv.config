namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.Extensions;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Provides extension methods for integrating HashiCorp Vault secrets with the Microsoft.Extensions.Configuration system.
/// </summary>
public static class VaultSecretConfigurationExtensions
{
	/// <summary>
	/// Adds HashiCorp Vault secrets to the configuration builder using the specified vault host, mount point, and secret paths.
	/// </summary>
	/// <param name="builder">The configuration builder to which the Vault secrets will be added.</param>
	/// <param name="vaultHost">The host address of the HashiCorp Vault server.</param>
	/// <param name="mountPoint">The mount point on the Vault server where the secrets are stored.</param>
	/// <param name="secretPath">A list of secret paths to retrieve from the Vault server.</param>
	/// <returns>An <see cref="IVaultSecretConfigurationBuilder"/> instance for adding additional configuration or authentication options.</returns>
	public static IVaultSecretConfigurationBuilder AddVaultSecrets
	(
		this IConfigurationBuilder builder,
		string vaultHost,
		string mountPoint,
		params string[] secretPath
	) => AddVaultSecrets(builder, vaultHost, mountPoint, string.Empty, secretPath);

	/// <summary>
	/// Adds HashiCorp Vault secrets to the configuration builder using the specified parameters.
	/// </summary>
	/// <param name="builder">The configuration builder to which the Vault secrets will be added.</param>
	/// <param name="vaultHost">The host address of the HashiCorp Vault server.</param>
	/// <param name="mountPoint">The mount point on the Vault server where the secrets are stored.</param>
	/// <param name="prefix">An optional prefix to be applied to all keys retrieved from the Vault.</param>
	/// <param name="secretPath">A list of secret paths to retrieve from the Vault server.</param>
	/// <returns>An <see cref="IVaultSecretConfigurationBuilder"/> instance for adding additional configuration or authentication options.</returns>
	public static IVaultSecretConfigurationBuilder AddVaultSecrets
	(
		this IConfigurationBuilder builder,
		string vaultHost,
		string mountPoint,
		string prefix,
		params string[] secretPath
	) => new VaultSecretSecretConfigurationBuilder(builder, vaultHost, mountPoint, prefix, secretPath);
}