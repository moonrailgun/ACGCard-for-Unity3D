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
        this.damage = 20;//测试用。随便写的
        this.consumedEnergy = 20;
    }
}