using UnityEngine;

public class LoginHandler : IHandler
{
    public void Process(SocketModel model)
    {
        int returnCode = model.returnCode;
        LoginDTO data = JsonCoding<LoginDTO>.decode(model.message);

        if (returnCode == ReturnCode.Success)
        {
            LogsSystem.Instance.Print("登陆成功");
            ShortMessagesSystem.Instance.ShowShortMessage("登陆成功，正在跳转到菜单页面");
            Global.Instance.playerName = data.playerName;
            Global.Instance.UUID = data.UUID;
            LogsSystem.Instance.Print(string.Format("成功登陆服务器，欢迎回来{0}，你分配到的UUID为{1}", Global.Instance.playerName, Global.Instance.UUID));
            Application.LoadLevel("MenuScene");//切换场景
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
}
