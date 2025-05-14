namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

/// <summary>
/// Represents the configuration settings for connecting to a HashiCorp Vault instance
/// and retrieving secrets using specified parameters.
/// </summary>
public class VaultSecretConfiguration(string vaultHost, string mountPoint, string prefix, string[] secretPath)
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
}