using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡片管理类
/// </summary>
public class CardManager
{
    #region 单例模式
    private static CardManager _instance;
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardManager();
            }
            return _instance;
        }
    }
    #endregion

    private Dictionary<int, Card> cardMap;

    public CardManager()
    {
        Init();
        CardRegister();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        cardMap = new Dictionary<int, Card>();
    }

    /// <summary>
    /// 注册卡片
    /// </summary>
    public void CardRegister()
    {
        AddCardGroup(new CharacterCard(1, "Rin", CardType.Character, new List<Skill>(new Skill[] { new ArcaneMissiles(), new Fireball() }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(7, "Saber", 1));
        AddCardGroup(new CharacterCard(13, "Yaya", 1));
        AddCardGroup(new CharacterCard(19, "Rukia", 1));
        AddCardGroup(new CharacterCard(25, "Illyasviel", 1));
        AddCardGroup(new CharacterCard(31, "Asuna", 1));
        AddCardGroup(new CharacterCard(37, "Haruhi", 1));
        AddCardGroup(new CharacterCard(43, "Kurumi", 1));
        AddCardGroup(new CharacterCard(49, "Lucy", 1));
        AddCardGroup(new CharacterCard(55, "Luotianyi", 1));
        AddCardGroup(new CharacterCard(61, "Rikka", 1));
        AddCardGroup(new CharacterCard(67, "ShiRo", 1));

        LogsSystem.Instance.Print(string.Format("卡片注册完毕。共注册卡片{0}个", cardMap.Count));

        //Debug.Log(cardMap[1].GetCardSkillList()[0].skillCommonName);
    }

    /// <summary>
    /// 添加卡片
    /// </summary>
    public void AddCard(Card card)
    {
        int cardID = card.GetCardID();
        if (!cardMap.ContainsKey(cardID))
            cardMap.Add(cardID, card);
        else
            LogsSystem.Instance.Print("[卡片管理器]重复的ID号" + cardID, LogLevel.WARN);
    }

    /// <summary>
    /// 成组添加卡片
    /// 一组六个
    /// </summary>
    public void AddCardGroup(Card beginCard)
    {
        int beginid = beginCard.GetCardID();

        for (int i = 1; i <= 6; i++)
        {
            if (beginCard is CharacterCard)
            {
                AddCard(new CharacterCard(beginid + i - 1, beginCard.GetCardName(), beginCard.GetCardType(), beginCard.GetCardSkillList(), (CardRarity)i, beginCard.GetCardDescription()));
            }
            else
            {
                AddCard(new Card(beginid + i - 1, beginCard.GetCardName(), beginCard.GetCardType(), beginCard.GetCardSkillList(), (CardRarity)i, beginCard.GetCardDescription()));
            }
        }
    }

    /// <summary>
    /// 获取卡片的拷贝
    /// </summary>
    public Card GetCardById(int id)
    {
        if (cardMap.ContainsKey(id))
        {
            Card card = cardMap[id];
            return card.Clone() as Card;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 获取角色卡的拷贝
    /// </summary>
    public Card GetCardById(int id, int health,int energy)
    {
        Card card = GetCardById(id);
        if (card is CharacterCard)
        {
            CharacterCard characterCard = card as CharacterCard;
            characterCard.SetCharacterInfo(health, energy);
            return characterCard;
        }
        return card;
    }
}