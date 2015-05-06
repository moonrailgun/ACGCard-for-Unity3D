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
    protected int allLastRound;//总共可以持续的回合数,0为无限使用
    protected CharacterCard ownerCard;//该状态的所有者

    protected StateSkill(int lastRound)
        : base()
    {
        //配置持续回合数
        this.lastRound = lastRound;
        this.allLastRound = lastRound;
    }

    /// <summary>
    /// 当回合开始时调用该函数
    /// 根据不同的状态可能会有不同的调用（可重写），因此不能在外部检测
    /// </summary>
    public virtual void OnRoundStart()
    {
        if (allLastRound != 0)
        {
            lastRound--;//持续回合递减
            if (lastRound <= 0)
            {
                DestoryState();
            }
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
        if (ownerCard != null)
        {
            Card origin = ownerCard.GetStateOrigin(this);
            if (origin is EquipmentCard)
            {
                //解除角色武装
                (origin as EquipmentCard).OnUnequiped(ownerCard);
            }
            else
            {
                ownerCard.RemoveState(this);
            }
        }
    }

    public override string GetSkillShowName()
    {
        string showName = string.Format("{0} 剩余：{1}回合", base.GetSkillShowName(), lastRound);
        return showName;
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

    #region 角色事件监听
    /// <summary>
    /// 当角色发动普通攻击时调用
    /// </summary>
    public virtual void OnCharaterAttack() { }
    #endregion


    #region 外部访问接口
    public int GetLastRound()
    { return this.lastRound; }
    public CharacterCard GetOwnerCard()
    { return this.ownerCard; }
    #endregion
}