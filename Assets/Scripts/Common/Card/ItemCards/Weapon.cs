using System;
using System.Collections.Generic;

public class Weapon : EquipmentCard
{
    protected int attackUpValue;//装备后攻击提升的值
    protected int usedTimes;//装备可使用次数，0为无限使用

    #region 构造函数
    public Weapon(int cardId, string cardName, CardRarity cardRarity,int attackUpValue,int usedTimes, string cardDescription = "")
        : base(cardId, cardName, cardRarity, cardDescription)
    {
        this.attackUpValue = attackUpValue;
        this.usedTimes = usedTimes;
    }
    #endregion

    protected override void OnEquiped(EquipmentCard from, CharacterCard to)
    {
        WeaponAttackUp state = new WeaponAttackUp(attackUpValue, usedTimes);

        to.AddState(state);
    }
}
