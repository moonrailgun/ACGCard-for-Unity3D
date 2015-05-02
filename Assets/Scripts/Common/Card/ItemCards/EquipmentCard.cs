using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备卡
/// </summary>
public class EquipmentCard : ItemCard
{
    #region 构造函数
    public EquipmentCard(int cardId, string cardName, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, cardRarity, cardDescription)
    { }
    #endregion

    public override void OnUse()
    {
        base.OnUse();

        gs.SetSelectedCard(container);
    }
    public override void OnUse(GameObject target)
    {
        base.OnUse(target);

        CardContainer targetContainer = target.GetComponent<CardContainer>();
        Card targetCard = targetContainer.GetCardData();

        if (targetContainer.GetGameSide() == GameScene.GameSide.Our&& targetCard is CharacterCard)
        {
            //--这里实现装备的装备
            LogsSystem.Instance.Print("物品被装备");
        }
        else
        {
            gs.SetSelectedCard(container);
            ShortMessagesSystem.Instance.ShowShortMessage("只能给我方英雄装备该道具卡");
        }
    }
}