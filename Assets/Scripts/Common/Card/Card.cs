using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡片基类
/// 数据层
/// </summary>
public class Card
{
    private int cardID;//卡片ID
    private string cardName;//卡片名称
    private CardType cardType;//卡片类型
    private List<Skill> cardSkill;//卡片技能列表
    private string cardOwner;//卡片拥有者
    private CardRarity cardRarity;//卡片稀有度
    private string cardDescription;//卡片描述

    /// <summary>
    /// 卡片构造函数
    /// </summary>
    public Card()
    {
        this.cardName = "";
        this.cardType = CardType.Character;
        this.cardSkill = new List<Skill>();
        this.cardRarity = CardRarity.Normal;
        this.cardDescription = CardDescriptions.Instance.GetCardDescription(cardName);
    }
    public Card(int cardId, string cardName, int cardRarity)
        : this()
    {
        this.cardID = cardId;
        this.cardName = cardName;
        this.cardRarity = (CardRarity)cardRarity;
    }
    public Card(int cardId, string cardName, int cardRarity, CardType type)
        : this(cardId, cardName, cardRarity)
    {
        this.cardType = type;
    }
    public Card(int cardId, string cardName, CardType cardType, List<Skill> cardSkill, CardRarity cardRarity, string cardDescription = "")
    {
        this.cardID = cardId;
        this.cardName = cardName;
        this.cardType = cardType;
        this.cardSkill = cardSkill;
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

    /// <summary>
    /// 设置卡片信息
    /// </summary>
    /// <param name="info"></param>
    public void SetCardInfo(Hashtable info)
    {
        if (info.ContainsKey("CardID"))
            this.cardID = (int)info["CardID"];
        if (info.ContainsKey("CardName"))
            this.cardName = (string)info["CardName"];
        if (info.ContainsKey("CardType"))
            this.cardType = (CardType)info["CardType"];


        if (info.ContainsKey("CardOwner"))
            this.cardOwner = (string)info["CardOwner"];
        if (info.ContainsKey("CardRarity"))
            this.cardRarity = (CardRarity)info["CardRarity"];
        if (info.ContainsKey("CardDescription"))
            this.cardDescription = (string)info["CardDescription"];
    }
    public void SetCardInfo(Card card)
    {
        this.cardID = card.cardID;
        this.cardName = card.cardName;
        this.cardName = card.cardName;
        this.cardType = card.cardType;
        this.cardSkill = card.cardSkill;
        this.cardOwner = card.cardOwner;
        this.cardRarity = card.cardRarity;
        this.cardDescription = card.cardDescription;
    }

    public void SetOwner(string OwnerName)
    {
        this.cardOwner = OwnerName;
    }

    #region 信息获取
    public int GetCardID()
    {
        return this.cardID;
    }
    public string GetCardName()
    {
        return this.cardName;
    }

    public CardType GetCardType()
    {
        return this.cardType;
    }
    public List<Skill> GetCardSkillList()
    {
        return this.cardSkill;
    }
    public string GetCardOwner()
    {
        return this.cardOwner;
    }
    public CardRarity GetCardRarity()
    {
        return this.cardRarity;
    }
    public string GetCardDescription()
    {
        return this.cardDescription;
    }
    #endregion
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