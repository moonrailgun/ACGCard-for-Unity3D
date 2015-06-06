using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 攻击提升
/// </summary>
public class AttackUp : Buff
{
    protected int value;//攻击力增加的值

    public AttackUp(int value, int lastRound)
        : base(lastRound)
    {
        this.skillCommonName = "AttackUp";
        this.value = value;
    }

    public string GetSkillShowName()
    {
        string showName = string.Format("{0} 攻击力+{1} 剩余{2}回合", SkillNames.Instance.GetSkillName(this.skillCommonName), value, lastRound);
        return showName;
    }
    public int GetAddedDamage()
    { return this.value; }

    public override void OnUse(CharacterCard toCard, string skillAppendData)
    {
        JsonData skillData = JsonMapper.ToObject(skillAppendData);
        this.value = Convert.ToInt32(skillData["value"].ToString());

        throw new NotImplementedException();
    }
}