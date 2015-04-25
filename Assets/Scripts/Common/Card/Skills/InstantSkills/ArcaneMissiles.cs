using UnityEngine;
using System.Collections;

/// <summary>
/// 奥术飞弹
/// </summary>
public class ArcaneMissiles : AttackSkill
{
    public ArcaneMissiles()
        :base()
    {
        this.skillCommonName = "ArcaneMissiles";
        this.damage = 10;//测试用。随便写的
    }

    public override void OnUse(GameObject target)
    {
        LogsSystem.Instance.Print("对" + target.name + "使用了奥术飞弹");
    }
}