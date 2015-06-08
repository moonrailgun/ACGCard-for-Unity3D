/// <summary>
/// Buff基类
/// </summary>
public abstract class Buff : StateSkill
{
    protected Buff(int skillID, string skillCommonName, bool haveIcon = false, string specialIconName = "")
        : base(skillID, skillCommonName, haveIcon, specialIconName)
    {

    }
}
