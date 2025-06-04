# T100.Extensions.Configuration.HashiCorpVault.Secrets

`T100.Extensions.Configuration.HashiCorpVault.Secrets` is a .NET library provides seamless integration with 
HashiCorp Vault for .NET configuration system, allowing secure retrieval of application secrets.
This package streamlines the retrieval of sensitive information using the `IConfiguration` system and supports various 
Vault authentication methods.

## Key Features
- Integration with the `IConfiguration` system for fetching secrets directly as part of your .NET configuration.
- Supports authentication with **Token**, **AppRole**, and **Certificates**.
- Compatible with **.NET 8.0** and **.NET Standard 2.0**, providing cross-platform availability.
- Flexible configuration for accessing multiple secret paths.
- Extended configuration options with support for logging.

## Getting started
This package simplifies integrating HashiCorp Vault with your .NET application by providing a flexible configuration 
source. You can easily fetch secrets and configuration values from Vault securely and make them part of your application 
configuration.

### Prerequisites
- A running instance of HashiCorp Vault.
- A valid method of authentication for Vault (for example, token, AppRole, or certificate).
- Your project must target:
	- .NET 8.0 or
	- .NET Standard 2.0 for cross-platform compatibility.

## Usage
To use the package, follow these steps:

1. **Install the package via NuGet**:
``` shell
dotnet add package T100.Extensions.Configuration.HashiCorpVault.Secrets
```
1. **Configure Vault integration in your application**:
   Use `AddVaultSecrets` to connect your application's configuration system to Vault.

### Example: Token Authentication
```csharp
using Microsoft.Extensions.Configuration;
using T100.Extensions.Configuration.HashiCorpVault.Secrets;

const string host = "https://vault-server";
const string mountPoint = "kv.secrets";
const string prefix = string.Empty;
const string secretPath = "app/config";
const string token = "hvs...";

var builder = new ConfigurationBuilder() 
    .AddVaultSecrets(host, mountPoint, prefix, secretPath) 
    .UseTokenAuth(token);

var configuration = builder.Build();

// Fetch a secret value
var secretValue = configuration["my-secret-key"];
Console.WriteLine($"Secret value: {secretValue}");
```

### Example: AppRole Authentication
```csharp
using Microsoft.Extensions.Configuration;
using T100.Extensions.Configuration.HashiCorpVault.Secrets;

const string host = "https://vault-server";
const string mountPoint = "kv.secrets";
const string prefix = "brand:";
const string secretPath = "shared/config";
const string roleId = "572a0b1f-...";
const string secretId = "35708c97-...";

var builder = new ConfigurationBuilder() 
    .AddVaultSecrets(host, mountPoint, prefix, secretPath) 
    .UseAppRoleAuth(roleId, secretId);

var configuration = builder.Build();

// Fetch a secret value
var secretValue = configuration["connection"];
Console.WriteLine($"Secret value: {secretValue}");
```

### Example: Certificate Authentication (certificate store)
```csharp
using Microsoft.Extensions.Configuration;
using T100.Extensions.Configuration.HashiCorpVault.Secrets;

const string host = "https://vault-server";
const string mountPoint = "kv.secrets";
const string prefix = string.Empty; // optional
const string secretPath = "app/config";
const string secretPath1 = "shared/config";
const string thumbprint = "e642fa82...";

var builder = new ConfigurationBuilder() 
    .AddVaultSecrets(host, mountPoint, [secretPath, secretPath1])
    .UseCertificateAuth(thumbprint); // By default, the certificate will be searched in the personal store 
                                     // (StoreName.My) of the current user
var configuration = builder.Build();

// Fetch all secrets
foreach (var pair in configuration.AsEnumerable())
{
    Console.WriteLine(pair);
}
```

### Example: Certificate Authentication (certificate file)
```csharp
using Microsoft.Extensions.Configuration;
using T100.Extensions.Configuration.HashiCorpVault.Secrets;

const string host = "https://vault-server";
const string mountPoint = "kv.secrets";
const string prefix = string.Empty;
const string secretPath = "app/config";
const string secretPath1 = "shared/config";
const string certificateFile = "path-to-certificate.pfx";
const string certificatePassword = "<PASSWORD>";

using var certificate = new X509Certificate2(certificateFile, certificatePassword);

var builder = new ConfigurationBuilder() 
    .AddVaultSecrets(host, mountPoint, [secretPath, secretPath1]) 
    .UseCertificateAuth(certificate);

var configuration = builder.Build();

// Fetch all secrets
foreach (var pair in configuration.AsEnumerable())
{
    Console.WriteLine(pair);
}
```

### Example: Complex Solution with troubleshooting
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using T100.Extensions.Configuration.HashiCorpVault.Secrets;

const string host = "https://vault-server";
const string mountPoint = "kv.secrets";
const string prefix = "brand:";
const string secretPath = "app/config";
const string secretPath1 = "shared/config";
const string secretPath2 = "other/config";
const string roleId = "572a0b1f-...";
const string secretId = "35708c97-...";
const string thumbprint = "e642fa82...";

using var factory = LoggerFactory.Create
(
    builder => builder.AddSystemdConsole(optional => optional.IncludeScopes = true) 
);

var logger = factory.CreateLogger<Program>();

var builder = new ConfigurationBuilder() 
    .AddVaultSecrets(host, mountPoint, [secretPath, secretPath1])
    .Configure(optional => optional.Logger = logger)
    .UseCertificateAuth(thumbprint)
    .AddVaultSecrets(host, mountPoint, prefix, secretPath3)
    .Configure(optional =>
    {
        optional.Logger = logger;
        optional.KeyDelimiter = "++";
    })
    .UseAppRoleAuth(roleId, secretId);

var configuration = builder.Build();

// Fetch all secrets
foreach (var pair in configuration.AsEnumerable())
{
    Console.WriteLine(pair);
}
```

## Contributing and Feedback
This library is **open source** under the [MIT license](https://licenses.nuget.org/MIT).
Contributions, feedback, and issues are welcome!

- Report issues on [GitHub](https://github.com/itvadim/hcv.config/issues).
- Fork the repository and submit a pull request to improve the functionality.

---

Happy coding, and efficiently retrieve your secrets with `T100.Extensions.Configuration.HashiCorpVault.Secrets`!