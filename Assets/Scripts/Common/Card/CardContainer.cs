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

    /// <summary>
    /// 更新UI贴图
    /// </summary>
    public void UpdateCardUI()
    {
        GameObject character = transform.FindChild("Character").gameObject;
        GameObject cardDescribe = transform.FindChild("Card-describe").gameObject;

        //更换图片
        character.GetComponent<UISprite>().spriteName = string.Format("Card-{0}", this.card.GetCardName());

        string rarityColorName;
        //选择颜色名
        switch (card.GetCardRarity())
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
        string cardName = CardNames.Instance.GetCardName(this.card.GetCardName());
        character.transform.FindChild("Label").GetComponent<UILabel>().text = cardName;
        cardDescribe.transform.FindChild("Label").GetComponent<UILabel>().text = this.card.GetCardDescription();
    }

    public void SetCardData(Card card)
    {
        this.card = card;
    }

    public Card GetCard()
    {
        return card;
    }
}