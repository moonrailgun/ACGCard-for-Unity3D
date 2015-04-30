using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScene : MonoBehaviour
{
    public GameCard cardList = new GameCard();//所有卡片集合
    private GameObject selectedCardObject;//被选中的卡片
    private Skill selectedSkill;//被选中的技能
    private GameCardUIManager uiManager;//卡片UI管理器
    private GameObject cardGrow;//卡片光晕物体
    private ArrowLine arrowLine;

    private void Awake()
    {
        Global.Instance.scene = SceneType.GameScene;

        uiManager = GetComponent<GameCardUIManager>();
        arrowLine = GameObject.Find("ArrowLine").GetComponent<ArrowLine>();

        //配置游戏桌面使其能够点击后取消卡片和技能的选中
        GameObject GamePanel = GameObject.Find("Background/GamePanel");
        UIEventListener.Get(GamePanel).onClick += OnClickGameDesktop;
        //

        //--网络编程
        //--获取对战信息
        //--获取卡片列表
    }

    private void Start()
    {
        //测试数据
        CreateGameCharacterCard(GameSide.Our, CardManager.Instance.GetCardById(1, 100, 100));
        CreateGameCharacterCard(GameSide.Our, CardManager.Instance.GetCardById(6, 100, 100));
        CreateGameCharacterCard(GameSide.Our, CardManager.Instance.GetCardById(8, 100, 100));
        CreateGameCharacterCard(GameSide.Our, CardManager.Instance.GetCardById(12, 100, 100));
        CreateGameCharacterCard(GameSide.Our, CardManager.Instance.GetCardById(24, 100, 100));
        CreateGameCharacterCard(GameSide.Enemy, CardManager.Instance.GetCardById(24, 100, 100));
        CreateGameCharacterCard(GameSide.Enemy, CardManager.Instance.GetCardById(12, 100, 100));
        CreateGameCharacterCard(GameSide.Enemy, CardManager.Instance.GetCardById(1, 100, 100));
        CreateGameCharacterCard(GameSide.Enemy, CardManager.Instance.GetCardById(25, 100, 100));
        CreateGameCharacterCard(GameSide.Enemy, CardManager.Instance.GetCardById(5, 100, 100));
        CreateGameCharacterCard(GameSide.Enemy, CardManager.Instance.GetCardById(17, 100, 100));
        //以上为测试数据
    }

    /// <summary>
    /// 生成游戏卡片
    /// 角色卡
    /// </summary>
    public GameObject CreateGameCharacterCard(GameSide side)
    {
        Card cardinfo = new Card();
        return CreateGameCharacterCard(side, cardinfo);
    }
    public GameObject CreateGameCharacterCard(GameSide side, Card cardinfo)
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            //实例化卡牌
            GameObject prefeb = Resources.Load<GameObject>("CharacterCard");
            GameObject parent = GameObject.Find("GamePanel/" + side.ToString() + "side/CardGrid");
            GameObject card = NGUITools.AddChild(parent, prefeb);

            CardContainer container = card.GetComponent<CardContainer>();
            container.SetCardData(cardinfo);//设置卡片属性
            container.UpdateCardUI();//更新贴图
            uiManager.AddUIListener(card, side);//添加UI事件监听

            return card;
        }
        else
        {
            LogsSystem.Instance.Print("不能在非游戏界面生成游戏卡牌", LogLevel.WARN);
            return null;
        }
    }

    /// <summary>
    /// 点击游戏桌面后调用
    /// 取消卡片选中
    /// </summary>
    private void OnClickGameDesktop(GameObject go)
    {
        //进行一次简单判定
        if (go.name == "GamePanel")
        {
            //重置选中的卡片和技能
            ResetSelectedCard();
            ResetSelectedSkill();
        }
    }

    #region 变量配置与修改
    /// <summary>
    /// 设置当前选中的卡片对象
    /// </summary>
    public void SetSelectedCard(GameObject go)
    {
        //删除光晕
        if (cardGrow != null) { DestroyImmediate(cardGrow); }

        //添加光晕
        if (go.GetComponent<CardContainer>() != null)
        {
            GameObject prefab = Resources.Load<GameObject>("CardGrow-small");
            GameObject glow = NGUITools.AddChild(go, prefab);
            this.cardGrow = glow;
        }

        //播放动画
        UITweener tweener = go.GetComponent<UITweener>();
        if (tweener != null)
        { tweener.PlayForward(); }

        //添加线条
        if (arrowLine != null)
        {
            arrowLine.ShowArrowLine(go);
        }

        this.selectedCardObject = go;
    }
    /// <summary>
    /// 设置选中技能
    /// </summary>
    public void SetSelectedSkill(Skill skill)
    {
        //播放动画
        GameObject go = skill.GetButtonObject();
        UITweener tweener = go.GetComponent<UITweener>();
        if (tweener != null)
        {
            tweener.PlayForward();
        }

        this.selectedSkill = skill;//赋值
    }
    /// <summary>
    /// 获取选中的卡片对象
    /// </summary>
    public GameObject GetSelectedCard()
    {
        return this.selectedCardObject;
    }
    /// <summary>
    /// 重置被选中的卡片
    /// </summary>
    public void ResetSelectedCard()
    {
        if (this.selectedCardObject != null)
        {
            //删除光晕
            if (cardGrow != null) { DestroyImmediate(cardGrow); }

            //播放恢复动画
            UITweener tweener = this.selectedCardObject.GetComponent<UITweener>();
            if (tweener != null)
            { tweener.PlayReverse(); }

            //重置选中卡片
            this.selectedCardObject = null;

            arrowLine.HideArrowLine();//隐藏指向线
        }

    }
    /// <summary>
    /// 获取选中的卡片技能
    /// </summary>
    /// <returns></returns>
    public Skill GetSelectedSkill()
    {
        return this.selectedSkill;
    }
    /// <summary>
    /// 获取选中技能并重置
    /// （即获得一次后重置选中的卡片技能）
    /// </summary>
    public Skill GetSelectedSkillAndReset()
    {
        Skill skill = this.selectedSkill;
        ResetSelectedSkill();
        return skill;
    }
    /// <summary>
    /// 重置选中的技能
    /// </summary>
    public void ResetSelectedSkill()
    {
        if (this.selectedSkill != null)
        {
            //播放恢复动画
            GameObject go = this.selectedSkill.GetButtonObject();
            UITweener tweener = go.GetComponent<UITweener>();
            if (tweener != null)
            {
                tweener.PlayReverse();
            }

            this.selectedSkill = null;
        }
    }
    #endregion

    public enum GameSide
    {
        Our, Enemy
    }

    public class GameCard
    {
        public List<Card> OurCharacterCard = new List<Card>();
        public List<Card> EnemyCharacterCard = new List<Card>();
        public List<Card> OurHandCard = new List<Card>();
        public List<Card> EnemyHandCard = new List<Card>();
    }
}


