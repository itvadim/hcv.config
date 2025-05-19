namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using System;
using AuthProviders;
using Microsoft.Extensions.Configuration;

public static partial class VaultSecretConfigurationExtensions
{
	/// <summary>
	/// Configures the application to use AppRole authentication with HashiCorp Vault.
	/// </summary>
	/// <param name="builder">
	/// The configuration builder that provides the context for HashiCorp Vault integration.
	/// </param>
	/// <param name="roleId">
	/// The Role ID used for AppRole authentication.
	/// </param>
	/// <param name="secretId">
	/// The Secret ID used for AppRole authentication.
	/// </param>
	/// <returns>
	/// The <see cref="IConfigurationBuilder"/> with AppRole authentication configured.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the provided builder is not a valid <see cref="VaultSecretConfigurationBuilder"/>.
	/// </exception>
	public static IConfigurationBuilder UseAppRoleAuth
	(
		this IVaultSecretConfigurationBuilder builder,
		string roleId,
		string secretId
	)
	{
		if (builder is not VaultSecretConfigurationBuilder configuration)
		{
			throw new InvalidOperationException();
		}

		return builder.Add
		(
			new VaultSecretConfigurationSource
			(
				configuration.VaultHost, 
				configuration.MountPoint, 
				configuration.Prefix, 
				configuration.SecretPath, 
				new AppRoleVaultAuthProvider(roleId, secretId), 
				configuration.Optional
			)
		);
	}
}