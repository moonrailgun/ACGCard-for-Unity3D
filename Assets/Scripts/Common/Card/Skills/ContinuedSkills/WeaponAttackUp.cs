using System;
using System.Collections.Generic;

public class WeaponAttackUp : AttackUp
{
    #region 构造函数
    public WeaponAttackUp(int value, int lastRound)
        : base(value,lastRound)
    {

    }
    #endregion

    public override void OnRoundStart()
    {
        //清空回合数的递减，设定为什么都不做
    }

    /// <summary>
    /// 当角色普通攻击
    /// </summary>
    public override void OnCharaterAttack()
    {
        //当角色发送普通攻击后减少使用次数，如果总共持续时间为0则可以无限使用
        if (allLastRound != 0)
        {
            lastRound--;//持续回合递减
            if (lastRound <= 0)
            {
                DestoryState();
            }
        }
    }
}