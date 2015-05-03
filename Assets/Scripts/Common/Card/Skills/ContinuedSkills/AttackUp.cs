using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 攻击提升
/// </summary>
public class AttackUp : Buff
{
    public AttackUp()
        : base()
    {

    }

    public AttackUp(string skillCommonName, int lastRound)
        : base()
    {
        this.skillCommonName = skillCommonName;
        this.lastRound = lastRound;
    }
}