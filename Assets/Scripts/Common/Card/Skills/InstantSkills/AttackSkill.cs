using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSkill : Skill
{
    protected AttackSkill()
        :base()
    {

    }

    public override void OnUse()
    {
        gameScene.SetSelectedSkill(this);
    }
}