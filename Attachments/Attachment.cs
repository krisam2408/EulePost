namespace EulePost.Attachments;

public class Attachment
{
    public string Filename { get; private set; }
    public byte[] Buffer { get; private set; }

    private readonly string m_contentType;
    public virtual string ContentType => m_contentType;

    public Attachment(byte[] buffer, string filename, string contentType)
    {
        Buffer = buffer;
        m_contentType = contentType;
        Filename = filename;
    }

    public Attachment(string base64, string filename, string contentType)
    {
        Filename = filename;
        Buffer = Convert.FromBase64String(base64);
        m_contentType = contentType;
    }

    public Attachment(MemoryStream stream, string filename, string contentType)
    {
        Filename = filename;
        Buffer = stream.ToArray();
        m_contentType = contentType;
    }

    protected Attachment(byte[] buffer, string filename)
    {
        Buffer = buffer;
        Filename = filename;
        m_contentType = "";
    }

    protected Attachment(MemoryStream stream, string filename)
    {
        Buffer = stream.ToArray();
        Filename = filename;
        m_contentType = "";
    }

    protected Attachment(string base64, string filename)
    {
        Buffer = Convert.FromBase64String(base64);
        Filename = filename;
        m_contentType = "";
    }
}
