using System;
using System.Collections.Generic;

/// <summary>
/// 护甲类
/// </summary>
public class Armor : EquipmentCard
{
    #region 构造函数
    public Armor(int cardId, string cardName, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, cardRarity, cardDescription)
    {
        //this.attackUpValue = attackUpValue;
        //this.usedTimes = usedTimes;
        //this.equipmentState = new WeaponAttackUp(attackUpValue, usedTimes);
    }
    #endregion

    //暂时不实现
}