namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.AuthProviders;

using System.Security.Cryptography.X509Certificates;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Cert;

/// <summary>
/// Provides an implementation of <see cref="IVaultAuthProvider"/> using a certificate-based
/// authentication method for HashiCorp Vault.
/// </summary>
/// <remarks>
/// This class leverages the provided <see cref="X509Certificate2"/> to authenticate
/// against a Vault instance using the certificate-based auth backend.
/// </remarks>
public class CertificateVaultAuthProvider(X509Certificate2 certificate) : IVaultAuthProvider
{
	/// <summary>
	/// Creates an instance of the <see cref="IAuthMethodInfo"/> used for
	/// authenticating with HashiCorp Vault.
	/// </summary>
	/// <returns>
	/// An instance of <see cref="IAuthMethodInfo"/>, specifically configured
	/// for the certificate-based authentication method.
	/// </returns>
	public IAuthMethodInfo CreateAuthMethod() => new CertAuthMethodInfo(certificate);
}