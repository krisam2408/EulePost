using EulePost.Attachments;
using MimeKit;

namespace EulePost;

public static class Extensions
{
    public static void SetMessageContacts(this MimeMessage msg, MailContactCollection contacts)
    {
        if (!contacts.IsValid())
            throw new FormatException("Contacts collection is not valid");

        foreach (MailContact contact in contacts)
        {
            if (contact.Type == ContactType.From)
            {
                msg.From.Add(contact.ToAddress());
                continue;
            }

            if (contact.Type == ContactType.CC)
            {
                msg.Cc.Add(contact.ToAddress());
                continue;
            }

            if (contact.Type == ContactType.BCC)
            {
                msg.Bcc.Add(contact.ToAddress());
                continue;
            }

            msg.To.Add(contact.ToAddress());
        }
    }

    public static void SetBody(this MimeMessage msg, string text)
    {
        msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = text
        };
    }

    public static void SetBody(this MimeMessage msg, string text, Attachment[] attachments)
    {
        BodyBuilder builder = new();
        builder.HtmlBody = text;

        foreach (Attachment attachment in attachments)
        {
            using (MemoryStream ms = new(attachment.Buffer))
            {
                MimePart part = new(attachment.ContentType)
                {
                    Content = new MimeContent(ms),
                    ContentDisposition = new(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.Filename
                };
                builder.Attachments.Add(part);
            }
        }

        msg.Body = builder.ToMessageBody();
    }
}
