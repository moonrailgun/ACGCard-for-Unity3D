using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackUp : AttackUp
{
    #region 构造函数
    public WeaponAttackUp(int skillID, string skillCommonName, bool haveIcon = false, string specialIconName = "")
        : base(skillID, skillCommonName, haveIcon, specialIconName)
    {

    }
    #endregion

    public override void OnRoundStart()
    {
        //清空回合数的递减，对于武器设定为什么都不做
    }

    /// <summary>
    /// 当角色普通攻击
    /// </summary>
    public override void OnCharacterAttack()
    {
        base.OnCharacterAttack();

        if (allLastRound != 0)
        {
            lastRound--;//持续回合递减
        }
    }
}