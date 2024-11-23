namespace EulePost.Attachments;

public class Excel2013Attachment : Attachment
{
    public override string ContentType => "application/vnd.ms-excel";

    public Excel2013Attachment(byte[] buffer, string filename) : base(buffer, filename) { }

    public Excel2013Attachment(string base64, string filename) : base(base64, filename) { }

    public Excel2013Attachment(MemoryStream stream, string fileName) : base(stream, fileName) { }
}
