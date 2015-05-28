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
    private GameClient gameClient;//游戏TCP数据管理器
    private GameScene gameSceneManager;//游戏场景管理器
    private AllocRoomData playerRoomData;//玩家分配到的房间信息
    private List<CardInfo> playerOwnCard;//玩家拥有的卡片
    private GameCard gameCardCollection = new GameCard();//所有卡片集合
    private PlayerInfo playerInfo;//玩家信息集合

    public GameManager(GameScene gameSceneManager)
    {
        this.gameClient = GameClient.Instance;
        this.gameSceneManager = gameSceneManager;
        this.playerInfo = Global.Instance.playerInfo;
        UpdateGameInfo();
    }

    /// <summary>
    /// 更新游戏信息
    /// </summary>
    public void UpdateGameInfo()
    {
        //从全局变量获取信息
        if (Global.Instance.playerRoomData != null)
        {
            this.playerRoomData = Global.Instance.playerRoomData;
        }
        if (Global.Instance.playerOwnCard != null)
        {
            this.playerOwnCard = Global.Instance.playerOwnCard;
        }

        if (this.gameSceneManager != null && this.playerOwnCard != null && this.playerRoomData != null && this.hasGameInit == false)
        {
            //游戏基础数据接受完毕。游戏开始
            LogsSystem.Instance.Print("数据转接正常，游戏开始");
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

            //循环添加选中列表的数据
            foreach (CardInfo cardInfo in playerOwnCard)
            {
                GameObject go = NGUITools.AddChild(parent, perfab);
                go.transform.FindChild("CharacterInfo").gameObject.SetActive(false);//隐藏血条
                go.transform.FindChild("CharacterLevel/Label").GetComponent<UILabel>().text = cardInfo.cardLevel.ToString();//修改等级
                go.AddComponent<UIDragScrollView>().scrollView = scrollView;//这是指向滚动条

                //修改显示数据
                CardContainer container = go.GetComponent<CardContainer>();
                Card card = CardManager.Instance.GetCharacterById(cardInfo.cardId, cardInfo.cardLevel, cardInfo.health, cardInfo.energy, cardInfo.attack, cardInfo.speed);
                card.SetCardUUID(cardInfo.cardUUID);
                container.SetCardData(card);
                container.UpdateCardUI();

                //添加点击事件
                UIEventListener.Get(go).onClick += OnSelectHeroToUp;
            }

            gameSceneManager.chooseCardPanel.alpha = 1;//显示窗口
            LogsSystem.Instance.Print("游戏初始化完毕，开始游戏");
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
        RequestAddCharacterCard(card as CharacterCard, GameSide.Our, card.GetCardInfo().cardUUID);

        MonoBehaviour.DestroyImmediate(go);//立刻销毁游戏物体
        gameSceneManager.chooseCardPanel.alpha = 0;//使窗口隐形
        this.chooseTimes++;//计数器自增
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    public void RoundStart()
    {
        if (chooseTimes < 6)
        {
            gameSceneManager.chooseCardPanel.alpha = 1;//显示选择窗口

            //------还有其他操作
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
    /// 向服务器请求卡片列表
    /// </summary>
    public void RequestCardInv()
    {
        GamePlayerOwnCardData detail = new GamePlayerOwnCardData();
        detail.operatePlayerPosition = playerRoomData.allocPosition;
        detail.operatePlayerUid = playerInfo.uid;
        detail.operatePlayerUUID = playerInfo.UUID;

        GameData data = new GameData();
        data.operateCode = OperateCode.PlayerOwnCard;
        data.roomID = playerRoomData.roomID;
        data.operateData = JsonCoding<GamePlayerOwnCardData>.encode(detail);

        gameClient.SendToServer(data);
    }

    #region 召唤英雄
    /// <summary>
    /// 向服务器请求添加英雄卡到场上
    /// </summary>
    public void RequestAddCharacterCard(CharacterCard character, GameSide side, string cardUUID)
    {
        //发送到远程服务器
        SummonCharacterData detailData = new SummonCharacterData();
        detailData.cardInfo = character.GetCardInfo();
        detailData.cardUUID = cardUUID;
        detailData.operatePlayerPosition = playerRoomData.allocPosition;
        detailData.operatePlayerUid = Global.Instance.playerInfo.uid;
        detailData.operatePlayerUUID = Global.Instance.playerInfo.UUID;

        GameData data = new GameData();
        data.operateCode = OperateCode.SummonCharacter;
        data.roomID = playerRoomData.roomID;
        data.operateData = JsonCoding<SummonCharacterData>.encode(detailData);

        GameClient.Instance.SendToServer(data);
    }
    /// <summary>
    /// 被TCP数据处理器调用
    /// 召唤英雄
    /// </summary>
    public void ResponseAddCharacterCard(SummonCharacterData detailData)
    {
        string cardUUID = detailData.cardUUID;
        CardInfo cardInfo = detailData.cardInfo;
        int operatePlayerPosition = detailData.operatePlayerPosition;
        GameSide operateSide;
        if (operatePlayerPosition == this.playerRoomData.allocPosition)
        { operateSide = GameSide.Our; }
        else
        { operateSide = GameSide.Enemy; }

        //添加数据
        CharacterCard character = CardManager.Instance.GetCharacterById(cardInfo.cardId, cardInfo.cardLevel, cardInfo.health, cardInfo.energy, cardInfo.attack, cardInfo.speed);
        gameCardCollection.AddCharacterCard(character, operateSide);

        //添加场景实体
        gameSceneManager.CreateGameCharacterCard(operateSide, character);
    }
    #endregion

    #region 普通攻击
    /// <summary>
    /// 向服务器发送普通攻击请求
    /// </summary>
    public void RequestCharacterAttack(CharacterCard from, CharacterCard to)
    {
        string fromCardUUID = from.GetCardUUID();
        string toCardUUID = to.GetCardUUID();

        AttackData detailData = new AttackData();
        detailData.fromCardUUID = fromCardUUID;
        detailData.toCardUUID = toCardUUID;
        detailData.operatePlayerPosition = playerRoomData.allocPosition;
        detailData.operatePlayerUid = Global.Instance.playerInfo.uid;
        detailData.operatePlayerUUID = Global.Instance.playerInfo.UUID;

        GameData data = new GameData();
        data.operateCode = OperateCode.Attack;
        data.roomID = playerRoomData.roomID;
        data.operateData = JsonCoding<AttackData>.encode(detailData);

        GameClient.Instance.SendToServer(data);
    }
    /// <summary>
    /// 响应服务器对普通攻击的反应
    /// </summary>
    public void ResponseCharacterAttack(AttackData detailData)
    {
        string fromCardUUID = detailData.fromCardUUID;
        string toCardUUID = detailData.toCardUUID;
        int damage = detailData.damage;
        GameSide operateSide;
        if (detailData.operatePlayerPosition == this.playerRoomData.allocPosition)
        { operateSide = GameSide.Our; }
        else
        { operateSide = GameSide.Enemy; }

        //获取场上卡片对象
        CharacterCard fromCard = gameCardCollection.GetCharacterCard(fromCardUUID, operateSide);
        CharacterCard toCard = gameCardCollection.GetCharacterCard(toCardUUID, GetOppositeSide(operateSide));

        //调用卡片操作
        fromCard.OnCharacterAttack(toCard, damage);
    }
    #endregion

    #region 使用技能
    /// <summary>
    /// 向服务器请求使用技能
    /// </summary>
    public void RequestUseSkill(Skill skill, GameObject from, GameObject to)
    {
        CardContainer fromCardContainer = from.GetComponent<CardContainer>();
        CardContainer toCardContainer = to.GetComponent<CardContainer>();
        if (fromCardContainer != null && toCardContainer != null)
        {
            Card fromCard = fromCardContainer.GetCardData();
            Card toCard = toCardContainer.GetCardData();

            if (fromCard is CharacterCard && toCard is CharacterCard)
            {
                RequestUseSkill(skill, fromCard as CharacterCard, toCard as CharacterCard);//转到正规函数
            }
            else
            {
                LogsSystem.Instance.Print("技能使用对象出错，必须为角色卡", LogLevel.WARN);
            }
        }
        else
        {
            LogsSystem.Instance.Print("出错。指向对象不是卡片", LogLevel.WARN);
        }
    }
    public void RequestUseSkill(Skill skill, CharacterCard from, CharacterCard to)
    {
        UseSkillData detail = new UseSkillData();
        detail.fromCardUUID = from.GetCardUUID();
        detail.toCardUUID = to.GetCardUUID();
        detail.skillCommonName = skill.GetSkillCommonName();

        //detail.skillID


        throw new NotImplementedException();
    }
    #endregion

    #region 装备道具
    #endregion

    #region 抽取手牌
    #endregion

    #region 结束回合
    #endregion

    #region 开始回合
    #endregion

    #region 附属结构
    /// <summary>
    /// 游戏中所有卡片列表
    /// </summary>
    public class GameCard
    {
        public Dictionary<string, CharacterCard> OurCharacterCard = new Dictionary<string, CharacterCard>();//场上卡片<卡片对象，卡片UUID>
        public Dictionary<string, CharacterCard> EnemyCharacterCard = new Dictionary<string, CharacterCard>();//场上卡片<卡片对象，卡片UUID>
        public List<ItemCard> OurHandCard = new List<ItemCard>();
        public List<ItemCard> EnemyHandCard = new List<ItemCard>();

        public void AddCharacterCard(CharacterCard card, GameSide side)
        {
            if (side == GameSide.Our)
            {
                OurCharacterCard.Add(card.GetCardInfo().cardUUID, card);
            }
            else
            {
                EnemyCharacterCard.Add(card.GetCardInfo().cardUUID, card);
            }
        }
        public CharacterCard GetCharacterCard(string cardUUID, GameSide side)
        {
            if (side == GameSide.Our)
            {
                return OurCharacterCard[cardUUID];
            }
            else
            {
                return EnemyCharacterCard[cardUUID];
            }
        }
    }
    /// <summary>
    /// 游戏方
    /// </summary>
    public enum GameSide
    {
        Our, Enemy
    }
    /// <summary>
    /// 获取对立方
    /// </summary>
    public static GameSide GetOppositeSide(GameSide side)
    {
        if (side == GameSide.Our)
        {
            return GameSide.Enemy;
        }
        else
        {
            return GameSide.Our;
        }
    }

    #endregion

}