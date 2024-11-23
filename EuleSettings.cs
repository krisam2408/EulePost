using Microsoft.Extensions.Configuration;

namespace EulePost;

public sealed class EuleSettings
{
    public required string EmailAddress { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public int Port { get; set; }
    public SSO SSO { get; set; }
    public bool Enabled { get; set; } = true;

    public EuleSettings() { }

    public EuleSettings(IConfigurationSection configuration)
    {
        string? email = configuration.GetSection("Address").Value;
        string? password = configuration.GetSection("Password").Value;
        string? host = configuration.GetSection("Host").Value;
        string? port = configuration.GetSection("Port").Value;
        string? sso = configuration.GetSection("SSO").Value;
        string? enabled = configuration.GetSection("Enabled").Value;

        if (string.IsNullOrWhiteSpace(email))
            throw new NullReferenceException(nameof(email));

        if (string.IsNullOrWhiteSpace(password))
            throw new NullReferenceException(nameof(password));

        if (string.IsNullOrWhiteSpace(host))
            throw new NullReferenceException(nameof(host));

        bool isEnabled = true;
        if(!string.IsNullOrWhiteSpace(enabled))
        {
            enabled = enabled.ToLower();
            isEnabled = enabled == "true";
        }

        if (string.IsNullOrWhiteSpace(sso))
            sso = "";

        if (int.TryParse(port, out int portNum))
        {
            EmailAddress = email;
            Password = password;
            Host = host;
            Port = portNum;
            Enabled = isEnabled;
            SSO = sso.ToUpper() switch
            {
                "TLS" => SSO.TLS,
                "SSL" => SSO.SSL,
                _ => SSO.NONE,
            };
            return;
        }

        throw new NullReferenceException(nameof(port));
    }
}
