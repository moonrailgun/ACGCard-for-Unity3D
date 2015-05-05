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
        this.consumedEnergy = 60;
    }
}