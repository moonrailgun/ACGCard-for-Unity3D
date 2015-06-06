using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class AttackSkill : Skill
{
    protected int damage = 0;

    public AttackSkill(string commonName, bool haveIcon = false, string specialIconName = "")
        : base()
    {
        this.skillCommonName = commonName;
        if (haveIcon)
        {
            if (string.IsNullOrEmpty(specialIconName))
            { SetIconName(commonName); }
            else
            { SetIconName(specialIconName); }
        }
        else
        { SetIconName("Unknown"); }
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

    public override void OnUse(CharacterCard toCard, string skillAppendData)
    {
        JsonData skillData = JsonMapper.ToObject(skillAppendData);
        int damage = Convert.ToInt32(skillData["damage"].ToString());
        int energy = Convert.ToInt32(skillData["energy"].ToString());

        toCard.GetDamage(damage);
        toCard.ExpendEnergy(energy);
    }
}