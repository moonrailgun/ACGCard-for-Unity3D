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
    public override void OnUse(GameObject from, GameObject target)
    {
        LogsSystem.Instance.Print(from.name + "攻击了" + target.name);
    }
}