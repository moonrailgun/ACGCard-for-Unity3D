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
    private List<CardInfo> playerOwnGameCard;//玩家拥有的参与游戏的卡片
    private GameCard gameCardCollection = new GameCard();//所有卡片集合
    private PlayerInfo playerInfo;//玩家信息集合

    public bool isGameStarted = false;
    public bool isMyRound = false;

    public GameManager(GameScene gameSceneManager)
    {
        this.gameClient = GameClient.Instance;
        this.gameSceneManager = gameSceneManager;
        this.playerInfo = Global.Instance.playerInfo;
    }

    /// <summary>
    /// 更新游戏信息
    /// </summary>
    public void UpdateGameInfo()
    {
        LogsSystem.Instance.Print("正在更新游戏信息");

        //从全局变量获取信息
        if (Global.Instance.playerRoomData != null)
        {
            this.playerRoomData = Global.Instance.playerRoomData;
            LogsSystem.Instance.Print("游戏管理器已接受到角色房间信息");
            if (Global.Instance.playerGameCard == null)
            {
                LogsSystem.Instance.Print("游戏管理器正在向服务器请求参与游戏卡片数据");
                RequestCardInv();//获取卡片背包列表
            }
        }
        if (Global.Instance.playerGameCard != null)
        {
            this.playerOwnGameCard = Global.Instance.playerGameCard;
            LogsSystem.Instance.Print("游戏管理器已接收到玩家拥有卡片列表");
        }

        if (this.gameSceneManager != null && Global.Instance.playerRoomData != null && Global.Instance.playerGameCard != null && this.hasGameInit == false)
        {
            //游戏基础数据接受完毕。游戏开始
            LogsSystem.Instance.Print("数据转接正常，游戏场景初始化...");
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
            LogsSystem.Instance.Print("正在把英雄添加到场景上,共" + playerOwnGameCard.Count + "个");
            foreach (CardInfo cardInfo in playerOwnGameCard)
            {
                GameObject go = NGUITools.AddChild(parent, perfab);
                go.transform.FindChild("CharacterInfo").gameObject.SetActive(false);//隐藏血条
                go.transform.FindChild("CharacterLevel/Label").GetComponent<UILabel>().text = cardInfo.cardLevel.ToString();//修改等级
                go.AddComponent<UIDragScrollView>().scrollView = scrollView;//这是指向滚动条

                //修改显示数据
                CardContainer container = go.GetComponent<CardContainer>();
                Card card = CardManager.Instance.GetCharacterById(cardInfo.cardUUID, cardInfo.cardId, cardInfo.cardLevel, cardInfo.health, cardInfo.energy, cardInfo.attack, cardInfo.speed, cardInfo.cardOwnSkill);
                card.SetCardUUID(cardInfo.cardUUID);
                container.SetCardData(card);
                container.UpdateCardUI();

                //添加点击事件
                UIEventListener.Get(go).onClick += OnSelectHeroToUp;
            }

            gameSceneManager.ShowChooseWindow();//显示窗口
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
        gameSceneManager.CloseChooseWindow();//使窗口隐形
        this.chooseTimes++;//计数器自增
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    public void RoundStart()
    {
        /*
        if (chooseTimes < 6)
        {
            gameSceneManager.ShowChooseWindow();
        }*/
        LogsSystem.Instance.Print("回合开始", LogLevel.GAMEDETAIL);
        this.isMyRound = true;
        this.gameSceneManager.ShowRoundSwitchBar();//显示回合开始条

        //使按钮可用
        this.gameSceneManager.TurnOnRoundButton();

        //重置所有卡片状态
        List<CharacterCard> list = gameCardCollection.GetAllCharacterCard(GameSide.Our);
        LogsSystem.Instance.Print(string.Format("重置所有卡片状态，共{0}张。", list.Count));
        foreach (CharacterCard card in list)
        {
            card.SetAvailable(true);
        }

        LogsSystem.Instance.Print("重置完毕");
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    public void RoundDone()
    {
        LogsSystem.Instance.Print("回合结束", LogLevel.GAMEDETAIL);

        this.isMyRound = false;
        foreach (CharacterCard card in gameCardCollection.GetAllCharacterCard(GameManager.GameSide.Our))
        {
            //将我方所有卡片都设为不可用
            card.SetAvailable(false);
        }

        //向服务器发送结束回合操作
        GameData data = new GameData();
        data.operateCode = OperateCode.RoundDown;
        data.roomID = playerRoomData.roomID;

        GameClient.Instance.SendToServer(data);
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
        return new List<CardInfo>(playerOwnGameCard);
    }

    public AllocRoomData GetPlayerRoomData()
    {
        return this.playerRoomData;
    }
    public GameCard GetGameCardCollection()
    {
        return this.gameCardCollection;
    }

    /// <summary>
    /// 向服务器请求卡片列表
    /// </summary>
    public void RequestCardInv()
    {
        LogsSystem.Instance.Print("正在向服务器请求卡片背包数据");

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
        detailData.cardInfo = character.GetCardInfo();//获取卡片信息
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
    /// 响应服务器召唤英雄的请求
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
        CharacterCard character = CardManager.Instance.GetCharacterById(cardInfo.cardUUID, cardInfo.cardId, cardInfo.cardLevel, cardInfo.health, cardInfo.energy, cardInfo.attack, cardInfo.speed, cardInfo.cardOwnSkill);
        character.SetCardUUID(cardUUID);
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
        //构建封包
        UseSkillData detailData = new UseSkillData();
        detailData.operatePlayerPosition = playerRoomData.allocPosition;
        detailData.operatePlayerUid = this.playerInfo.uid;
        detailData.operatePlayerUUID = this.playerInfo.UUID;
        detailData.fromCardUUID = from.GetCardUUID();
        detailData.toCardUUID = to.GetCardUUID();
        detailData.skillID = skill.GetSkillID();
        detailData.skillCommonName = skill.GetSkillCommonName();
        detailData.skillAppendData = skill.GetSkillAppendData();//----------暂时没有任何内容

        GameData data = new GameData();
        data.operateCode = OperateCode.UseSkill;
        data.roomID = playerRoomData.roomID;
        data.operateData = JsonCoding<UseSkillData>.encode(detailData);

        GameClient.Instance.SendToServer(data);
    }

    /// <summary>
    /// 响应服务器对使用技能的数据返回
    /// </summary>
    public void ResponseUseSkill(UseSkillData detailData, bool isSuccess = true)
    {
        if (isSuccess)
        {
            int skillID = detailData.skillID;
            string skillAppendData = detailData.skillAppendData;
            string fromCardUUID = detailData.fromCardUUID;
            string toCardUUID = detailData.toCardUUID;
            GameSide operateSide;
            if (detailData.operatePlayerPosition == this.playerRoomData.allocPosition)
            { operateSide = GameSide.Our; }
            else
            { operateSide = GameSide.Enemy; }

            //获取场上卡片对象
            CharacterCard fromCard = gameCardCollection.GetCharacterCard(fromCardUUID, operateSide);
            CharacterCard toCard = gameCardCollection.GetCharacterCard(toCardUUID);

            Skill skill = fromCard.GetSkillByID(skillID);
            if (skill != null)
            {
                skill.OnUse(toCard, skillAppendData);//调用施法卡片下的技能使用具体方法
            }
            else
            { LogsSystem.Instance.Print("错误！试图调用卡片不存在的技能", LogLevel.WARN); }
        }
        else
        {
            LogsSystem.Instance.Print("能量不足,无法释放技能", LogLevel.GAMEDETAIL);
        }

    }
    #endregion

    #region 状态修改
    public void AddState(string cardUUID, int skillID, string appendData)
    {
        CharacterCard characterCard = this.gameCardCollection.GetCharacterCard(cardUUID);
        Skill skill = SkillManager.Instance.GetSkillByID(skillID, appendData);
        if (skill is StateSkill)
        {
            characterCard.AddState(skill as StateSkill, null);
        }
        else
        {
            LogsSystem.Instance.Print("该技能ID不是状态技能" + skillID, LogLevel.WARN);
        }

        LogsSystem.Instance.Print(string.Format("添加状态ID {0} 到 {1}"), LogLevel.GAMEDETAIL);
    }

    public void RemoveState(string cardUUID, int skillID, string appendData)
    {
        CharacterCard characterCard = this.gameCardCollection.GetCharacterCard(cardUUID);
        StateSkill state = characterCard.GetStateById(skillID);
        if (state != null)
        {
            characterCard.RemoveState(state);
            LogsSystem.Instance.Print(string.Format("删除状态ID {0} 到 {1}"), LogLevel.GAMEDETAIL);
        }
        else
        {
            LogsSystem.Instance.Print("卡片不具有该状态", LogLevel.WARN);
        }
    }

    public void UpdateState(string cardUUID, int skillID, string appendData)
    {
        throw new NotImplementedException();//未完成
        //LogsSystem.Instance.Print(string.Format("更新状态ID {0} 到 {1}"), LogLevel.GAMEDETAIL);
    }
    #endregion

    #region 装备道具
    /// <summary>
    /// 请求装上装备
    /// </summary>
    public void RequestEquipment(EquipmentCard equip, int equipPosition)//Weapon = 1, Armor = 2, Jewelry = 3
    {
        LogsSystem.Instance.Print(string.Format("向网络请求装备 {0} ,位置 {1}", equip.GetCardName(), equipPosition));
        OperateEquipData detail = new OperateEquipData();
        detail.operatePlayerUUID = this.playerInfo.UUID;
        detail.operatePlayerUid = this.playerInfo.uid;
        detail.operatePlayerPosition = this.playerRoomData.allocPosition;
        detail.operateCardUUID = equip.GetCardUUID();
        detail.equipCardId = equip.GetCardID();
        detail.equipPosition = equipPosition;
        detail.operateCode = 0;

        GameData data = new GameData();
        data.roomID = this.playerRoomData.roomID;
        data.operateCode = OperateCode.OperateEquip;
        data.operateData = JsonCoding<OperateEquipData>.encode(detail);

        GameClient.Instance.SendToServer(data);
    }

    /// <summary>
    /// 添加装备
    /// </summary>
    public void AddEquipment(string cardUUID, int equipCardID, int position, string appendData)
    {
        LogsSystem.Instance.Print(string.Format("向卡片{0}添加装备卡（ID:{1},位置:{2},附加信息:{3}）", cardUUID, equipCardID, position, appendData));
        CharacterCard characterCard = this.gameCardCollection.GetCharacterCard(cardUUID);
        ItemCard card = CardManager.Instance.GetItemById(equipCardID);
        if (card is EquipmentCard)
        {
            EquipmentCard equipmentCard = card as EquipmentCard;
            characterCard.SetEquipment(equipmentCard, position);//正常输出
        }
        else
        {
            LogsSystem.Instance.Print(string.Format("卡片ID：{0}不合法，不是合法的装备卡，装备操作被跳过", equipCardID), LogLevel.WARN);
        }
    }

    /// <summary>
    /// 移除装备
    /// </summary>
    public void RemoveEquipment(string cardUUID, int equipCardID, int position)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region 抽取手牌
    /// <summary>
    /// 向服务器请求手牌
    /// </summary>
    public void RequestHandCard(int requestCardNum)
    {
        GameHandCardData detail = new GameHandCardData();
        detail.operatePlayerUid = playerInfo.uid;
        detail.operatePlayerUUID = playerInfo.UUID;
        detail.operatePlayerPosition = playerRoomData.allocPosition;
        detail.count = requestCardNum;

        GameData data = new GameData();
        data.operateCode = OperateCode.OperateHandCard;
        data.roomID = playerRoomData.roomID;
        data.operateData = JsonCoding<GameHandCardData>.encode(detail);

        GameClient.Instance.SendToServer(data);
    }
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

        /// <summary>
        /// 获取场上的角色卡
        /// </summary>
        public CharacterCard GetCharacterCard(string cardUUID)
        {
            if (OurCharacterCard.ContainsKey(cardUUID))
            {
                return OurCharacterCard[cardUUID];
            }
            else if (EnemyCharacterCard.ContainsKey(cardUUID))
            {
                return EnemyCharacterCard[cardUUID];
            }
            else
            {
                LogsSystem.Instance.Print("场上不存在这张卡片" + cardUUID);
                return null;
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

        public List<CharacterCard> GetAllCharacterCard(GameSide side)
        {
            List<CharacterCard> list = new List<CharacterCard>();
            Dictionary<string, CharacterCard> dic;
            if (side == GameSide.Our)
            {
                dic = OurCharacterCard;
            }
            else
            {
                dic = EnemyCharacterCard;
            }
            foreach(KeyValuePair<string, CharacterCard> pair in dic){
                list.Add(pair.Value);
            }

            return list;
        }

        /// <summary>
        /// 配置卡片是否可用
        /// </summary>
        public void SetCardAvailable(string cardUUID, bool available)
        {
            GetCharacterCard(cardUUID).SetAvailable(available);
        }
        public bool GetCardAvailable(string cardUUID)
        {
            return GetCharacterCard(cardUUID).GetAvailable();
        }

    }
    /// <summary>
    /// 游戏方
    /// 相对
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