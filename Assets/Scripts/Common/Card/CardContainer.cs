using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡片类容器
/// 对外接口
/// </summary>
public class CardContainer : MonoBehaviour
{
    private Card card;

    private void Awake()
    {
        //当没有卡片数据时，添加一份空数据
        if (this.card == null)
        {
            this.card = new Card();
        }
    }

    public void OnSelected()
    {

    }

    /// <summary>
    /// 更新UI贴图
    /// </summary>
    public void UpdateCardUI()
    {
        card.UpdateCardUIBaseByCardInfo(gameObject);
    }

    public void SetCardData(Card card)
    {
        this.card = card;
    }
    public Card GetCardData()
    {
        return card;
    }
}