using System;
using System.Collections.Generic;

public class ManaShield : Buff
{
    public ManaShield(int lastRound)
        :base(lastRound)
    {
        this.skillCommonName = "ManaShield";
    }
}
