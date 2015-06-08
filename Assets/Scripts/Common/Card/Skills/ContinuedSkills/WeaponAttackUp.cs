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
    public override void OnCharaterAttack()
    {
        //当角色发送普通攻击后减少使用次数，如果总共持续时间为0则可以无限使用
        /* 由服务端管理是否移除
        if (allLastRound != 0)
        {
            lastRound--;//持续回合递减
            if (lastRound <= 0)
            {
                DestoryState();
            }
        }*/
    }
}