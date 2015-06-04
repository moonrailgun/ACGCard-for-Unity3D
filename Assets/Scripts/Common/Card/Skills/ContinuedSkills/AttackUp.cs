using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 攻击提升
/// </summary>
public class AttackUp : Buff
{
    protected int addedValue;//攻击力增加的值

    public AttackUp(int value, int lastRound)
        : base(lastRound)
    {
        this.skillCommonName = "AttackUp";
        this.addedValue = value;
    }

    public override string GetSkillShowName()
    {
        string showName = string.Format("{0} 攻击力+{1} 剩余{2}回合", SkillNames.Instance.GetSkillName(this.skillCommonName), addedValue, lastRound);
        return showName;
    }
    public int GetAddedDamage()
    { return this.addedValue; }

    public override void OnUse(CharacterCard toCard, string skillAppendData)
    {
        throw new NotImplementedException();
    }
}