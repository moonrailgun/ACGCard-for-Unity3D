using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽象
/// 状态类技能基类
/// </summary>
public abstract class StateSkill : Skill
{
    protected int lastRound;//可以持续的回合数
    protected CharacterCard ownerCard;//该状态的所有者

    protected StateSkill()
        : base()
    {

    }

    /// <summary>
    /// 当回合开始时调用该函数
    /// 根据不同的状态可能会有不同的调用（可重写），因此不能在外部检测
    /// </summary>
    public virtual void OnRoundStart()
    {
        lastRound--;//持续回合递减
        if (lastRound <= 0)
        {
            DestoryState();
        }
    }

    /// <summary>
    /// 设置该状态的所有者
    /// </summary>
    public void SetOwnerCard(CharacterCard ownerCard)
    {
        this.ownerCard = ownerCard;
    }

    /// <summary>
    /// 销毁该状态
    /// （状态自毁）
    /// </summary>
    public void DestoryState()
    {
        ownerCard.RemoveState(this);
    }

    public override void OnUse()
    {
        throw new NotImplementedException();
    }

    public override void OnUse(GameObject target)
    {
        throw new NotImplementedException();
    }

    public override void OnUse(GameObject from, GameObject target)
    {
        throw new NotImplementedException();
    }
}