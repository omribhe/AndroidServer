namespace WebApplication1.Models;

public class Message
{
    public int id { get; set; }
    public string content { get; set; }

    public string created { get; set; }

    public bool sent { get; set; }

    public string contactId { get; set; }

    public Message()
    {

    }
    public Message(int id,string content, string created, bool sent,string contactId)
    {
        this.id = id;
        this.content = content;
        this.created = created;
        this.sent = sent;
        this.contactId = contactId;

    }


}



