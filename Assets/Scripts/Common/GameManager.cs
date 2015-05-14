using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一局游戏的管理器
/// 用于管理数据层操作
/// 每次游戏开始都会建立一个新的
/// </summary>
public class GameManager
{
    private GameScene gameSceneManager;//游戏场景管理器
    private AllocRoomData playerRoomData;//玩家分配到的房间信息
    private List<CardInfo> playerOwnCard;//玩家拥有的卡片
    private GameCard gameCardCollection = new GameCard();//所有卡片集合

    public GameManager(GameScene gameSceneManager)
    {
        this.gameSceneManager = gameSceneManager;
        UpdateGameInfo();
    }

    /// <summary>
    /// 更新游戏信息
    /// </summary>
    public void UpdateGameInfo()
    {
        //从全局变量获取信息
        this.playerRoomData = Global.Instance.playerRoomData;
        this.playerOwnCard = Global.Instance.playerOwnCard;

        if (this.gameSceneManager != null && this.playerOwnCard != null && this.playerRoomData != null && this.hasGameInit == false)
        {
            //游戏基础数据接受完毕。游戏开始
            this.GameStart();
        }
    }

    private bool hasGameInit = false;
    /// <summary>
    /// 游戏开始
    /// 创建并生成可以被选中的英雄
    /// 只能初始化一次
    /// </summary>
    public void GameStart()
    {
        if (this.hasGameInit == false)
        {
            GameObject perfab = Resources.Load<GameObject>("CharacterCard");
            GameObject parent = GameObject.Find("ChooseCardPanel/ChooseContainer/ChooseList/Grid");
            UIScrollView scrollView = GameObject.Find("ChooseCardPanel/ChooseContainer/ChooseList").GetComponent<UIScrollView>();

            foreach (CardInfo cardInfo in playerOwnCard)
            {
                GameObject go = NGUITools.AddChild(parent, perfab);
                go.transform.FindChild("CharacterInfo").gameObject.SetActive(false);//隐藏血条
                go.transform.FindChild("CharacterLevel/Label").GetComponent<UILabel>().text = cardInfo.cardLevel.ToString();//修改等级
                go.AddComponent<UIDragScrollView>().scrollView = scrollView;//这是指向滚动条

                //修改显示数据
                CardContainer container = go.GetComponent<CardContainer>();
                Card card = CardManager.Instance.GetCharacterById(cardInfo.cardId, cardInfo.cardLevel, cardInfo.health, cardInfo.energy, cardInfo.attack, cardInfo.speed);
                container.SetCardData(card);
                container.UpdateCardUI();

                //添加点击事件
                UIEventListener.Get(go).onClick += OnSelectHeroToUp;
            }
        }
        this.hasGameInit = true;
    }

    private int chooseTimes = 0;//已经召唤的次数

    /// <summary>
    /// 当选中英雄上场时调用此函数
    /// </summary>
    private void OnSelectHeroToUp(GameObject go)
    {
        Card card = go.GetComponent<CardContainer>().GetCardClone();//获取卡片数据的克隆
        AddCharacterCard(card as CharacterCard, GameSide.Our, card.GetCardInfo().cardUUID);

        MonoBehaviour.DestroyImmediate(go);//立刻销毁游戏物体
        gameSceneManager.chooseCardPanel.alpha = 0;//使窗口隐形
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    public void RoundStart()
    {
        if (chooseTimes < 6)
        {
            gameSceneManager.chooseCardPanel.alpha = 1;//显示窗口

            //------还有其他操作

            chooseTimes++;
        }
        LogsSystem.Instance.Print("回合开始");
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    public void RoundDown()
    {
        LogsSystem.Instance.Print("回合结束");
    }

    /// <summary>
    /// 游戏结束，清空变量
    /// </summary>
    public void GameOver()
    {
        Global.Instance.playerRoomData = null;
        Global.Instance.playerOwnCard = null;

        this.UpdateGameInfo();
    }

    /// <summary>
    /// 获取玩家拥有卡片的备份
    /// </summary>
    public List<CardInfo> GetPlayerOwnCardClone()
    {
        return new List<CardInfo>(playerOwnCard);
    }

    /// <summary>
    /// 添加英雄卡到场上
    /// </summary>
    public void AddCharacterCard(CharacterCard character, GameSide side,string cardUUID)
    {
        //添加到场景卡片集合
        if (side == GameSide.Our)
            this.gameCardCollection.OurCharacterCard.Add(character);
        else if (side == GameSide.Enemy)
            this.gameCardCollection.EnemyCharacterCard.Add(character);

        gameSceneManager.SummonCharacterUp(character,side);//让场景管理器能够调用召唤这张卡

        //发送到远程服务器
        GameData data = new GameData();
        data.operateCode = OperateCode.SummonCharacter;
        data.roomID = playerRoomData.roomID;
        data.operateData = cardUUID;

        GameClient.Instance.SendToServer(data);
    }

    #region 附属结构
    /// <summary>
    /// 游戏中所有卡片列表
    /// </summary>
    public class GameCard
    {
        public List<CharacterCard> OurCharacterCard = new List<CharacterCard>();
        public List<CharacterCard> EnemyCharacterCard = new List<CharacterCard>();
        public List<ItemCard> OurHandCard = new List<ItemCard>();
        public List<ItemCard> EnemyHandCard = new List<ItemCard>();
    }
    /// <summary>
    /// 游戏方
    /// </summary>
    public enum GameSide
    {
        Our, Enemy
    }

    #endregion

}