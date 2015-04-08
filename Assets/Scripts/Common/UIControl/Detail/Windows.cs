using UnityEngine;
using System.Collections;

public class Windows
{
    /*
    private float widthPixel = 300;
    private float heightPixel = 300;
    */

    /// <summary>
    /// 创建窗体
    /// </summary>
    public static void CreateWindows(string title, string message, string buttonText = "确认", UIWidget.Pivot messagePivot = UIWidget.Pivot.TopLeft, WindowsType type = WindowsType.MessageWindow)
    {
        GameObject window = Resources.Load<GameObject>("Window");
        GameObject titleTextLabel = window.transform.FindChild("WindowMain/Title/TitleText").gameObject;
        GameObject messageLabel = window.transform.FindChild("WindowMain/Message").gameObject;
        GameObject confirmButtonLabel = window.transform.FindChild("WindowMain/ConfirmButton/Label").gameObject;

        //文本赋值
        titleTextLabel.GetComponent<UILabel>().text = title;
        messageLabel.GetComponent<UILabel>().text = message;
        messageLabel.GetComponent<UILabel>().pivot = messagePivot;
        confirmButtonLabel.GetComponent<UILabel>().text = buttonText;

        //创建窗体
        Transform uiroot = GameObject.Find("UI Root").transform;
        if (uiroot != null)
        {
            GameObject go = Object.Instantiate<GameObject>(window);
            go.transform.parent = uiroot.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            LogsSystem.Instance.Print("生成窗体发生异常", LogLevel.ERROR);
        }
    }

    public enum WindowsType
    {
        MessageWindow, ErrorWindow, WaittingWindow
    }
}
