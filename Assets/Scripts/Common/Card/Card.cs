using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡片基类
/// </summary>
public class Card : MonoBehaviour
{
    public int cardID;//卡片ID
    public string cardName;//卡片名称
    public CardType cardType;//卡片类型
    public List<CardSkill> cardSkill;//卡片技能列表
    //public UIAtlas atlas;//卡片图集
    public string cardPicName;//卡片图片
    public string cardOwner;//卡片拥有者
    public CardRarity cardRarity;//卡片稀有度
    public string cardDescription;//卡片描述


    public Card()
    {
        this.cardName = "";
        this.cardType = CardType.Character;
        this.cardSkill = new List<CardSkill>();
        this.cardPicName = "Default";
    }

    public Card(int cardId, string cardName, int cardRarity)
    {
        this.cardID = cardId;
        this.cardName = cardName;
        this.cardRarity = (CardRarity)cardRarity;
    }

    public void UpdateCardUI()
    {
        GameObject character = transform.FindChild("Character").gameObject;
        GameObject cardDescribe = transform.FindChild("Card-describe").gameObject;

        //更换图片
        character.GetComponent<UISprite>().spriteName = string.Format("Card-{0}", cardName);

        string rarityColorName;
        //选择颜色名
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
        GetComponent<UISprite>().spriteName = string.Format("CardFrame-{0}-frame", rarityColorName);
        //更换描述信息背景
        cardDescribe.GetComponent<UISprite>().spriteName = string.Format("CardFrame-{0}-describe", rarityColorName);

        //更新文本
        character.GetComponentInChildren<UILabel>().text = CardNames.Instance.GetCardName(cardName);
        cardDescribe.GetComponentInChildren<UILabel>().text = this.cardDescription;
    }

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
