using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;

public class MessageHandler : MonoBehaviour
{
    private CardClient cardClient;
    private GameClient gameClient;
    private LoginHandler loginHandler;
    private ChatHandler chatHandler;
    private PlayerInfoHandler playerInfoHandler;
    private CardInfoHandler cardInfoHandler;
    private InvInfoHandler invInfoHandler;

    private TCPGameDataHandler gameDataHandler;//TCP数据处理器

    private List<SocketModel> udpMessageList;
    private Dictionary<GameData, Socket> tcpMessageList;

    private void Awake()
    {
        cardClient = GameObject.FindGameObjectWithTag(Tags.Networks).GetComponent<CardClient>();
        gameClient = GameClient.Instance;
        loginHandler = new LoginHandler();
        chatHandler = new ChatHandler();
        playerInfoHandler = new PlayerInfoHandler();
        cardInfoHandler = new CardInfoHandler();
        invInfoHandler = new InvInfoHandler();

        gameDataHandler = new TCPGameDataHandler();


        if (cardClient != null)
        { udpMessageList = cardClient.GetMessageList(); }
        else
        { LogsSystem.Instance.Print("UDP数据管理器不存在", LogLevel.ERROR); }
        if (gameClient != null)
        { tcpMessageList = gameClient.GetGameDataList(); }
        else
        { LogsSystem.Instance.Print("TCP数据管理器不存在", LogLevel.ERROR); }

    }
    private void Update()
    {
        try
        {
            //每帧检测UDP数据列表,每帧处理一个消息请求（降低瞬间负荷）
            if (udpMessageList.Count > 0)
            {
                SocketModel model = udpMessageList[0];

                ProcessUDPMessage(model);
                udpMessageList.Remove(model);
            }

            //每帧检测TCP数据列表,每帧处理一个消息请求（降低瞬间负荷）
            if (tcpMessageList.Count > 0)
            {
                List<GameData> gameData = new List<GameData>(tcpMessageList.Keys);

                GameData receiveData = gameData[0];//获取第一个
                Socket socket = tcpMessageList[receiveData];

                GameData returnData = gameDataHandler.Process(receiveData, socket);//数据处理
                if (returnData != null)
                {
                    gameClient.Send(socket, returnData);
                }
                tcpMessageList.Remove(receiveData);
            }
        }
        catch (System.Exception ex)
        {
            LogsSystem.Instance.Print(ex.ToString(), LogLevel.WARN);
        }
    }

    /// <summary>
    /// 处理UDP SOCKET消息
    /// </summary>
    private void ProcessUDPMessage(SocketModel model)
    {
        LogsSystem.Instance.Print(string.Format("数据返回：返回码[{0}] 协议[{1}] 消息[{2}]", model.returnCode, model.protocol, model.message));
        switch (model.protocol)
        {
            case SocketProtocol.LOGIN:
                {
                    loginHandler.Process(model);
                    break;
                }
            case SocketProtocol.CHAT:
                {
                    chatHandler.Process(model);
                    break;
                }
            case SocketProtocol.PLAYERINFO:
                {
                    playerInfoHandler.Process(model);
                    break;
                }
            case SocketProtocol.CARDINFOLIST:
                {
                    cardInfoHandler.Process(model);
                    break;
                }
            case SocketProtocol.OFFLINE:
                {
                    List<EventDelegate.Callback> events = new List<EventDelegate.Callback>();
                    events.Add(new EventDelegate.Callback(Windows.CloseWindow));
                    events.Add(new EventDelegate.Callback(LoadLoginScene));
                    Windows.CreateWindows("断线", "您已经断开了连接", "重新登录", UIWidget.Pivot.Top, events, Windows.WindowsType.MessageWindow);
                    break;
                }
            case SocketProtocol.INVINFO:
                {
                    invInfoHandler.Process(model);
                    break;
                }
            default:
                {
                    LogsSystem.Instance.Print("未知的协议");
                    break;
                }
        }
    }

    /// <summary>
    /// 跳转登陆界面
    /// </summary>
    private void LoadLoginScene()
    {
        Application.LoadLevel("LoginScene");
    }
}
