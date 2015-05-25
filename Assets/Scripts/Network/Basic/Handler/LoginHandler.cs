using UnityEngine;
using System.Collections.Generic;

public class LoginHandler : IHandler
{
    public void Process(SocketModel model)
    {
        int returnCode = model.returnCode;
        LoginDTO data = JsonCoding<LoginDTO>.decode(model.message);

        if (returnCode == ReturnCode.Success)
        {
            LogsSystem.Instance.Print("登陆成功");
            Global.Instance.playerName = data.playerName;
            Global.Instance.UUID = data.UUID;
            EnterGame();
        }
        else if (returnCode == ReturnCode.Pass)
        {
            LogsSystem.Instance.Print("登陆成功，内部版本号不同");
            Global.Instance.playerName = data.playerName;
            Global.Instance.UUID = data.UUID;

            //按钮事件
            List<EventDelegate.Callback> events = new List<EventDelegate.Callback>();
            events.Add(new EventDelegate.Callback(Windows.CloseWindow));
            events.Add(new EventDelegate.Callback(EnterGame));

            Windows.CreateWindows("版本已过时", "游戏版本已过时，是否仍旧进入游戏?（可能会产生未知的错误）", "进入游戏", UIWidget.Pivot.Center, events);
        }
        else if (returnCode == ReturnCode.Repeal)
        {
            LogsSystem.Instance.Print("外部版本号不同");
            Windows.CreateWindows("版本已过时", "游戏版本已过时，请前往官网下载最新游戏版本以进入游戏");
        }
        else if (returnCode == ReturnCode.Refuse)
        {
            LogsSystem.Instance.Print("服务器已满员");
            ShortMessagesSystem.Instance.ShowShortMessage("服务器已满员");
            return;
        }
        else if (returnCode == ReturnCode.Failed)
        {
            LogsSystem.Instance.Print("登陆失败，可能是账户或者密码错误");
            ShortMessagesSystem.Instance.ShowShortMessage("登陆失败，可能是账户或者密码错误");
            return;
        }
        else
        {
            LogsSystem.Instance.Print("未知的返回值");
        }
    }

    public void EnterGame()
    {

        ShortMessagesSystem.Instance.ShowShortMessage("登陆成功，正在跳转到菜单页面");
        LogsSystem.Instance.Print(string.Format("成功登陆服务器，欢迎回来{0}，你分配到的UUID为{1}", Global.Instance.playerName, Global.Instance.UUID));
        Application.LoadLevel("MenuScene");//切换场景
    }
}
