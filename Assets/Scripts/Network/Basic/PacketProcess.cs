using UnityEngine;
using System.Collections;

public class PacketProcess
{
    public void Process(string text)
    {
        string[] arguments = text.Split(new char[] { ' ' });
        if (arguments[1] == "login")
        {
            if (arguments[2] == "true") { LoginPacketProcess(true); }
            else { LoginPacketProcess(false); }
        }
    }

    private void LoginPacketProcess(bool isAccess)
    {
        if (isAccess)
        {
            ShortMessagesSystem.Instance.ShowShortMessage("登陆成功");
            Application.LoadLevel("MenuScene");//载入主场景
        }
        else
        {
            ShortMessagesSystem.Instance.ShowShortMessage("登陆失败");
        }
    }
}
