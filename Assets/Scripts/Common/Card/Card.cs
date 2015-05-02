using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡片基类
/// 数据层
/// </summary>
public class Card : ICloneable
{
    protected int cardID;//卡片ID
    protected string cardName;//卡片名称
    protected CardType cardType;//卡片类型
    protected string cardOwner;//卡片拥有者
    protected CardRarity cardRarity;//卡片稀有度
    protected string cardDescription;//卡片描述
    protected GameObject container;//容器对象。用于从卡片对象内部访问外部容器

    /// <summary>
    /// 卡片构造函数
    /// </summary>
    public Card()
    {
        Init();
        this.cardName = "";
        this.cardType = CardType.Character;//默认为角色卡
        this.cardRarity = CardRarity.Normal;
    }
    public Card(int cardId, string cardName, int cardRarity)
        : this()
    {
        this.cardID = cardId;
        this.cardName = cardName;
        this.cardRarity = (CardRarity)cardRarity;
        this.cardDescription = CardDescriptions.Instance.GetCardDescription(cardName);
    }
    public Card(int cardId, string cardName, int cardRarity, CardType type)
        : this(cardId, cardName, cardRarity)
    {
        this.cardType = type;
    }
    public Card(int cardId, string cardName, CardType cardType, CardRarity cardRarity, string cardDescription = "")
    {
        Init();
        this.cardID = cardId;
        this.cardName = cardName;
        this.cardType = cardType;
        this.cardRarity = cardRarity;
        if (cardDescription != "")
        {
            this.cardDescription = cardDescription;
        }
        else
        {

            this.cardDescription = CardDescriptions.Instance.GetCardDescription(cardName);
        }
    }

    protected virtual void Init()
    {

    }

    /// <summary>
    /// 根据卡片信息更新卡片UI
    /// </summary>
    /// <param name="container">卡片容器对象</param>
    public virtual void UpdateCardUIBaseByCardInfo(GameObject container)
    {
        GameObject cardDescribe = container.transform.FindChild("Card-describe").gameObject;

        //选择颜色名
        string rarityColorName;
        switch (this.cardRarity)
        {
            case CardRarity.Normal:
                rarityColorName = "white";
                break;
            case CardRarity.Excellent:
                rarityColorName = "green";
                break;
            case CardRarity.Scarce:
                rarityColorName = "blue";
                break;
            case CardRarity.Rare:
                rarityColorName = "gold";
                break;
            case CardRarity.Legend:
                rarityColorName = "purple";
                break;
            case CardRarity.Epic:
                rarityColorName = "orange";
                break;
            default:
                rarityColorName = "gold";
                break;
        }

        //更换背景边框
        container.GetComponent<UISprite>().spriteName = string.Format("CardFrame-{0}-frame", rarityColorName);
        //更换描述信息背景
        cardDescribe.GetComponent<UISprite>().spriteName = string.Format("CardFrame-{0}-describe", rarityColorName);

        //更新文本
        string cardName = CardNames.Instance.GetCardName(this.cardName);
        container.transform.FindChild("Character/Label").GetComponent<UILabel>().text = cardName;
        cardDescribe.transform.FindChild("Label").GetComponent<UILabel>().text = this.cardDescription;
    }

    /// <summary>
    /// 当被技能指向（使用）
    /// </summary>
    /// <param name="skill">卡片被指向的技能</param>
    public virtual void OnSkillUsed(Skill skill,Card from)
    { }

    #region 信息设置
    /// <summary>
    /// 设置卡片容器
    /// </summary>
    public void SetCardContainer(GameObject container)
    {
        this.container = container;
    }
    /// <summary>
    /// 设置所有者
    /// </summary>
    public void SetOwner(string OwnerName)
    {
        this.cardOwner = OwnerName;
    }
    #endregion

    #region 信息获取
    public int GetCardID()
    { return this.cardID; }
    public string GetCardName()
    { return this.cardName; }
    public CardType GetCardType()
    { return this.cardType; }
    public string GetCardOwner()
    { return this.cardOwner; }
    public CardRarity GetCardRarity()
    { return this.cardRarity; }
    public string GetCardDescription()
    { return this.cardDescription; }
    public GameObject GetCardContainer()
    { return this.container; }
    #endregion

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public enum CardType
{
    Character,//角色卡
    Cast,//声优卡
    Item//道具卡
}

public enum CardRarity
{
    Normal = 1,//普通
    Excellent = 2,//精良
    Scarce = 3,//稀有
    Rare = 4,//罕见
    Legend = 5,//传说
    Epic = 6//史诗
}