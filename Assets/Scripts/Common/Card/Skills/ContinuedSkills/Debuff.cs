/// <summary>
/// Debuff基类
/// </summary>
public abstract class Debuff : StateSkill
{
    protected Debuff(int skillID, string skillCommonName, bool haveIcon = false, string specialIconName = "")
        : base(skillID, skillCommonName, haveIcon, specialIconName)
    {

    }
}
