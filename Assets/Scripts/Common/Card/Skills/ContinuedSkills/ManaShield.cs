using System;
using System.Collections.Generic;

public class ManaShield : Buff
{
    public ManaShield(int lastRound)
        :base(lastRound)
    {
        this.skillCommonName = "ManaShield";
    }

    public override void OnUse(CharacterCard toCard, string skillAppendData)
    {
        throw new NotImplementedException();
    }

    public override void AnalyzeSkillAppendData()
    {
        throw new NotImplementedException();
    }
}
