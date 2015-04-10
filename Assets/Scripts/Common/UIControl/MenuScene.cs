using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MenuScene : MonoBehaviour
{
    //全局
    private CardClient cardClient;

    //功能按键
    //private GameObject StartButton;

    //聊天
    private UITextList chatList;
    private UIScrollBar chatScroll;

    //角色信息
    private PlayerInfo playerInfo;
    private UILabel coinLabel;
    private UILabel gemLabel;
    private UILabel playerNameLabel;
    private UILabel levelLabel;

    //信息面板
    //private GameObject infoPanel;
    private GameObject cardListGrid;
    public bool isWaittingForCardInv = false;

    private void Awake()
    {
        Global.Instance.scene = SceneType.MenuScene;//切换场景变量

        //尝试获取网络端口
        try
        {
            cardClient = GameObject.FindGameObjectWithTag(Tags.Networks).GetComponent<CardClient>();
            RequestPlayerInfo();//获取角色信息
        }
        catch (Exception ex) { LogsSystem.Instance.Print(ex.ToString()); }

        //获取功能按键
        //StartButton = GameObject.Find("GameStart");

        //聊天窗初始化
        chatList = GameObject.Find("Chatting/ChattingList").GetComponent<UITextList>();
        if (chatList != null)
        {
            chatScroll = chatList.scrollBar as UIScrollBar;
            chatScroll.alpha = 0.01f;
        }

        //角色信息获取
        coinLabel = GameObject.Find("Economy/Coin/Sprite/Num").GetComponent<UILabel>();
        gemLabel = GameObject.Find("Economy/Gem/Sprite/Num").GetComponent<UILabel>();
        playerNameLabel = GameObject.Find("Head/Name").GetComponent<UILabel>();
        levelLabel = GameObject.Find("Head/Head-bg/Level").GetComponent<UILabel>();

        //信息控件获取
        //infoPanel = GameObject.Find("Frame/Background/InfoPanel");
        cardListGrid = GameObject.Find("Frame/Background/InfoPanel/CardContainer/CardList/Grid");
    }

    /// <summary>
    /// 本地按下游戏开始按键
    /// </summary>
    public void GameStart()
    {
        Windows.CreateWindows("请稍后", "正在匹配对手...", "取消匹配", UIWidget.Pivot.Center);
        if (cardClient != null || cardClient.hostName != "0.0.0.0")
        {
            string serverHost = cardClient.hostName;//获取IP地址
            GameClient.Instance.ConnectGameServer(serverHost);//连接服务器
        }

        /*
        //发送游戏队列请求
        SocketModel model = new SocketModel();
        model.protocol = SocketProtocol.GAME;

        GameRequestDTO data = new GameRequestDTO();
        data.playerUUID = Global.Instance.playerInfo.UUID;
        data.playerUid = Global.Instance.playerInfo.uid;
        data.playerName = Global.Instance.playerInfo.playerName;
        data.playerLevel = Global.Instance.playerInfo.level;
        model.message = JsonCoding<GameRequestDTO>.encode(data);

        cardClient.SendMsg(JsonCoding<SocketModel>.encode(model));*/
    }

    /// <summary>
    /// 本地输入公共聊天消息提交
    /// </summary>
    public void OnPublicChattingSubmit(UIInput input)
    {
        string text = input.value;
        if (text != null && text != "" && text.Trim() != "")
        {
            LogsSystem.Instance.Print("[用户]" + text);
            AddChatList("我", text);
            if (cardClient != null)
            {
                SocketModel model = new SocketModel();
                model.protocol = SocketProtocol.CHAT;
                ChatDTO data = new ChatDTO(text, Global.Instance.playerName, Global.Instance.UUID);
                model.message = JsonCoding<ChatDTO>.encode(data);

                cardClient.SendMsg(JsonCoding<SocketModel>.encode(model));
            }
        }
        input.value = "";
    }
    /// <summary>
    /// 添加聊天信息到窗口
    /// </summary>
    /// <param name="username">发送者ID</param>
    /// <param name="message">发送的信息</param>
    public void AddChatList(string username, string message)
    {
        if (chatList != null)
        {
            chatList.Add(string.Format("[{0}] {1} : {2}", System.DateTime.Now.ToString("HH:mm:ss"), username, message));
            CheckScrollSize();
        }
    }

    #region 卡片背包
    /// <summary>
    /// 更新玩家拥有的卡片列表
    /// 并显示卡片背包
    /// </summary>
    public void UpdatePlayerCardList()
    {
        //删除过期的卡片
        for (int i = cardListGrid.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cardListGrid.transform.GetChild(i).gameObject);
        }

        //获取列表
        List<CardInfo> cardList = Global.Instance.playerOwnCard;
        //如果没有获取到远程数据，则重新发送邀请并等待返回
        if (cardList == null)
        {
            RequestPlayerCardList();
            isWaittingForCardInv = true;
            ShortMessagesSystem.Instance.ShowShortMessage("正在获取卡片。请稍后");
            return;
        }
        //如果没有任何卡片。则提示
        if (cardList.Count == 0)
        {
            ShortMessagesSystem.Instance.ShowShortMessage("暂时没有拥有任何卡片");
            return;
        }

        //添加数据
        foreach (CardInfo card in cardList)
        {
            AddCardListItem(card.cardId, card.cardName, card.cardRarity);
        }
        LogsSystem.Instance.Print("卡片背包显示完毕");
    }

    private void AddCardListItem(int id, string cardName, int rarity)
    {
        GameObject card = Instantiate<GameObject>(Resources.Load<GameObject>("Card-small"));
        card.transform.parent = cardListGrid.transform;
        card.transform.localScale = new Vector3(1, 1, 1);

        Card cardInfo = card.GetComponent<Card>();
        cardInfo.cardID = id;
        cardInfo.cardName = cardName;
        cardInfo.cardRarity = (CardRarity)rarity;
        cardInfo.UpdateCardUI();

        cardListGrid.GetComponent<UIGrid>().enabled = true;
    }
    #endregion

    /// <summary>
    /// 检查滚动条大小
    /// 当文字内容超过一个屏幕时显示滚动条
    /// </summary>
    private void CheckScrollSize()
    {
        if (chatScroll.barSize != 1)
        {
            chatScroll.alpha = 1.0f;
            Color temp = chatScroll.foregroundWidget.gameObject.GetComponent<UIButton>().defaultColor;
            temp.a = 1.0f;
            chatScroll.foregroundWidget.gameObject.GetComponent<UIButton>().defaultColor = temp;
        }
    }


    #region 信息处理
    /// <summary>
    /// 获取角色信息
    /// </summary>
    private void RequestPlayerInfo()
    {
        SocketModel model = new SocketModel();
        model.protocol = SocketProtocol.PLAYERINFO;
        PlayerInfoDTO data = new PlayerInfoDTO(Global.Instance.UUID);
        model.message = JsonCoding<PlayerInfoDTO>.encode(data);

        cardClient.SendMsg(JsonCoding<SocketModel>.encode(model));
    }

    /// <summary>
    /// 刷新玩家信息
    /// 并获取玩家拥有卡片
    /// </summary>
    public void UpdatePlayerInfo()
    {
        playerInfo = Global.Instance.playerInfo;//此时该对象有值

        if (playerInfo != null)
        {
            coinLabel.text = playerInfo.coin.ToString();
            gemLabel.text = playerInfo.gem.ToString();
            playerNameLabel.text = playerInfo.playerName;
            levelLabel.text = playerInfo.level.ToString();
        }

        RequestPlayerCardList();//获取玩家拥有卡片
    }
    /// <summary>
    /// 获取玩家拥有的卡片列表
    /// </summary>
    private void RequestPlayerCardList()
    {
        SocketModel model = new SocketModel();
        model.protocol = SocketProtocol.CARDINFOLIST;
        CardInfoDTO data = new CardInfoDTO() { cardOwnerId = Global.Instance.playerInfo.uid };
        model.message = JsonCoding<CardInfoDTO>.encode(data);

        cardClient.SendMsg(JsonCoding<SocketModel>.encode(model));
    }
    #endregion

}
