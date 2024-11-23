using EulePost.Attachments;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EulePost;

public sealed class EuleMail
{
    private readonly string m_emailAddress;
    private readonly string m_password;
    private readonly string m_host;
    private readonly int m_port;
    private readonly SecureSocketOptions m_sso;
    private readonly bool m_enabled;

    public EuleMail(EuleSettings settings)
    {
        m_emailAddress = settings.EmailAddress;
        m_password = settings.Password;
        m_host = settings.Host;
        m_port = settings.Port;

        m_sso = settings.SSO switch
        {
            SSO.SSL => SecureSocketOptions.SslOnConnect,
            SSO.TLS => SecureSocketOptions.StartTls,
            _ => SecureSocketOptions.None
        };

        m_enabled = settings.Enabled;
    }

    private async Task SendAsync(MimeMessage message)
    {
        using (SmtpClient smtp = new())
        {
            await smtp.ConnectAsync(m_host, m_port, options: m_sso);
            await smtp.AuthenticateAsync(m_emailAddress, m_password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }

    private bool IsValid()
    {
        bool validMail = MailboxAddress.TryParse(m_emailAddress, out _);
        return validMail && m_enabled;
    }

    public async Task SendAsync(MailContactCollection contacts, string subject, string body)
    {
        if (!IsValid())
            return;

        MimeMessage message = new();

        message.SetMessageContacts(contacts);

        message.Subject = subject;

        message.SetBody(body);

        await SendAsync(message);
    }

    public async Task SendAsync(MailContactCollection contacts, string subject, string body, Attachment[] attachments)
    {
        if (!IsValid())
            return;

        MimeMessage message = new();

        message.SetMessageContacts(contacts);

        message.Subject = subject;

        message.SetBody(body, attachments);

        await SendAsync(message);
    }
}
