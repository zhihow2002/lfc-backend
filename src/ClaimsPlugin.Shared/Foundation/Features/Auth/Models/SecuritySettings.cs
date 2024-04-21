namespace ClaimsPlugin.Shared.Foundation.Features.Auth.Models;

public class SecuritySettings
{
    public required bool EnableUserAccessCaching { get; set; }
    public required OtpSettingsObject OtpSettings { get; set; }

    public required OpenIdConnectSettingsObject OpenIdConnectSettings { get; set; }

    public class OtpSettingsObject
    {
        public required int TimeStepInSeconds { get; set; }
        public required bool AllowMultipleUseAfterVerified { get; set; }
    }

    public class OpenIdConnectSettingsObject
    {
        public required int TokenExpirationInMinutes { get; set; }

        public required int RefreshTokenExpirationInDays { get; set; }

        public required CertificateObject Certificate { get; set; }

        public required string Issuer { get; set; }

        public required EndpointObject Endpoint { get; set; }

        public class CertificateObject
        {
            public required DetailObject Encryption { get; set; }
            public required DetailObject Signing { get; set; }

            public class DetailObject
            {
                public required string? Name { get; set; }
                public required string? Key { get; set; }
            }
        }

        public class EndpointObject
        {
            public required string Token { get; set; }
            public required string Introspection { get; set; }
            public required string Cryptography { get; set; }
        }
    }
}
