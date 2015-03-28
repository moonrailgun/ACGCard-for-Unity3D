using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 卡片基类
/// </summary>
public class Card {
    public string cardName;//卡片名称
    public CardType cardType;//卡片类型
    public List<CardSkill> cardSkill;//卡片技能列表
    public UIAtlas atlas;//卡片图集
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

    public GameObject GetCardGameobject()
    {
        //TODO
        return null;
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
    Normal,//普通
    Excellent,//精良
    Scarce,//稀有
    Rare,//罕见
    Legend,//传说
    Epic//史诗
}
