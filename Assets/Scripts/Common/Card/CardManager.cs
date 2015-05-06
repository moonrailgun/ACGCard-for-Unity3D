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

    private Dictionary<int, CharacterCard> characterCardMap;
    private Dictionary<int, ItemCard> itemCardMap;
    private Dictionary<int, CastCard> castCardMap;

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
        characterCardMap = new Dictionary<int, CharacterCard>();
        itemCardMap = new Dictionary<int, ItemCard>();
        castCardMap = new Dictionary<int, CastCard>();
    }

    /// <summary>
    /// 注册卡片
    /// </summary>
    public void CardRegister()
    {
        //角色卡
        AddCardGroup(new CharacterCard(1, "Rin", new List<Skill>(new Skill[] { new ArcaneMissiles(), new Fireball() }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(7, "Saber", new List<Skill>(new Skill[] { new UniversalAttackSkill("MeteoriteCut", 40, 40) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(13, "Yaya", new List<Skill>(new Skill[] { new UniversalAttackSkill("Yaya01", 25, 30), new UniversalAttackSkill("Yaya02", 34, 20), new UniversalAttackSkill("Yaya03", 70, 80) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(19, "Rukia", new List<Skill>(new Skill[] { new UniversalAttackSkill("Rukia01", 20, 20), new UniversalAttackSkill("Rukia02", 40, 40), new UniversalAttackSkill("Rukia03", 60, 60) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(25, "Illyasviel", new List<Skill>(new Skill[] { new UniversalAttackSkill("Illyasviel01",50,40) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(31, "Asuna", new List<Skill>(new Skill[] { new UniversalAttackSkill("Asuna01", 40, 10), new UniversalAttackSkill("Asuna02",60,40) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(37, "Haruhi", new List<Skill>(new Skill[] { new UniversalAttackSkill("Haruhi01", 20, 10), new UniversalAttackSkill("Haruhi02",30,30) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(43, "Kurumi", new List<Skill>(new Skill[] { new UniversalAttackSkill("Kurumi01", 25, 10), new UniversalAttackSkill("Kurumi02", 40, 30), new UniversalAttackSkill("Kurumi03",70,50) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(49, "Lucy", new List<Skill>(new Skill[] { new UniversalAttackSkill("Lucy01", 30, 50), new UniversalAttackSkill("Lucy02", 40, 40), new UniversalAttackSkill("Lucy03",60,50) }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(55, "Luotianyi", new List<Skill>(new Skill[] { }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(61, "Rikka", new List<Skill>(new Skill[] { }), CardRarity.Normal));
        AddCardGroup(new CharacterCard(67, "ShiRo", new List<Skill>(new Skill[] { }), CardRarity.Normal));

        //物品卡
        AddCard(new Weapon(1, "Excalibur", CardRarity.Legend, 50, 3));
        AddCard(new EquipmentCard(2, "Avalon", CardRarity.Legend));
        AddCard(new Weapon(3, "IceSword", CardRarity.Excellent, 10, 5));
        AddCard(new EquipmentCard(4, "Shield", CardRarity.Normal));

        LogsSystem.Instance.Print(string.Format("卡片注册完毕。共注册卡片{0}个", characterCardMap.Count));
    }

    /// <summary>
    /// 单个添加卡片
    /// </summary>
    public void AddCard(Card card)
    {
        if (card is CharacterCard)
        {
            int cardID = card.GetCardID();
            if (!characterCardMap.ContainsKey(cardID))
                characterCardMap.Add(cardID, card as CharacterCard);
            else
                LogsSystem.Instance.Print("[卡片管理器]无法向角色卡添加卡片：重复的ID号" + cardID, LogLevel.WARN);
        }
        else if (card is ItemCard)
        {
            int cardID = card.GetCardID();
            if (!itemCardMap.ContainsKey(cardID))
                itemCardMap.Add(cardID, card as ItemCard);
            else
                LogsSystem.Instance.Print("[卡片管理器]无法向角色卡添加卡片：重复的ID号" + cardID, LogLevel.WARN);
        }
        else if (card is CastCard)
        {
            int cardID = card.GetCardID();
            if (!castCardMap.ContainsKey(cardID))
                castCardMap.Add(cardID, card as CastCard);
            else
                LogsSystem.Instance.Print("[卡片管理器]无法向角色卡添加卡片：重复的ID号" + cardID, LogLevel.WARN);
        }
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
            //通用成员属性
            int cardId = beginid + i - 1;
            string cardName = beginCard.GetCardName();
            CardRarity cardRarity = (CardRarity)i;
            string cardDescription = beginCard.GetCardDescription();

            if (beginCard is CharacterCard)//如果是角色卡
            {
                //角色卡成组通用参数
                CharacterCard beginCharacterCard = beginCard as CharacterCard;
                List<Skill> cardSkill = beginCharacterCard.GetCardSkillList();

                AddCard(new CharacterCard(cardId, cardName, cardSkill, cardRarity, cardDescription));
            }
            else if (beginCard is ItemCard)
            {
                AddCard(new ItemCard(cardId, cardName, cardRarity, cardDescription));
                //AddCard(new Card(beginid + i - 1, beginCard.GetCardName(), beginCard.GetCardType(), beginCard.GetCardSkillList(), (CardRarity)i, beginCard.GetCardDescription()));
            }
        }
    }

    /// <summary>
    /// 获取卡片的拷贝
    /// </summary>
    private Card GetCardById(int id, CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Character:
                {
                    if (characterCardMap.ContainsKey(id))
                    {
                        Card card = characterCardMap[id];
                        return card.Clone() as Card;
                    }
                    else
                    {
                        return null;
                    }
                }
            case CardType.Item:
                {
                    if (itemCardMap.ContainsKey(id))
                    {
                        Card card = itemCardMap[id];
                        return card.Clone() as Card;
                    }
                    else
                    {
                        return null;
                    }
                }
            case CardType.Cast:
                {
                    if (castCardMap.ContainsKey(id))
                    {
                        Card card = castCardMap[id];
                        return card.Clone() as Card;
                    }
                    else
                    {
                        return null;
                    }
                }
            default:
                {
                    LogsSystem.Instance.Print("[卡片管理器]获取卡片拷贝时发生错误：未知的卡片类型");
                    break;
                }
        }
        return null;
    }

    /// <summary>
    /// 获取角色卡的拷贝
    /// </summary>
    public CharacterCard GetCharacterById(int id, int level, int health, int energy, int baseDamage, int baseSpeed)
    {
        CharacterCard card = GetCardById(id, CardType.Character) as CharacterCard;
        if (card != null)
            card.SetCharacterInfo(level, health, energy, baseDamage, baseSpeed);
        return card;
    }

    public ItemCard GetItemById(int id)
    {
        return GetCardById(id, CardType.Item) as ItemCard;
    }
}