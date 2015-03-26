using UnityEngine;

public class ChatHandler : IHandler
{
    MenuScene sceneController;

    public void Process(SocketModel model)
    {
        string message = model.message;
        ChatDTO data = JsonCoding<ChatDTO>.decode(message);

        if (Global.Instance.scene == SceneType.MenuScene)
        {
            if (sceneController == null)
            {
                sceneController = GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<MenuScene>();
            }

            if (data.senderName != "Server")
            {
                sceneController.AddChatList(data.senderName, data.content);
            }
            else
            {
                sceneController.AddChatList("系统公告", data.content);
            }
        }
    }
}
