using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterCard : Card
{
    protected int level;//等级
    protected int health;//生命
    protected int energy;//能量
    protected int maxHealth;//最大生命值
    protected int maxEnergy;//最大能量值
    protected int baseDamage;//攻击力
    protected int baseSpeed;//速度

    protected List<Skill> cardSkill;//卡片技能列表
    protected List<StateSkill> cardState;//卡片状态列表

    #region 构造函数
    public CharacterCard(int cardId, string cardName, List<Skill> cardSkill, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, CardType.Character, cardRarity, cardDescription)
    {
        this.cardSkill = cardSkill;
        this.cardState = new List<StateSkill>();
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
    public void SetCharacterInfo(int level, int health, int energy, int baseDamage, int baseSpeed)
    {
        this.level = level;
        this.health = health;
        this.energy = energy;
        this.maxHealth = health;
        this.maxEnergy = energy;
        this.baseDamage = baseDamage;
        this.baseSpeed = baseSpeed;
    }

    /// <summary>
    /// 当被技能指向（使用）
    /// </summary>
    /// <param name="skill">卡片被指向的技能</param>
    /// <param name="from">技能来源</param>
    public override void OnSkillUsed(Skill skill, Card from)
    {
        base.OnSkillUsed(skill, from);//调用上级

        if (skill is AttackSkill)
        {
            AttackSkill attackSkill = skill as AttackSkill;
            int damage = attackSkill.GetCalculatedDamage();
            GetDamage(damage);
        }
    }

    /// <summary>
    /// 当角色发动普通攻击时
    /// </summary>
    /// <param name="target">攻击对象</param>
    public void OnCharacterAttack(GameObject target)
    {
        CardContainer targetContainer = target.GetComponent<CardContainer>();

        //播放动画
        Hashtable args = new Hashtable();
        args.Add("amount", target.transform.position - container.transform.position);
        args.Add("time", 1f);

        iTween.PunchPosition(container.gameObject, args);

        //造成伤害
        Card card = targetContainer.GetCardData();
        if (card is CharacterCard)
        {
            CharacterCard character = card as CharacterCard;
            character.GetDamage(this.GetCardDamage());//传递卡片伤害
        }

        //状态回调
        if (cardState.Count != 0)
        {
            foreach (StateSkill state in cardState)
            {
                state.OnCharaterAttack();//向下调用事件
                if (cardState.Count == 0) { break; }
            }
        }
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    public override void UpdateCardUIBaseByCardInfo(GameObject container)
    {
        base.UpdateCardUIBaseByCardInfo(container);

        //更换图片
        GameObject character = container.transform.FindChild("Character").gameObject;
        character.GetComponent<UISprite>().spriteName = string.Format("Card-{0}", this.cardName);

        //更新等级
        Transform charaterLevel = container.transform.FindChild("CharacterLevel");
        if (charaterLevel != null)
        {
            charaterLevel.GetComponentInChildren<UILabel>().text = this.level.ToString();
        }

        //获取信息面板
        Transform info = container.transform.FindChild("CharacterInfo");
        if (info != null)
        {
            //更新血条
            UISlider healthSlider = info.FindChild("Health").GetComponent<UISlider>();
            UISlider energySlider = info.FindChild("Energy").GetComponent<UISlider>();

            healthSlider.value = (float)health / maxHealth;
            energySlider.value = (float)energy / maxEnergy;
        }


    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    public void GetDamage(int damage)
    {
        health -= damage;//伤害扣血
        UpdateCardUIBaseByCardInfo(this.container.gameObject);//更新贴图
        container.GetComponent<CardContainer>().ShakeCard();//震动卡片
        LogsSystem.Instance.Print(string.Format("{0}受到{1}点伤害,当前血量{2}", this.cardName, damage, this.health));//日志记录
    }

    /// <summary>
    /// 添加卡片状态
    /// </summary>
    public void AddState(StateSkill state)
    {
        state.SetOwnerCard(this);
        this.cardState.Add(state);

        LogsSystem.Instance.Print(
            string.Format("角色 {0} 获得状态 {1} ，持续 {2} 回合", CardNames.Instance.GetCardName(this.cardName), SkillNames.Instance.GetSkillName(state.GetSkillCommonName()), state.GetLastRound())
            );
    }

    /// <summary>
    /// 删除卡片状态
    /// </summary>
    public void RemoveState(StateSkill state)
    {
        this.cardState.Remove(state);
    }

    #region 外部访问接口
    public int GetCardLevel()
    { return this.level; }
    public List<Skill> GetCardSkillList()
    { return this.cardSkill; }
    public List<StateSkill> GetCardState()
    { return this.cardState; }
    public int GetBaseCardDamageValue()
    { return this.baseDamage; }
    public int GetBaseCardSpeedValue()
    { return this.baseSpeed; }
    public int GetCardDamage()
    {
        int value = baseDamage;
        if (cardState.Count != 0)
        {
            foreach (StateSkill state in cardState)
            {
                if (state is AttackUp)
                {
                    //如果是攻击上升类状态
                    value += ((AttackUp)state).GetAddedDamage();
                }
            }
        }
        return value;
    }
    public int GetCardSpeed()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
