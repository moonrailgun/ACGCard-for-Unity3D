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

        gs.SetSelectedCard(container.gameObject);
    }
    public override void OnUse(GameObject target)
    {
        base.OnUse(target);

        CardContainer targetContainer = target.GetComponent<CardContainer>();
        Card targetCard = targetContainer.GetCardData();

        if (targetContainer.GetGameSide() == GameScene.GameSide.Our && targetCard is CharacterCard)
        {
            OnEquiped(this,targetCard as CharacterCard);
            LogsSystem.Instance.Print(string.Format("物品{0}被装备", this.GetCardName()));
        }
        else
        {
            gs.SetSelectedCard(container.gameObject);
            ShortMessagesSystem.Instance.ShowShortMessage("只能给我方英雄装备该道具卡");
        }
    }

    /// <summary>
    /// 当装备被装备时调用该函数
    /// </summary>
    protected virtual void OnEquiped(EquipmentCard from, CharacterCard to) { }
}