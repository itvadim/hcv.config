namespace T100.Extensions.Configuration.HashiCorpVault.Secrets.UnitTests;

using AuthProviders;
using Microsoft.Extensions.Logging;
using Moq;
using VaultSharp;
using VaultSharp.V1.Commons;

public class VaultSecretConfigurationProviderShould
{
	private const string VaultHost = "http://vault";
	private const string MountPoint = "mount";
	private const string Prefix = "prefix:";
	private const string SecretPath = "path";
	private const string Token = "token";
	private const string Key1 = "Key1";
	private const string Key2 = "Key2";
	private const string Value1 = "Value1";
	private const string Value2 = "Value2";
	private readonly Mock<IVaultClient> _vaultClientMock = new();
	private readonly Mock<ILogger> _loggerMock = new();

	[Fact]
	public void ThrowArgumentNullExceptionIfVaultClientIsNull()
	{
		// Arrange
		var source = new VaultSecretConfigurationSource
		(
			VaultHost, 
			MountPoint, 
			Prefix,
			[SecretPath],
			Mock.Of<IVaultAuthProvider>(), 
			new VaultSecretConfigurationOptional()
		);

		// Act
		Action act = () => new VaultSecretConfigurationProvider(null, source, new VaultSecretConfigurationOptional());

		// Assert
		act.Should().Throw<ArgumentNullException>().WithMessage("*vaultClient*");
	}
	
	[Fact]
	public void ThrowArgumentNullExceptionIfSourceIsNull()
	{
		// Act
		Action act = () => new VaultSecretConfigurationProvider
		(
			_vaultClientMock.Object,
			null, 
			new VaultSecretConfigurationOptional()
		);

		// Assert
		act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
	}

	[Fact]
	public void ThrowArgumentNullExceptionIfOptionalIsNull()
	{
		// Arrange
		var source = new VaultSecretConfigurationSource
		(
			VaultHost, 
			MountPoint, 
			Prefix,
			[SecretPath],
			Mock.Of<IVaultAuthProvider>(),
			new VaultSecretConfigurationOptional()
		);

		// Act
		Action act = () => new VaultSecretConfigurationProvider(_vaultClientMock.Object, source, null);

		// Assert
		act.Should().Throw<ArgumentNullException>().WithMessage("*optional*");
	}

	[Fact]
	public async Task FetchSecretsFromVaultAsyncReturnCorrectSecrets()
	{
		// Arrange
		var source = new VaultSecretConfigurationSource
		(
			VaultHost, 
			MountPoint, 
			Prefix,
			[SecretPath],
			Mock.Of<IVaultAuthProvider>(),
			new VaultSecretConfigurationOptional()
		);

		var provider = new VaultSecretConfigurationProvider
		(
			_vaultClientMock.Object, source, 
			new VaultSecretConfigurationOptional { Logger = _loggerMock.Object }
		);

		_vaultClientMock.SetupGet(client => client.Settings).Returns(() => new VaultClientSettings(VaultHost, new TokenVaultAuthProvider(Token).CreateAuthMethod()));
		
		var returnedSecrets = new Dictionary<string, object>
		{
			{ Key1, Value1 },
			{ Key2, Value2 }
		};

		_vaultClientMock
			.Setup
			(v => v.V1.Secrets.KeyValue.V2.ReadSecretAsync(It.IsAny<string>(), null, It.IsAny<string>(), null)
			)
			.ReturnsAsync(new Secret<SecretData>
			{
				Data = new SecretData
				{
					Data = returnedSecrets
				}
			});

		// Act
		var result = await provider.FetchSecretsFromVaultAsync();

		// Assert
		result.Should()
			.HaveCount(2).And
			.ContainKeys(Key1, Key2).And
			.ContainValues(Value1, Value1);
	}
	
	[Theory]
	[MemberData(nameof(NormalizeShouldReplaceKeyDelimiterData))]
	public void NormalizeShouldReplaceKeyDelimiter(string key, string delimiter, string expected)
	{
		// Arrange
		var source = new VaultSecretConfigurationSource
		(
			VaultHost, 
			MountPoint, 
			Prefix,
			[SecretPath],
			Mock.Of<IVaultAuthProvider>(), 
			new VaultSecretConfigurationOptional()
		);

		var provider = new VaultSecretConfigurationProvider
		(
			Mock.Of<IVaultClient>(),
			source,
			new VaultSecretConfigurationOptional
			{
				KeyDelimiter = delimiter
			}
		);

		// Act
		var result = provider.Normalize(key);

		// Assert
		result.Should().Be(expected);
	}
	
	[Fact]
	public void AddSecretToDictionaryShouldAddNormalizedKeys()
	{
		// Arrange
		var source = new VaultSecretConfigurationSource
		(
			VaultHost, 
			MountPoint, 
			Prefix,
			[SecretPath],
			Mock.Of<IVaultAuthProvider>(),
			new VaultSecretConfigurationOptional()
		);

		var provider = new VaultSecretConfigurationProvider(Mock.Of<IVaultClient>(), source, new VaultSecretConfigurationOptional());

		var data = new Dictionary<string, string>();

		// Act
		provider.AddSecretToDictionary(data, "prefix:Key", "Value");

		// Assert
		data.Should()
			.HaveCount(1).And
			.ContainKey("Key").WhoseValue.Should().Be("Value");
	}

	[Fact]
	public void AddSecretToDictionaryShouldHandleKeysWithoutPrefix()
	{
		// Arrange
		var source = new VaultSecretConfigurationSource
		(
			VaultHost, 
			MountPoint, 
			Prefix,
			[SecretPath],
			Mock.Of<IVaultAuthProvider>(),
			new VaultSecretConfigurationOptional()
		);

		var provider = new VaultSecretConfigurationProvider(Mock.Of<IVaultClient>(), source, new VaultSecretConfigurationOptional());

		var data = new Dictionary<string, string>();

		// Act
		provider.AddSecretToDictionary(data, "otherPrefix:Key", "Value");

		// Assert
		data.Should()
			.HaveCount(1).And
			.ContainKey("otherPrefix:Key").WhoseValue.Should().Be("Value");
	}
	
	public static IEnumerable<object[]> NormalizeShouldReplaceKeyDelimiterData() => new List<object[]>
	{
		new object[] { "parent--child", "--", "parent:child" },
		new object[] { "parent:child", ":", "parent:child" },
		new object[] { "parent__child", "__", "parent:child" },
	};

}