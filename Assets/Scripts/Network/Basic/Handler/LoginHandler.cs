using UnityEngine;

public class LoginHandler : IHandler
{
    public void Process(int returnCode,string data)
    {
        if (returnCode == ReturnCode.Success)
        {
            LogsSystem.Instance.Print("登陆成功");
            ShortMessagesSystem.Instance.ShowShortMessage("登陆成功，正在跳转到菜单页面");
            Application.LoadLevel("MenuScene");
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
