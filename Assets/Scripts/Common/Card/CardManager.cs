using System;
using System.Collections.Generic;

/// <summary>
/// 卡片管理类
/// </summary>
public class CardManager
{
    public Dictionary<int, Card> cardMap;

    public CardManager()
    {
        CardRegister();
    }

    /// <summary>
    /// 注册卡片
    /// </summary>
    public void CardRegister()
    {
        AddCardGroup(new Card(1, "Rin", 1));

        LogsSystem.Instance.Print(string.Format("卡片注册完毕。共注册卡片{0}个", cardMap.Count));
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
    /// </summary>
    public void AddCardGroup(Card beginCard)
    {
        int beginid = beginCard.GetCardID();

        for (int i = 1; i <= 6; i++)
        {
            AddCard(new Card(beginid + i - 1, beginCard.GetCardName(), beginCard.GetCardType(), beginCard.GetCardSkillList(), (CardRarity)i, beginCard.GetCardDescription()));
        }
    }
}