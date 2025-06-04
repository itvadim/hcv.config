namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VaultSharp;

/// <summary>
/// Provides configuration data by retrieving secrets from a HashiCorp Vault instance.
/// </summary>
/// <remarks>
/// This class extends the <see cref="ConfigurationProvider"/> base class to load key-value pairs
/// from a Vault server as configuration settings. It interacts with a Vault through
/// a <see cref="VaultSecretConfigurationSource"/>, which defines the Vault connection and authentication details.
/// </remarks>
/// <example>
/// This provider can be used in an application to seamlessly integrate sensitive data stored
/// in HashiCorp Vault into the application configuration system.
/// </example>
/// <seealso cref="VaultSecretConfigurationSource"/>
public class VaultSecretConfigurationProvider : ConfigurationProvider
{
	private readonly IVaultClient _vaultClient;
	private readonly VaultSecretConfigurationSource _source;
	private readonly string _normalizedPrefix;
	private readonly ILogger _logger;
	private readonly string _vaultKeyDelimiter;

	/// <summary>
	/// Initializes a new instance of the <see cref="VaultSecretConfigurationProvider"/> class.
	/// </summary>
	/// <param name="source">The <see cref="VaultSecretConfigurationSource"/> used to configure the provider.</param>
	/// <param name="optional">The <see cref="VaultSecretConfigurationOptional"/> instance used to configure the provider.</param>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> parameter is null.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="optional"/> parameter is null.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="source"/> parameter is not of the expected type.</exception>
	public VaultSecretConfigurationProvider(VaultSecretConfigurationSource source, VaultSecretConfigurationOptional optional) : this
	(
		new VaultClient(new VaultClientSettings(source.VaultHost, source.AuthProvider.CreateAuthMethod())), 
		source, 
		optional
	)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="VaultSecretConfigurationProvider"/> class.
	/// </summary>
	/// <param name="vaultClient">The <see cref="IVaultClient"/> instance used to interact with the HashiCorp Vault server.</param>
	/// <param name="source">The <see cref="VaultSecretConfigurationSource"/> used to configure the provider.</param>
	/// <param name="optional">The <see cref="VaultSecretConfigurationOptional"/> instance used to configure the provider.</param>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> parameter is null.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="optional"/> parameter is null.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="source"/> parameter is not of the expected type.</exception>
	public VaultSecretConfigurationProvider(IVaultClient vaultClient, VaultSecretConfigurationSource source, VaultSecretConfigurationOptional optional)
	{
		if (optional == null)
		{
			throw new ArgumentNullException(nameof(optional));
		}

		_vaultClient = vaultClient ?? throw new ArgumentNullException(nameof(vaultClient));
		_source = source ?? throw new ArgumentNullException(nameof(source));
		_vaultKeyDelimiter = optional.KeyDelimiter;
		_normalizedPrefix = Normalize(source.Prefix);
		_logger = optional.Logger;
	}

	/// <inheritdoc/>
	public override void Load()
	{
		var secrets = FetchSecretsFromVaultAsync().GetAwaiter().GetResult();
		
		foreach (var kvp in secrets)
		{
			Data[kvp.Key] = kvp.Value;
		}
	}

	internal async Task<Dictionary<string, string>> FetchSecretsFromVaultAsync()
	{
		var result = new Dictionary<string, string>();
		
		foreach (var secretPath in _source.SecretPath)
		{
			IDictionary<string, object> enrichmentData = new Dictionary<string, object>()
			{
				["Vault.Host"] = _source.VaultHost,
				["Vault.AuthMethod"] = _vaultClient.Settings.AuthMethodInfo.AuthMethodType.Type,
				["Vault.MountPoint"] = _source.MountPoint,
				["Vault.SecretPath"] = secretPath
			};
			
			using var scope = _logger.BeginScope(enrichmentData);
			
			try
			{
				var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
				(
					path: secretPath, mountPoint: _source.MountPoint
				);

				var data = secret.Data.Data;

				foreach (var item in data)
				{
					if (item.Value != null)
					{
						AddSecretToDictionary(result, Normalize(item.Key), item.Value.ToString() ?? string.Empty);
					}
				}

				_logger.LogInformation(Strings.VaultSecretsDownloadSuccess);
			}
			catch (Exception ex)
			{
				_logger.LogError(string.Format(Strings.VaultSecretsDownloadError, ex.Message));
			}
		}
		
		return result;
	}

	internal void AddSecretToDictionary(Dictionary<string, string> data, string normalizedKey, string value)
	{
		if (normalizedKey.StartsWith(_normalizedPrefix, StringComparison.OrdinalIgnoreCase))
		{
			data[normalizedKey.Substring(_normalizedPrefix.Length)] = value;
		}
		else
		{
			data[normalizedKey] = value;
		}
	}

	internal string Normalize(string key) => key.Replace(_vaultKeyDelimiter, ConfigurationPath.KeyDelimiter);
}