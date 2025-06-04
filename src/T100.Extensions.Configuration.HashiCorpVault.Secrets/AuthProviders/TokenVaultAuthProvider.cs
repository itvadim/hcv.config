namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.AuthProviders;

using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

/// <summary>
/// Provides an authentication provider implementation for HashiCorp Vault using a token.
/// </summary>
/// <remarks>
/// The <see cref="TokenVaultAuthProvider"/> class facilitates authentication to the Vault
/// server by utilizing a static authentication token. The token is provided during
/// instantiation and is used to generate the appropriate authentication method.
/// </remarks>
public class TokenVaultAuthProvider(string token) : IVaultAuthProvider
{
	/// <inheritdoc/>
	public IAuthMethodInfo CreateAuthMethod() => new TokenAuthMethodInfo(token);
}