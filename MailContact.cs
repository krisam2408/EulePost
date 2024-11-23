using MimeKit;

namespace EulePost;

public struct MailContact
{
    public string? Display { get; set; }
    public required string Email { get; set; }
    public ContactType Type { get; set; }

    public MailContact(string email, ContactType type = ContactType.From, string? displayName = null)
    {
        Display = displayName;
        Email = email;
        Type = type;
    }

    public bool IsValid => MailboxAddress.TryParse(Email, out _);

    public MailboxAddress ToAddress()
    {
        if (string.IsNullOrWhiteSpace(Display))
            return MailboxAddress.Parse(Email);
        return new(Display, Email);
    }
}
