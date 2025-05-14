namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using System;
using Microsoft.Extensions.Configuration;

/// <summary>
/// A builder class for configuring and creating a Vault configuration.
/// It allows setting necessary parameters such as vault host, mount point,
/// secret paths, and other optional settings required for integrating with
/// HashiCorp Vault.
/// </summary>
public class VaultSecretSecretConfigurationBuilder
(
	IConfigurationBuilder builder,
	string host,
	string mountPoint,
	string prefix,
	string[] secretPath
) : VaultSecretConfiguration(host, mountPoint, prefix, secretPath), IVaultSecretConfigurationBuilder
{
	/// <summary>
	/// Gets or sets the optional configuration settings for the HashiCorp Vault integration.
	/// </summary>
	public VaultSecretConfigurationOptional Optional { get; private set; } = new();
	
	/// <inheritdoc />
	IConfigurationBuilder IVaultSecretConfigurationBuilder.Add(IConfigurationSource source) => builder.Add(source);
	
	/// <inheritdoc />
	IVaultSecretConfigurationBuilder IVaultSecretConfigurationBuilder.Configure(Action<VaultSecretConfigurationOptional> configure)
	{
		configure(Optional);
		return this;
	}
}