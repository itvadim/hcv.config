namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.Extensions;

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AuthProviders;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Provides extension methods for configuring Vault secret authentication using certificates.
/// </summary>
public static class VaultSecretCertificateAuthConfigurationExtensions
{
	/// <summary>
	/// Configures the Vault secret configuration to use certificate authentication.
	/// </summary>
	/// <param name="builder">The Vault secret configuration builder.</param>
	/// <param name="certificate">The certificate used for authentication.</param>
	/// <returns>The updated configuration builder.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the provided builder is not of type VaultSecretConfigurationBuilder.</exception>
	public static IConfigurationBuilder UseCertificateAuth
	(
		this IVaultSecretConfigurationBuilder builder, 
		X509Certificate2 certificate
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
				new CertificateVaultAuthProvider(certificate), 
				configuration.Optional
			)
		);
	}

	/// <summary>
	/// Configures the Vault secret configuration to use certificate authentication.
	/// </summary>
	/// <param name="builder">The Vault secret configuration builder.</param>
	/// <param name="thumbprint">The thumbprint of the certificate to use for authentication.</param>
	/// <param name="storeName">The name of the certificate store. Defaults to StoreName.My.</param>
	/// <param name="storeLocation">The location of the certificate store. Defaults to StoreLocation.CurrentUser.</param>
	/// <returns>The updated configuration builder.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the provided builder is not of type VaultSecretConfigurationBuilder.</exception>
	/// <exception cref="Exception">Thrown if the certificate with the specified thumbprint cannot be found in the specified certificate store.</exception>
	public static IConfigurationBuilder UseCertificateAuth
	(
		this IVaultSecretConfigurationBuilder builder,
		string thumbprint,
		StoreName storeName = StoreName.My,
		StoreLocation storeLocation = StoreLocation.CurrentUser
	)
	{
		if (builder is not VaultSecretConfigurationBuilder configuration)
		{
			throw new InvalidOperationException();
		}

		var certificate = FindCertificate(thumbprint, storeName, storeLocation);

		if (certificate == null)
		{
			throw new Exception(string.Format(Strings.CertificateNotFound, thumbprint, storeName, storeLocation));
		}

		return builder.Add
		(
			new VaultSecretConfigurationSource
			(
				configuration.VaultHost,
				configuration.MountPoint,
				configuration.Prefix,
				configuration.SecretPath,
				new CertificateVaultAuthProvider(certificate),
				configuration.Optional
			)
		);

		X509Certificate2? FindCertificate(string certThumbprint, StoreName certStoreName, StoreLocation certStoreLocation)
		{
			using var store = new X509Store(certStoreName, certStoreLocation);
			store.Open(OpenFlags.ReadOnly);

			return store.Certificates
				.Find(X509FindType.FindByThumbprint, certThumbprint, validOnly: false)
				.OfType<X509Certificate2>()
				.FirstOrDefault();
		}
	}
}