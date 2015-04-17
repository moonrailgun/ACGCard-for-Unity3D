using UnityEngine;

/// <summary>
/// 技能基类
/// </summary>
public abstract class Skill : ISkill
{
    public virtual void OnUse()
    {
        throw new System.NotImplementedException();
    }

    public void OnUse(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
