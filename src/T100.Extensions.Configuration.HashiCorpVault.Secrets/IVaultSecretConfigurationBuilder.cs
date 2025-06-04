namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using System;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Defines a builder interface for configuring HashiCorp Vault secrets in an application.
/// This interface provides mechanisms to add configuration sources to the configuration builder.
/// </summary>
public interface IVaultSecretConfigurationBuilder
{
	/// <summary>
	/// Adds a configuration source to the configuration builder.
	/// </summary>
	/// <param name="source">The configuration source to be added.</param>
	/// <returns>The updated <see cref="IConfigurationBuilder"/> with the added configuration source.</returns>
	IConfigurationBuilder Add(IConfigurationSource source);

	/// <summary>
	/// Configures optional settings for the Vault using the specified configuration action.
	/// </summary>
	/// <param name="configure">An action that applies custom configurations to the <see cref="VaultSecretConfigurationOptional"/> instance.</param>
	/// <returns>The updated <see cref="IVaultSecretConfigurationBuilder"/> instance with applied configurations.</returns>
	IVaultSecretConfigurationBuilder Configure(Action<VaultSecretConfigurationOptional> configure);
}