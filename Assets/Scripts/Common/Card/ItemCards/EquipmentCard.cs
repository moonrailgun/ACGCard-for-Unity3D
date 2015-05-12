using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备卡
/// </summary>
public class EquipmentCard : ItemCard
{
    protected StateSkill equipmentState;//装备拥有的状态

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

        if (targetContainer.GetGameSide() == GameManager.GameSide.Our && targetCard is CharacterCard)
        {
            OnEquiped(targetCard as CharacterCard);
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
    /// 因为装备类型不一定所以需要由装备发起调用
    /// </summary>
    public virtual void OnEquiped(CharacterCard toCharacterCard) { }

    /// <summary>
    /// 当装备被卸下时调用
    /// 删除buff
    /// </summary>
    public virtual void OnUnequiped(CharacterCard toCharacterCard)
    {
        if (this.equipmentState != null)
            toCharacterCard.RemoveState(this.equipmentState);
    }
}