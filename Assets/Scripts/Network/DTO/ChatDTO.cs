using System.Collections;

public class ChatDTO : CommonDTO
{
    public string content;
    public string senderUUID;
    public string toUUID;

    public ChatDTO()
        : base()
    {

    }

    public ChatDTO(string content, string senderUUID, string toUUID = "")
        : base()
    {
        this.content = content;
        this.senderUUID = senderUUID;
    }
}
