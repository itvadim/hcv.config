namespace T100.Extensions.Configuration.HashiCorpVault.Secrets;

using System;
using AuthProviders;
using Microsoft.Extensions.Configuration;

public static partial class VaultSecretConfigurationExtensions
{
	/// <summary>
	/// Configures the use of token-based authentication for HashiCorp Vault in the provided configuration builder.
	/// </summary>
	/// <param name="builder">The <see cref="IVaultSecretConfigurationBuilder"/> instance used to build the Vault configuration.</param>
	/// <param name="token">The authentication token to access the HashiCorp Vault.</param>
	/// <returns>The updated <see cref="IConfigurationBuilder"/> instance configured with token-based authentication.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the builder is not of the expected VaultConfiguration type.</exception>
	public static IConfigurationBuilder UseTokenAuth(this IVaultSecretConfigurationBuilder builder, string token)
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
				new TokenVaultAuthProvider(token),
				configuration.Optional
			)
		);
	}
}