using UnityEngine;
using System.Collections;

public class Windows
{
    /*
    private float widthPixel = 300;
    private float heightPixel = 300;
    */
    public static GameObject activedWindow;

    /// <summary>
    /// 创建窗体
    /// </summary>
    public static void CreateWindows(string title, string message, string buttonText = "确认", UIWidget.Pivot messagePivot = UIWidget.Pivot.TopLeft, EventDelegate.Callback events = null, WindowsType type = WindowsType.MessageWindow)
    {
        //创建窗体
        GameObject uiroot = GameObject.Find("UI Root");
        if (uiroot != null)
        {
            //实例化
            GameObject prefab = Resources.Load<GameObject>("Window");
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

            //文本赋值
            titleTextLabel.GetComponent<UILabel>().text = title;
            messageLabel.GetComponent<UILabel>().text = message;
            messageLabel.GetComponent<UILabel>().pivot = messagePivot;
            confirmButtonLabel.GetComponent<UILabel>().text = buttonText;

            //点击事件
            if (events == null && type == WindowsType.MessageWindow)
            {
                events += CloseWindow;
            }
            if (events != null)
            {
                confirmButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(events));
            }

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

    private static void OnButtonHover(GameObject go, bool state)
    {
        UITweener tweener = go.GetComponent<UITweener>();
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

    public enum WindowsType
    {
        MessageWindow, ErrorWindow, WaittingWindow
    }
}
