using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 窗口创建程序
/// 类型：
/// 消息窗口(MessageWindow):主要是为了提示,按钮可以传递事件，可以直接关闭
/// 等待窗口(WaittingWindow):有等待的标志，按钮有事件，
/// 错误窗口(ErrorWindow):有错误标示与按钮。按钮有后续事件，没有关闭按钮
/// </summary>
public class Windows
{
    /*
    private float widthPixel = 300;
    private float heightPixel = 300;
    */
    public static GameObject activedWindow;//当前激活的窗口

    /// <summary>
    /// 创建窗体
    /// </summary>
    public static void CreateWindows(string title, string message, string buttonText = "确认", UIWidget.Pivot messagePivot = UIWidget.Pivot.TopLeft, List<EventDelegate.Callback> events = null, WindowsType type = WindowsType.MessageWindow)
    {
        //创建窗体
        GameObject uiroot = GameObject.Find("UI Root");
        if (uiroot != null)
        {
            //实例化

            //选择实例化窗口
            string windowPrefab;
            switch(type)
            {
                case WindowsType.MessageWindow:
                    windowPrefab = "Windows/MessageWindow";
                    break;
                case WindowsType.WaittingWindow:
                    windowPrefab = "Windows/WaittingWindow";
                    break;
                case WindowsType.ErrorWindow:
                    windowPrefab = "Windows/ErrorWindow";
                    break;
                default:
                    windowPrefab = "Window";
                    break;
            }

            GameObject prefab = Resources.Load<GameObject>(windowPrefab);
            GameObject window = NGUITools.AddChild(uiroot, prefab);

            if (activedWindow != null)
            {
                CloseWindow();
            }
            activedWindow = window;

            //取值
            GameObject titleTextLabel = window.transform.FindChild("WindowMain/Title/TitleText").gameObject;
            GameObject messageLabel = window.transform.FindChild("WindowMain/Message").gameObject;
            GameObject confirmButton = window.transform.FindChild("WindowMain/ConfirmButton").gameObject;
            GameObject confirmButtonLabel = window.transform.FindChild("WindowMain/ConfirmButton/Label").gameObject;
            //GameObject overlay = window.transform.FindChild("Overlay").gameObject;
            GameObject closeButton = window.transform.FindChild("WindowMain/CloseButton").gameObject;

            //文本赋值
            titleTextLabel.GetComponent<UILabel>().text = title;
            messageLabel.GetComponent<UILabel>().text = message;
            messageLabel.GetComponent<UILabel>().pivot = messagePivot;
            confirmButtonLabel.GetComponent<UILabel>().text = buttonText;

            //点击事件
            if (events == null && type == WindowsType.MessageWindow)
            {
                events = new List<EventDelegate.Callback>();
                events.Add(new EventDelegate.Callback(CloseWindow));
            }
            if (events != null)
            {
                foreach (EventDelegate.Callback _event in events)
                    confirmButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(_event));
            }

            //背景事件
            //UIEventListener.Get(overlay).onClick += CloseWindow;

            //关闭按钮
            UIEventListener.Get(closeButton).onClick += CloseWindow;

            //动画
            TweenScale ts = confirmButton.AddComponent<TweenScale>();
            ts.to = new Vector3(1.1f, 1.1f, 1.1f);
            ts.duration = 0.1f;
            ts.enabled = false;

            UIEventListener.Get(confirmButton).onHover += OnButtonHover;
        }
        else
        {
            LogsSystem.Instance.Print("生成窗体发生异常", LogLevel.ERROR);
        }
    }

    /// <summary>
    /// 按钮事件
    /// </summary>
    private static void OnButtonHover(GameObject go, bool state)
    {
        TweenScale tweener = go.GetComponent<TweenScale>();
        if (state)
        {
            tweener.PlayForward();
        }
        else
        {
            tweener.PlayReverse();
        }
    }

    public static void CloseWindow()
    {
        Object.DestroyImmediate(Windows.activedWindow);
        LogsSystem.Instance.Print("窗口已经被关闭，当前活动的窗口为" + activedWindow);
    }
    public static void CloseWindow(GameObject go)
    {
        CloseWindow();
    }

    public enum WindowsType
    {
        MessageWindow, ErrorWindow, WaittingWindow
    }
}
