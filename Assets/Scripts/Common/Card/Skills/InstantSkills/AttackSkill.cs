using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSkill : Skill
{
    protected int damage = 0;

    protected AttackSkill()
        : base()
    {

    }

    public void SetBasicDamage(int value)
    {
        this.damage = value;
    }

    /// <summary>
    /// 返回造成的伤害
    /// </summary>
    /// <returns></returns>
    public virtual int GetCalculatedDamage()
    {
        return this.damage;
    }
    /// <summary>
    /// 根据技能来源（技能释放者）计算伤害
    /// </summary>
    public virtual int GetCalculatedDamage(Card from)
    {
        return this.GetCalculatedDamage();
    }

    public override void OnUse()
    {
        gameScene.SetSelectedSkill(this);
    }
    public override void OnUse(GameObject from, GameObject target)
    {
        if (CheckConjureCondition(from))
        {
            Card skillOrigin = from.GetComponent<CardContainer>().GetCardData();//技能源数据
            Card skillBelong = target.GetComponent<CardContainer>().GetCardData();//技能归属数据
            LogsSystem.Instance.Print(skillOrigin.GetCardName() + "攻击了" + skillBelong.GetCardName());
            skillBelong.OnSkillUsed(this, skillOrigin);

            //--创建技能特效
        }
    }

    public override void OnUse(GameObject target)
    {
        throw new NotImplementedException();
    }
}