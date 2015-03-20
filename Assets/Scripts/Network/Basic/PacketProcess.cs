using UnityEngine;
using System.Collections;

public class PacketProcess
{
    public void Process(string text)
    {
        text = text.Trim();//去除多余的空格
        string[] arguments = text.Split(new char[] { ' ' });
        if (arguments[1] == "login")//登陆
        {
            if (arguments[2] == "true") { LoginPacketProcess(true); }
            else { LoginPacketProcess(false); }
        }
        else if (arguments[1] == "chat")//聊天
        {
            if (arguments[2] == "true")
            {
                string chatMessage = arguments[3];
                string fromUUID = arguments[4];
                ChatPacketProcess(chatMessage, fromUUID);
            }
        }
        else if (arguments[1] == "privatechat")//私聊
        {
            Debug.Log("私聊尚未实现");
        }
    }

    private void LoginPacketProcess(bool isAccess, string UUID = "")
    {
        if (isAccess)
        {
            ShortMessagesSystem.Instance.ShowShortMessage("登陆成功");
            if (UUID == "") { LogsSystem.Instance.Print("未获取到服务器分配的UUID", LogLevel.ERROR); }
            else { Global.Instance.UUID = UUID; }

            Application.LoadLevel("MenuScene");//载入主场景-----------------------------------------------------------------------错误，只能从主线程调用
        }
        else
        {
            ShortMessagesSystem.Instance.ShowShortMessage("登陆失败");
        }
    }

    private void ChatPacketProcess(string message, string fromUUID)
    {
        if (Global.Instance.scene == SceneType.MenuScene)//如果在大厅界面
        {
            MenuScene ms = GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<MenuScene>();
            ms.AddChatList(fromUUID, message);
        }
    }
}
