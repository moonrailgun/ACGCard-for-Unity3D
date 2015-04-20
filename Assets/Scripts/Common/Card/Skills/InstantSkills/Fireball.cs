using System;
using System.Collections.Generic;

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

    public override void OnUse()
    {
        throw new NotImplementedException();
    }

    public override void OnUse(UnityEngine.GameObject target)
    {
        throw new NotImplementedException();
    }
}

