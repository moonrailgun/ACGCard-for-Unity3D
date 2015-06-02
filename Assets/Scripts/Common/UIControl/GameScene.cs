using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScene : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject selectedCardObject;//被选中的卡片
    private Skill selectedSkill;//被选中的技能
    private GameCardUIManager uiManager;//卡片UI管理器
    private GameObject cardGrow;//卡片光晕物体
    private ArrowLine arrowLine;//箭头指向线
    private GameObject roundDoneButton;//回合结束按钮
    public UIPanel chooseCardPanel;//选择卡片根面板
    public UIGrid chooseCardGrid;//选择卡片的网格

    private void Awake()
    {
        Global.Instance.scene = SceneType.GameScene;
        Global.Instance.activedSceneManager = this;

        this.uiManager = GetComponent<GameCardUIManager>();
        this.roundDoneButton = GameObject.Find("GamePanel/RoundDone");
        this.chooseCardPanel = GameObject.Find("UI Root/Background/ChooseCardPanel").GetComponent<UIPanel>();
        this.chooseCardGrid = GameObject.Find("UI Root/Background/ChooseCardPanel/ChooseContainer/ChooseList/Grid").GetComponent<UIGrid>();

        Init();
    }
    private void Init()
    {
        //初始化一局游戏的管理器
        this.gameManager = new GameManager(this);

        //配置游戏桌面使其能够点击后取消卡片和技能的选中
        GameObject GamePanel = GameObject.Find("Background/GamePanel");
        UIEventListener.Get(GamePanel).onClick += OnClickGameDesktop;

        this.gameManager.UpdateGameInfo();//更新一下数据
    }

    private void Start()
    {
        /*
        //测试数据
        CreateGameCharacterCard(GameManager.GameSide.Our, CardManager.Instance.GetCharacterById(1, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Our, CardManager.Instance.GetCharacterById(6, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Our, CardManager.Instance.GetCharacterById(8, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Our, CardManager.Instance.GetCharacterById(12, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Our, CardManager.Instance.GetCharacterById(24, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Enemy, CardManager.Instance.GetCharacterById(24, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Enemy, CardManager.Instance.GetCharacterById(12, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Enemy, CardManager.Instance.GetCharacterById(1, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Enemy, CardManager.Instance.GetCharacterById(25, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Enemy, CardManager.Instance.GetCharacterById(5, 1, 100, 100, 10, 200));
        CreateGameCharacterCard(GameManager.GameSide.Enemy, CardManager.Instance.GetCharacterById(17, 1, 100, 100, 10, 200));


        CreateGameHandCard(CardManager.Instance.GetItemById(1));
        CreateGameHandCard(CardManager.Instance.GetItemById(2));
        CreateGameHandCard(CardManager.Instance.GetItemById(3));
        CreateGameHandCard(CardManager.Instance.GetItemById(4));
        LogsSystem.Instance.Print("已生成测试数据");
        //以上为测试数据
        */
    }

    #region 场景卡片管理
 /// <summary>
    /// 生成游戏卡片
    /// 角色卡
    /// </summary>
    public GameObject CreateGameCharacterCard(GameManager.GameSide side)
    {
        Card cardinfo = new Card();
        return CreateGameCharacterCard(side, cardinfo);
    }
    public GameObject CreateGameCharacterCard(GameManager.GameSide side, Card cardData)
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            //实例化卡牌
            GameObject prefeb = Resources.Load<GameObject>("CharacterCard");
            GameObject parent = GameObject.Find("GamePanel/" + side.ToString() + "side/CardGrid");
            GameObject card = NGUITools.AddChild(parent, prefeb);

            CardContainer container = card.GetComponent<CardContainer>();
            container.SetCardData(cardData);//设置卡片属性
            container.UpdateCardUI();//更新贴图
            uiManager.AddCharacterUIListener(card, side);//添加UI事件监听
            container.SetGameSide(side);//设置卡片归属方

            parent.GetComponent<UIGrid>().Reposition();//更新卡片位置

            return card;
        }
        else
        {
            LogsSystem.Instance.Print("不能在非游戏界面生成游戏卡牌", LogLevel.WARN);
            return null;
        }
    }

    /// <summary>
    /// 生成手牌
    /// </summary>
    public GameObject CreateGameHandCard(Card cardinfo)
    {
        //实例化卡牌
        GameObject prefeb = Resources.Load<GameObject>("Card-small");
        GameObject parent = GameObject.Find("GamePanel/Ourside/HandCard");
        GameObject card = NGUITools.AddChild(parent, prefeb);

        CardContainer container = card.GetComponent<CardContainer>();
        container.SetCardData(cardinfo);//设置卡片属性
        container.UpdateCardUI();//更新贴图
        uiManager.AddHandUIListener(card);//添加手牌UI事件监听
        container.SetGameSide(GameManager.GameSide.Our);

        //parent.GetComponent<UIGrid>().Reposition();//更新卡片位置

        return card;
    }

    /// <summary>
    /// 销毁卡片
    /// </summary>
    public void DestoryCard(GameObject card)
    {
        UIGrid grid = card.transform.parent.GetComponent<UIGrid>();
        DestroyImmediate(card);//立刻销毁卡片

        if (grid != null)
            grid.Reposition();
    }
    public void DestoryCard(Card card)
    {
        DestoryCard(card.GetCardContainer().gameObject);
    }

    /// <summary>
    /// 召唤选中的角色卡
    /// </summary>
    public void SummonCharacterUp(CharacterCard card, GameManager.GameSide side)
    {
        LogsSystem.Instance.Print(string.Format("{0}召唤{1}上场", side.ToString(), card.GetCardName()));

        //召唤卡片到场上
        string path = string.Format("UI Root/Background/GamePanel/{0}side/CardGrid", side.ToString());
        GameObject parent = GameObject.Find(path);
        GameObject prefab = Resources.Load<GameObject>("CharacterCard");
        GameObject go = NGUITools.AddChild(parent, prefab);
        CardContainer container = go.GetComponent<CardContainer>();
        container.SetCardData(card, true);
        parent.GetComponent<UIGrid>().Reposition();
    }
    #endregion


    #region 场景事件
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
        }
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    public void RoundDown()
    {
        UIButton button = roundDoneButton.GetComponent<UIButton>();
        button.isEnabled = false;
        roundDoneButton.transform.Find("Label").GetComponent<UILabel>().color = button.disabledColor;
        LogsSystem.Instance.Print("回合结束");

        LogsSystem.Instance.Print("回合结束按钮测试........");
        Invoke("TurnOnRoundButton", 2f);//用于测试
    }

    /// <summary>
    /// 进入自己的回合
    /// 使回合结束按钮可用
    /// </summary>
    public void TurnOnRoundButton()
    {
        UIButton button = roundDoneButton.GetComponent<UIButton>();
        button.isEnabled = true;
        roundDoneButton.transform.Find("Label").GetComponent<UILabel>().color = button.defaultColor;
        LogsSystem.Instance.Print("回合开始");
    }

    /// <summary>
    /// 打开选择英雄窗口
    /// </summary>
    public void ShowChooseWindow()
    {
        this.chooseCardGrid.Reposition();//刷新排列
        this.chooseCardPanel.alpha = 1;//显示选择窗口
    }

    /// <summary>
    /// 关闭选择英雄窗口
    /// </summary>
    public void CloseChooseWindow()
    {
        this.chooseCardPanel.alpha = 0;
    }
    #endregion
    

   

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

        ShowArrowLine(go);//添加线条

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
    /// 重置被选中的卡片和技能
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

            HideArrowLine();//隐藏线条

            ResetSelectedSkill();//重置选中卡片必定重置选中技能
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

    #region 指向线条操作
    /// <summary>
    /// 添加线条
    /// 如果不存在就创建
    /// </summary>
    public void ShowArrowLine(GameObject go)
    {
        if (arrowLine == null)
        {
            arrowLine = ArrowLine.CreateArrowLine();
        }
        if (arrowLine != null)
        {
            arrowLine.ShowArrowLine(go);
        }
    }
    /// <summary>
    /// 隐藏线条
    /// </summary>
    public void HideArrowLine()
    {
        arrowLine.HideArrowLine();
    }
    #endregion
}