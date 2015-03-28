using UnityEngine;

/// <summary>
/// 卡片技能基类
/// </summary>
public class CardSkill {
    public string skillName;
    public SkillUsedTarget target;
    public Color skillColor;

    public void OnSkillUse()
    {

    }
}

public enum SkillUsedTarget
{
    ALL,
    Cast,
    Item,
    Character,
    CastAndItem,
    CastAndCharacter,
    ItemAndCharacter,
    Scene,
    Self
}
