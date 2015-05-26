using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginScene : MonoBehaviour
{
    [SerializeField]
    private GameObject indexPanel;
    [SerializeField]
    private GameObject serverSelectPanel;
    [SerializeField]
    private GameObject loginPanel;
    [SerializeField]
    private GameObject configPanel;

    private CardClient cardClient;
    private UILabel gameVersion;

    private void Awake()
    {
        Global.Instance.scene = SceneType.LoginScene;//切换场景变量
        Global.Instance.activedSceneManager = this;

        GameObject network = GameObject.FindGameObjectWithTag(Tags.Networks);
        if (network != null)
        { cardClient = network.GetComponent<CardClient>(); }
        else
        { LogsSystem.Instance.Print("请从StartScene开始游戏", LogLevel.ERROR); }

        gameVersion = GameObject.Find("MainPanel/Version").GetComponent<UILabel>();
    }

    private void Start()
    {
        indexPanel.GetComponent<TweenPosition>().to.y = -Screen.height / 2 - 50;

        gameVersion.text = "Ver.  " + Global.Instance.officialVersion;

        if (indexPanel == null || serverSelectPanel == null || loginPanel == null || configPanel == null)
        {
            Debug.LogError("没有给面板赋值");
        }
        else
        {
            indexPanel.SetActive(true);
            serverSelectPanel.SetActive(false);
            loginPanel.SetActive(false);
            configPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        if (indexPanel != null && serverSelectPanel != null)
        {
            serverSelectPanel.SetActive(true);

            //获取服务器列表
            gameObject.GetComponent<ServerLink>().ReadServerList();

            indexPanel.GetComponent<TweenAlpha>().PlayForward();
            indexPanel.GetComponent<TweenPosition>().PlayForward();
            serverSelectPanel.GetComponent<TweenAlpha>().PlayForward();
        }
    }

    /// <summary>
    /// 从服务器列表返回
    /// </summary>
    public void BackFromServerList()
    {
        if (indexPanel != null && serverSelectPanel != null)
        {
            serverSelectPanel.SetActive(false);

            indexPanel.GetComponent<TweenAlpha>().PlayReverse();
            indexPanel.GetComponent<TweenPosition>().PlayReverse();
            serverSelectPanel.GetComponent<TweenAlpha>().PlayReverse();
        }
    }

    /// <summary>
    /// 左下角显示“没有内容”
    /// </summary>
    public void NoContent()
    {
        ShortMessagesSystem.Instance.ShowShortMessage("暂无内容,敬请期待!");
    }

    /// <summary>
    /// 显示游戏设置窗口
    /// </summary>
    public void ShowConfig()
    {
        if (configPanel != null && loginPanel != null)
        {
            indexPanel.SetActive(false);
            configPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 应用设置
    /// </summary>
    public void ConfigConfirm()
    {
        ShortMessagesSystem.Instance.ShowShortMessage("暂未实现");
    }

    /// <summary>
    /// 取消设置
    /// </summary>
    public void ConfigCancel()
    {
        if (configPanel != null && loginPanel != null)
        {
            indexPanel.SetActive(true);
            configPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 显示登陆界面
    /// </summary>
    public void ShowLoginPanel()
    {
        if (serverSelectPanel != null && loginPanel != null)
        {
            serverSelectPanel.SetActive(false);
            loginPanel.SetActive(true);

            loginPanel.GetComponent<TweenAlpha>().PlayForward();
        }
    }

    /// <summary>
    /// 从登陆界面返回
    /// </summary>
    public void BackToServerSelectFromLoginPanel()
    {
        if (serverSelectPanel != null && loginPanel != null)
        {
            serverSelectPanel.SetActive(true);
            loginPanel.SetActive(false);

            loginPanel.GetComponent<TweenAlpha>().PlayReverse();
        }

        //关闭监听线程
        cardClient.StopListen();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 登陆.
    /// </summary>
    public void OnLogin()
    {
        string account = GameObject.Find("LoginPanel/background/Container/Account").GetComponentInChildren<UIInput>().value;
        string password = GameObject.Find("LoginPanel/background/Container/Password").GetComponentInChildren<UIInput>().value;

        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
        {
            ShortMessagesSystem.Instance.ShowShortMessage("请输入账号或者密码");
        }
        else
        {
            //登陆请求
            ShortMessagesSystem.Instance.ShowShortMessage("正在发送登陆请求...请稍后");

            string sendPass = MD5.Encrypt(MD5.Encrypt(password + "ACGCard_By_moonrailgun"));

            SocketModel model = new SocketModel();
            model.protocol = SocketProtocol.LOGIN;
            model.message = JsonCoding<LoginDTO>.encode(new LoginDTO(account, sendPass));//发送登陆请求

            cardClient.SendMsg(JsonCoding<SocketModel>.encode(model));
        }
    }
}
