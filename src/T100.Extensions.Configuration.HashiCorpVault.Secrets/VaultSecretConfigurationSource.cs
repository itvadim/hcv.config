namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using AuthProviders;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Represents a configuration source that retrieves secrets from a HashiCorp Vault and integrates them into the configuration system.
/// </summary>
/// <remarks>
/// This class is responsible for defining the configuration source needed to interact with a Vault server.
/// It manages the parameters necessary for connectivity and authentication, such as the Vault host, mount point, secret paths,
/// and the authentication provider.
/// </remarks>
public class VaultSecretConfigurationSource
(
	string vaultHost,
	string mountPoint,
	string prefix,
	string[] secretPath,
	IVaultAuthProvider authProvider,
	VaultSecretConfigurationOptional optional
) : IConfigurationSource
{
	/// <summary>
	/// Gets the URL of the HashiCorp Vault server.
	/// </summary>
	/// <remarks>
	/// This property defines the address of the Vault server to connect to.
	/// It is a required configuration parameter for establishing a connection to
	/// retrieve secrets stored in the Vault.
	/// </remarks>
	public string VaultHost { get; private set; } = vaultHost;

	/// <summary>
	/// Gets the mount point of the secrets engine within the HashiCorp Vault.
	/// </summary>
	/// <remarks>
	/// This property specifies the location within the Vault where the secrets engine is mounted.
	/// The mount point is a key configuration component used to read/write secrets while interacting with the Vault.
	/// It must correspond to the correct secrets engine path configured in the Vault.
	/// </remarks>
	public string MountPoint { get; private set; } = mountPoint;

	/// <summary>
	/// Gets the prefix used to filter the keys retrieved from the HashiCorp Vault.
	/// </summary>
	/// <remarks>
	/// This property defines a key prefix that is applied during the retrieval of secrets
	/// from the Vault. It allows for logical grouping or scoping of secrets, ensuring only
	/// those keys that match the defined prefix are considered.
	/// </remarks>
	public string Prefix { get; private set; } = prefix;

	/// <summary>
	/// Gets the secret paths within the HashiCorp Vault from which configuration data will be retrieved.
	/// </summary>
	/// <remarks>
	/// This property specifies an array of paths in the Vault that contain the secrets to be loaded into the configuration.
	/// Each path is used to query the Vault for secrets, which are then added to the application's configuration.
	/// </remarks>
	public string[] SecretPath { get; private set; } = secretPath;

	/// <summary>
	/// Gets the authentication provider used to connect and authenticate with the HashiCorp Vault server.
	/// </summary>
	/// <remarks>
	/// This property defines the mechanism for authenticating with the Vault, such as token-based,
	/// AppRole, or other supported authentication methods. It is critical for obtaining the necessary
	/// credentials to access secrets in the Vault.
	/// </remarks>
	public IVaultAuthProvider AuthProvider { get; private set; } = authProvider;

	/// <summary>
	/// Builds the <see cref="IConfigurationProvider"/> for retrieving and managing configuration data
	/// from the Vault secret configuration source.
	/// </summary>
	/// <param name="builder">The <see cref="IConfigurationBuilder"/> used to build the provider.</param>
	/// <returns>An <see cref="IConfigurationProvider"/> instance responsible for interacting with the Vault secrets.</returns>
	public IConfigurationProvider Build(IConfigurationBuilder builder) => new VaultSecretConfigurationProvider(this, optional);
}