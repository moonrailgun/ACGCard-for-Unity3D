using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 火球术
/// </summary>
public class Fireball : AttackSkill
{
    public Fireball()
        :base()
    {
        this.skillCommonName = "Fireball";
    }

    public override void OnUse(GameObject target)
    {
        LogsSystem.Instance.Print("对" + target.name + "使用了火球术");
    }
}

