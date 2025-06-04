namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.AuthProviders;

using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;

/// <summary>
/// Provides an implementation of the <see cref="IVaultAuthProvider"/> interface
/// using the AppRole authentication method for HashiCorp Vault.
/// </summary>
/// <remarks>
/// The <see cref="AppRoleVaultAuthProvider"/> class allows authentication to Vault
/// by providing a Role ID and Secret ID. This information is used to create an
/// instance of <see cref="AppRoleAuthMethodInfo"/> for the Vault API.
/// </remarks>
public class AppRoleVaultAuthProvider(string roleId, string secretId) : IVaultAuthProvider
{
	/// <summary>
	/// Creates and returns an instance of the authentication method information
	/// for HashiCorp Vault.
	/// </summary>
	/// <returns>
	/// An instance of <see cref="IAuthMethodInfo"/> representing the authentication
	/// information configured for the specific Vault authentication provider.
	/// </returns>
	public IAuthMethodInfo CreateAuthMethod() => new AppRoleAuthMethodInfo(roleId, secretId);
}