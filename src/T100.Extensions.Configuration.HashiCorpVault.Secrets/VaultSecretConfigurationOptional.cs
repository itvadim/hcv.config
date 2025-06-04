namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

/// <summary>
/// Represents optional configuration settings for HashiCorp Vault integration.
/// </summary>
public class VaultSecretConfigurationOptional
{
	/// <summary>
	/// Gets or sets the <see cref="ILogger"/> instance used for logging operations within the HashiCorp Vault configuration.
	/// </summary>
	/// <remarks>
	/// By default, this property is initialized with <see cref="NullLogger.Instance"/> to provide a no-op logging implementation.
	/// Users can override this property with a specific logger instance to enable logging for debugging or monitoring purposes.
	/// </remarks>
	public ILogger Logger { get; set; } = NullLogger.Instance;

	/// <summary>
	/// Gets or sets the string used to delimit hierarchical keys in the HashiCorp Vault configuration settings.
	/// </summary>
	/// <remarks>
	/// The default value of this property is "--".
	/// Users can customize this delimiter to align with their preferred key structure for easier navigation and management of configuration keys.
	/// </remarks>
	public string KeyDelimiter { get; set; } = "--";
}