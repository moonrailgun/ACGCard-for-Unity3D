using UnityEngine;
using System.Collections;

/// <summary>
/// 用于没有特效只有纯攻击
/// 只修改了名字和图标的技能
/// （为了凑数）
/// </summary>
public class UniversalAttackSkill : AttackSkill {
    /// <summary>
    /// 如果iconName为空则设为未知图标
    /// </summary>
    /// <param name="commonName"></param>
    /// <param name="damage"></param>
    /// <param name="iconName"></param>
    public UniversalAttackSkill(string commonName,int damage,string iconName = "")
        :base()
    {
        SetCommonName(commonName);
        SetBasicDamage(damage);
        if (!string.IsNullOrEmpty(iconName))
        {
            SetIconName(iconName);
        }
        else
        {
            SetIconName("Unknown");
        }
    }

    /// <summary>
    /// 设置通用名
    /// </summary>
    public Skill SetCommonName(string name)
    {
        this.skillCommonName = name;
        return this;
    }
}
