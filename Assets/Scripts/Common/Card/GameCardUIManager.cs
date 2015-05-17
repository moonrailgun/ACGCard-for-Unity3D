using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 游戏界面侧边栏
/// </summary>
public class GameCardUIManager : MonoBehaviour
{
    private GameScene gameSceneManager;
    private UILabel CardName;
    private UILabel CardType;
    private UILabel CardRarity;
    private UILabel CardOwner;
    private UILabel CardDes;
    private UIGrid CardSkillList;
    private UIPanel CardDesPanel;

    private void Awake()
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            this.CardName = GameObject.Find("CardInfo/Name/Label").GetComponent<UILabel>();
            this.CardType = GameObject.Find("CardInfo/Type/Label").GetComponent<UILabel>();
            this.CardRarity = GameObject.Find("CardInfo/Rarity/Label").GetComponent<UILabel>();
            this.CardOwner = GameObject.Find("CardInfo/Owner/Label").GetComponent<UILabel>();
            this.CardDes = GameObject.Find("CardInfo/Description/Label").GetComponent<UILabel>();
            this.CardSkillList = GameObject.Find("SkillList/Grid").GetComponent<UIGrid>();
            this.CardDesPanel = GameObject.Find("GamePanel/CardDes").GetComponent<UIPanel>();

            this.CardName.text = "";
            this.CardType.text = "";
            this.CardRarity.text = "";
            this.CardOwner.text = "";
            this.CardDes.text = "";

            this.gameSceneManager = GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<GameScene>();
            ClearSkillButton();
        }
    }

    /// <summary>
    /// 添加角色卡事件监听
    /// </summary>
    public void AddCharacterUIListener(GameObject go, GameManager.GameSide side)
    {
        UIEventListener listener = UIEventListener.Get(go);

        //点击事件
        if (side == GameManager.GameSide.Our)
        {
            listener.onClick += OnOurCharacterCardSelected;
        }
        else if (side == GameManager.GameSide.Enemy)
        {
            listener.onClick += OnEnemyCharacterCardSelected;
        }

        //鼠标指向事件
        listener.onHover += OnCharacterCardHover;
    }

    /// <summary>
    /// 添加手牌事件监听
    /// </summary>
    public void AddHandUIListener(GameObject go)
    {
        UIEventListener listener = UIEventListener.Get(go);

        listener.onHover += OnItemCardHover;
        listener.onClick += OnItemCardSelected;
    }

    #region 卡片被指向
    /// <summary>
    /// 当角色卡片被指向后
    /// 显示放大版的卡片
    /// </summary>
    private void OnCharacterCardHover(GameObject go, bool state)
    {
        if (state == true)
        {
            CardDesPanel.alpha = 1;

            //修改显示的内容
            Card cardData = go.GetComponent<CardContainer>().GetCardClone();//获取卡片的克隆
            CardContainer desCardContainer = CardDesPanel.transform.FindChild("Container/Card").gameObject.GetComponent<CardContainer>();
            desCardContainer.SetCardData(cardData);//设置卡片数据
            desCardContainer.UpdateCardUI();//根据卡片数据刷新卡片UI
            CardDesPanel.GetComponent<CardDesPanel>().UpdateDesPanelUI(cardData);//更新描述面板信息

            //修改显示的位置
            Vector2 cardSize = go.GetComponent<UISprite>().localSize;
            CardDesPanel.transform.parent = go.transform;//成为该物体子物体
            CardDesPanel.transform.localPosition = new Vector3(cardSize.x / 2 + 10, 0, 0);//位置是详情最左端向右偏移半个卡片距离+10像素
            CardDesPanel.transform.localScale = Vector3.one;
        }
        else
        {
            CardDesPanel.alpha = 0;
        }
    }

    /// <summary>
    /// 当物品卡片被指向
    /// </summary>
    private void OnItemCardHover(GameObject go, bool state)
    {
        UITweener tweener = go.GetComponent<UITweener>();
        float upDistance = 50;

        if (state)
        {
            if (tweener != null) { tweener.PlayForward(); }

            Hashtable args = new Hashtable();
            args.Add("y", upDistance);
            args.Add("islocal", true);
            args.Add("time", 0.4f);

            iTween.MoveTo(go, args);
        }
        else
        {
            if (tweener != null) { tweener.PlayReverse(); }

            iTween.Stop(go);

            Hashtable args = new Hashtable();
            args.Add("y", 0);
            args.Add("islocal", true);
            args.Add("time", 0.4f);

            iTween.MoveTo(go, args);
            //HandCardTable.Reposition();
        }


    }
    #endregion

    #region 卡片选中
    /// <summary>
    /// 我方卡片选中
    /// 生成技能列表
    /// 修改卡片信息
    /// 设定为选中
    /// </summary>
    private void OnOurCharacterCardSelected(GameObject go)
    {
        CardContainer container = go.GetComponent<CardContainer>();
        Card card = container.GetCardData();
        if (container != null && card != null && Global.Instance.scene == SceneType.GameScene && card is CharacterCard)//数据正常
        {
            Skill selectedSkill = gameSceneManager.GetSelectedSkill();
            GameObject selectedCard = gameSceneManager.GetSelectedCard();//获得已经被选中的我方卡片
            Card selectedCardData = null;//选中卡片的数据
            if (selectedCard != null)
            {
                selectedCardData = selectedCard.GetComponent<CardContainer>().GetCardData();//获取选中的卡片数据
            }

            if (selectedCard != null && selectedSkill != null && (selectedSkill is Buff))
            {
                //如果已经选中了技能并且技能是BUFF类（可以对己方使用）
                if (selectedSkill != null)
                {
                    selectedSkill.OnUse(selectedCard, go);//技能被使用（从Card到go）
                }
            }
            else if (selectedCard != null && selectedCardData is ItemCard)
            {
                //已经被选中的卡片为手牌
                ItemCard item = selectedCardData as ItemCard;
                item.OnUse(go);
            }
            else
            {
                //将改卡设定为初始选中卡

                if (go.transform.IsChildOf(GameObject.Find("Ourside/CardGrid").transform))
                {
                    //清空技能按钮列表
                    ClearSkillButton();
                }

                ShowCardInfo(card);//显示出卡片信息

                //显示卡片技能列表
                List<Skill> skillList = ((CharacterCard)card).GetCardSkillList();
                foreach (Skill skill in skillList)
                {
                    skill.CreateSkillButton("SkillList/Grid");//在SkillList/Grid目录下创建技能图标
                }

                //设置被选中
                this.gameSceneManager.SetSelectedCard(go);
            }
        }
        else
        {
            LogsSystem.Instance.Print("数据异常", LogLevel.WARN);
        }
    }
    /// <summary>
    /// 敌方卡片被选中
    /// </summary>
    private void OnEnemyCharacterCardSelected(GameObject go)
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            Skill skill = gameSceneManager.GetSelectedSkillAndReset();
            GameObject selectedCard = gameSceneManager.GetSelectedCard();//已经被选中的我方卡片
            if (selectedCard != null)
            {
                Card selectedCardData = selectedCard.GetComponent<CardContainer>().GetCardData();//获取选中的卡片数据

                if (skill != null)
                {
                    skill.OnUse(selectedCard, go);//技能被使用（从Card到go)
                    gameSceneManager.ResetSelectedCard();
                }
                else if (selectedCardData is CharacterCard)
                {
                    //普通攻击
                    CharacterCard character = selectedCardData as CharacterCard;
                    gameSceneManager.gameManager.RequestCharacterAttack(character, go.GetComponent<CardContainer>().GetCardData() as CharacterCard);//向服务器请求攻击
                    gameSceneManager.ResetSelectedCard();//重置选中的卡
                }
                else if (selectedCardData is ItemCard)
                {
                    //已经被选中的卡片为手牌
                    ItemCard item = selectedCardData as ItemCard;
                    item.OnUse(go);//使用对某使用道具
                }
            }
            else
            {
                ShortMessagesSystem.Instance.ShowShortMessage("请选择技能或卡片");
            }
        }
    }

    /// <summary>
    /// 手牌被选中
    /// </summary>
    private void OnItemCardSelected(GameObject go)
    {
        try
        {
            CardContainer container = go.GetComponent<CardContainer>();
            ItemCard item = container.GetCardData() as ItemCard;
            item.OnUse();
        }
        catch (Exception ex)
        { LogsSystem.Instance.Print("出现错误" + ex, LogLevel.WARN); }
    }

    #endregion

    /// <summary>
    /// 清空技能列表
    /// </summary>
    private void ClearSkillButton()
    {
        int childcount = CardSkillList.transform.childCount;
        for (int i = childcount; i > 0; i--)
        {
            DestroyImmediate(CardSkillList.transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// 根据脚本显示出卡片信息
    /// </summary>
    private void ShowCardInfo(Card card)
    {
        this.CardName.text = card.GetCardName();
        this.CardType.text = CardTypes.GetCardTypeNames(card.GetCardType());
        this.CardRarity.text = CardRaritys.GetCardRarityNames(card.GetCardRarity());
        this.CardOwner.text = card.GetCardOwner();
        this.CardDes.text = card.GetCardDescription();
    }
}