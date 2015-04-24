using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterCard : Card
{
    private int health;//生命
    private int energy;//能量

    #region 构造函数
    public CharacterCard()
        : base()
    {
    }
    public CharacterCard(int cardId, string cardName, int cardRarity)
        : base(cardId, cardName, cardRarity)
    {
    }
    public CharacterCard(int cardId, string cardName, int cardRarity, CardType type)
        : base(cardId, cardName, cardRarity, type)
    {
    }
    public CharacterCard(int cardId, string cardName, CardType cardType, List<Skill> cardSkill, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, cardType, cardSkill, cardRarity, cardDescription)
    {
    }
    #endregion

    public int GetHealth()
    {
        return this.health;
    }
    public int GetEnergy()
    {
        return this.energy;
    }
    public void SetCharacterInfo(int health, int energy)
    {
        this.health = health;
        this.energy = energy;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Init()
    {
        base.Init();
    }

    public override void OnSkillUsed(Skill skill)
    {
        base.OnSkillUsed(skill);//调用上级

        if (skill is AttackSkill)
        {
            AttackSkill attackSkill = skill as AttackSkill;
            int damage = attackSkill.damage;
            health -= damage;
            LogsSystem.Instance.Print(string.Format("{0}收到{1}点伤害,当前血量{2}", this.cardName, damage, this.health));
        }
    }
}
