namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.AuthProviders;

using VaultSharp.V1.AuthMethods;

/// <summary>
/// Defines the contract for implementing Vault authentication providers.
/// </summary>
/// <remarks>
/// Implementing this interface allows the creation of custom authentication methods
/// to be used with HashiCorp Vault. The implementer is responsible for providing
/// the authentication details through the <see cref="CreateAuthMethod"/> method.
/// </remarks>
public interface IVaultAuthProvider
{
	/// <summary>
	/// Creates and returns an instance of the Vault authentication method information.
	/// </summary>
	/// <returns>
	/// An instance of <see cref="IAuthMethodInfo"/> representing the authentication method
	/// to be used with HashiCorp Vault.
	/// </returns>
	IAuthMethodInfo CreateAuthMethod();
}