using System;
using System.Collections.Generic;

/// <summary>
/// 武器类
/// </summary>
public class Weapon : EquipmentCard
{
    protected int attackUpValue;//装备后攻击提升的值
    protected int usedTimes;//装备可使用次数，0为无限使用


    #region 构造函数
    public Weapon(int cardId, string cardName, CardRarity cardRarity, int attackUpValue, int usedTimes, string cardDescription = "")
        : base(cardId, cardName, cardRarity, cardDescription)
    {
        this.attackUpValue = attackUpValue;
        this.usedTimes = usedTimes;
       // this.equipmentState = new WeaponAttackUp(attackUpValue, usedTimes);
    }
    #endregion

    public override void OnEquiped(CharacterCard toCharacterCard)
    {
        //告知角色被装备该装备，如果已有装备则删除以后装备和BUFF
        toCharacterCard.EquipWeapon(this);

        //添加BUFF
        toCharacterCard.AddState(equipmentState, this);
    }

    #region 外部访问接口
    public int GetAttackUpValue()
    { return this.attackUpValue; }
    public int GetUsedTimes()
    { return this.usedTimes; }
    #endregion
}
