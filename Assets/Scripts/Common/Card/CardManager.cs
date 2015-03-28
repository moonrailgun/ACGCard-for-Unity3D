using System;
using System.Collections.Generic;

/// <summary>
/// 卡片管理类
/// </summary>
public class CardManager {
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

    }

    /// <summary>
    /// 添加卡片
    /// </summary>
    public void AddCard(Card card)
    {
        int hashcode = card.GetHashCode();
        cardMap.Add(hashcode, card);
    }
}
