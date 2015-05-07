using System;
using System.Collections.Generic;

/// <summary>
/// 首饰类
/// </summary>
public class Jewelry : EquipmentCard
{
    #region 构造函数
    public Jewelry(int cardId, string cardName, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, cardRarity, cardDescription)
    {
        //创建BUFF
    }
    #endregion

    public override void OnEquiped(CharacterCard toCharacterCard)
    {
        //告知角色被装备该装备，如果已有装备则删除以后装备和BUFF
        toCharacterCard.EquipJewelry(this);

        //添加BUFF
        if (equipmentState != null)
        {
            toCharacterCard.AddState(equipmentState, this);
        }
    }
}