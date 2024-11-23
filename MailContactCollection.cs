namespace EulePost;

public sealed class MailContactCollection
{
    private readonly List<MailContact> m_contacts = [];

    public bool TryAddContact(MailContact contact)
    {
        if (contact.Type == ContactType.From)
        {
            if (CountByType(ContactType.From) == 0)
            {
                m_contacts.Add(contact);
                return true;
            }
            return false;
        }

        if (Contains(contact.Email))
            return false;

        m_contacts.Add(contact);
        return true;
    }

    public bool Contains(string email)
    {
        foreach (MailContact contact in this)
            if (contact.Email == email)
                return true;
        return false;
    }

    private int CountByType(ContactType type)
    {
        int cnt = 0;

        foreach (MailContact contact in this)
            if (contact.Type == type)
                cnt++;

        return cnt;
    }

    public bool IsValid()
    {
        foreach(MailContact contact in this)
            if(!contact.IsValid)
                return false;

        bool from = CountByType(ContactType.From) == 1;
        bool to = CountByType(ContactType.To) > 0;
        bool cc = CountByType(ContactType.CC) > 0;
        bool bcc = CountByType(ContactType.BCC) > 0;

        return from && (to || cc || bcc);
    }

    public IEnumerator<MailContact> GetEnumerator()
    {
        return m_contacts.GetEnumerator();
    }
}
