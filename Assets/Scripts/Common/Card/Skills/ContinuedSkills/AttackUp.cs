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

    public AttackUp(int skillID, string skillCommonName, bool haveIcon = false, string specialIconName = "")
        : base(skillID, skillCommonName, haveIcon, specialIconName)
    { }

    public override string GetSkillShowName()
    {
        string showName = string.Format("{0} 攻击力+{1} 剩余{2}回合", SkillNames.Instance.GetSkillName(this.skillCommonName), this.value, this.lastRound);
        return showName;
    }
    public int GetAddedDamage()
    { return this.value; }

    public override JsonData ApplyAppendData(string skillAppendData)
    {
        JsonData json = base.ApplyAppendData(skillAppendData);

        this.value = Convert.ToInt32(json["value"].ToString());

        return json;
    }

    public override void OnUse(CharacterCard toCard, string skillAppendData)
    {
        ApplyAppendData(skillAppendData);

        toCard.AddState(this, null);
    }
}