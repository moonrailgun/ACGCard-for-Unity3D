using UnityEngine;
using System.Collections;

public class CharacterCard : Card
{
    private int health;//生命
    private int energy;//能量

    public int GetHealth()
    {
        return this.health;
    }
    public int GetEnergy()
    {
        return this.energy;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Init()
    {
        base.Init();
    }
}
