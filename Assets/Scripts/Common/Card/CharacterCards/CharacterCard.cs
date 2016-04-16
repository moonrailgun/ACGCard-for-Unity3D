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
    protected int attack;//攻击力
    protected int speed;//速度

    protected bool isAvailabel;//是否可用

    protected Equipment equipments;//装备

    protected List<Skill> cardOwnSkill;//卡片技能列表
    protected Dictionary<StateSkill, Card> cardState;//卡片状态列表 - <状态内容,状态来源>

    #region 构造函数
    public CharacterCard(int cardId, string cardName, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, CardType.Character, cardRarity, cardDescription)
    {
        this.cardState = new Dictionary<StateSkill, Card>();
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
    public void SetCharacterInfo(int level, int health, int energy, int attack, int speed, List<Skill> cardOwnSkill)
    {
        this.level = level;
        this.health = health;
        this.energy = energy;
        this.maxHealth = health;
        this.maxEnergy = energy;
        this.attack = attack;
        this.speed = speed;
        this.cardOwnSkill = cardOwnSkill;
    }

    /// <summary>
    /// 尝试消耗能量
    /// 如果能量不足则返回false
    /// </summary>
    public bool TryConsumeEnergy(int consumeEnergyValue)
    {
        if (energy >= consumeEnergyValue)
        {
            //能量足够
            energy -= consumeEnergyValue;
            return true;
        }
        else
        {
            //能量不足
            return false;
        }
    }

    /// <summary>
    /// 当被技能指向（使用）
    /// </summary>
    /// <param name="skill">卡片被指向的技能</param>
    /// <param name="from">技能来源</param>
    public override void OnSkillUsed(Skill skill, Card from)
    {
        if (!this.isAvailabel)
        {
            LogsSystem.Instance.Print("卡片不可用");
        }

        base.OnSkillUsed(skill, from);//调用上级

        if (skill is AttackSkill)
        {
            AttackSkill attackSkill = skill as AttackSkill;
            int damage = attackSkill.GetCalculatedDamage();
            GetDamage(damage);
        }

        this.SetAvailable(false);//设置卡片不可用
    }

    /// <summary>
    /// 当角色发动普通攻击时
    /// </summary>
    /// <param name="targetCharacter">攻击对象</param>
    /// <param name="damage">伤害值</param>
    public void OnCharacterAttack(CharacterCard targetCharacter, int damage)
    {
        if (!this.isAvailabel)
        {
            LogsSystem.Instance.Print("卡片不可用");
        }

        GameObject target = targetCharacter.container.gameObject;
        //播放动画
        Hashtable args = new Hashtable();
        args.Add("amount", target.transform.position - container.transform.position);
        args.Add("time", 1f);
        iTween.PunchPosition(container.gameObject, args);

        targetCharacter.GetDamage(damage);
        this.SetAvailable(false);//设置卡片不可用

        //状态回调
        if (cardState.Count != 0)
        {
            foreach (KeyValuePair<StateSkill, Card> pair in cardState)
            {
                pair.Key.OnCharacterAttack();//向下调用事件
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
        container.ShakeCard();//震动卡片
        LogsSystem.Instance.Print(string.Format("{0}受到{1}点伤害,当前血量{2}", this.cardName, damage, this.health));//日志记录

        //死亡
        if (health <= 0)
        {
            OnDead();
        }
    }

    /// <summary>
    /// 尝试消耗能量值
    /// 无需验证（在服务端已通过）
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public void ExpendEnergy(int value)
    {
        if (this.energy >= value)
        {
            this.energy -= value;
        }
        else
        {
            this.energy = 0;
            LogsSystem.Instance.Print("出现异常，尝试扣除比大于人物拥有能量的能量，可能是由于服务端和客户端数据不同步造成的。请联系开发人员", LogLevel.WARN);
        }

    }

    /// <summary>
    /// 死亡
    /// </summary>
    protected void OnDead()
    {
        //立即销毁卡片
        LogsSystem.Instance.Print(string.Format("{0}生命值归零，死亡。立即销毁卡片", this.cardName));
        Object.DestroyImmediate(container.gameObject);
    }

    /// <summary>
    /// 添加卡片状态
    /// </summary>
    public void AddState(StateSkill state, Card from)
    {
        state.SetOwnerCard(this);//设置状态的拥有者
        this.cardState.Add(state, from);//状态，来源

        LogsSystem.Instance.Print(
            string.Format("角色 {0} 获得状态 {1} ，持续 {2} 回合(来自:{3})", CardNames.Instance.GetCardName(this.cardName), SkillNames.Instance.GetSkillName(state.GetSkillCommonName()), state.GetLastRound(), from.GetCardName())
            );
    }

    /// <summary>
    /// 删除卡片状态
    /// </summary>
    public void RemoveState(StateSkill state)
    {
        if (this.cardState.ContainsKey(state))
        {
            this.cardState.Remove(state);
        }
    }

    /// <summary>
    /// 根据技能ID获取状态
    /// </summary>
    public StateSkill GetStateById(int skillID)
    {
        foreach (KeyValuePair<StateSkill,Card> pair in this.cardState)
        {
            StateSkill state = pair.Key;

            if (state.GetSkillID() == skillID)
            {
                return state;
            }
        }

        LogsSystem.Instance.Print(string.Format("角色{0}没有该ID为{1}的技能", this.cardName, skillID), LogLevel.WARN);
        return null;
    }

    /// <summary>
    /// 根据技能ID获取卡片技能
    /// </summary>
    public Skill GetSkillByID(int skillID)
    {
        foreach (Skill skill in this.cardOwnSkill)
        {
            if (skill.GetSkillID() == skillID)
            {
                return skill;
            }
        }

        LogsSystem.Instance.Print(string.Format("角色{0}没有该ID为{1}的技能", this.cardName, skillID), LogLevel.WARN);
        return null;
    }

    public override void SetCardInfo(CardInfo info)
    {
        base.SetCardInfo(info);

        SetCharacterInfo(info.cardLevel, info.health, info.energy, info.attack, info.speed, SkillManager.Instance.GetSkillListByIDArray(IntArray.StringToIntArray(info.cardOwnSkill)));
    }

    /// <summary>
    /// 获取技能来源
    /// </summary>
    public Card GetStateOrigin(StateSkill state)
    {
        return this.cardState[state];
    }

    #region 装备道具
    /// <summary>
    /// 装备武器
    /// </summary>
    public void RequestEquipWeapon(Weapon weapon)
    {
        //请求网络
        GameClient.Instance.GetGameSceneManager().gameManager.RequestEquipment(weapon, 1);
    }
    /// <summary>
    /// 装备盔甲
    /// </summary>
    public void RequestEquipArmor(EquipmentCard armor)
    {
        //请求网络
        GameClient.Instance.GetGameSceneManager().gameManager.RequestEquipment(armor, 2);
    }
    /// <summary>
    /// 装备首饰
    /// </summary>
    public void RequestEquipJewelry(Jewelry jewelry)
    {
        //请求网络
        GameClient.Instance.GetGameSceneManager().gameManager.RequestEquipment(jewelry, 3);
    }

    #endregion

    #region 响应修改
    public void SetEquipment(EquipmentCard equipmentCard, int position)
    {
        //Weapon = 1, Armor = 2, Jewelry1 = 3,Jewelry2 = 4
        switch (position)
        {
            case 1:
                {
                    this.equipments.weapon = equipmentCard;
                    break;
                }
            case 2:
                {
                    this.equipments.armor = equipmentCard;
                    break;
                }
            case 3:
                {
                    this.equipments.jewelry1 = equipmentCard;
                    break;
                }
            case 4:
                {
                    this.equipments.jewelry2 = equipmentCard;
                    break;
                }
            default:
                {
                    LogsSystem.Instance.Print("未知的装备位置:" + position, LogLevel.WARN);
                    break;
                }
        }
    }
    #endregion

    #region 外部访问接口
    public int GetCardLevel()
    { return this.level; }
    public List<Skill> GetCardSkillList()
    { return this.cardOwnSkill; }
    public Dictionary<StateSkill, Card> GetCardState()
    { return this.cardState; }
    public int GetBaseCardDamageValue()
    { return this.attack; }
    public int GetBaseCardSpeedValue()
    { return this.speed; }
    public int GetCardSpeed()
    {
        throw new System.NotImplementedException();
    }
    public Equipment GetCharacterEquipments()
    { return this.equipments; }

    public override CardInfo GetCardInfo()
    {
        CardInfo info = new CardInfo();

        info.cardUUID = this.cardUUID;
        info.cardId = this.cardID;
        //info.cardOwnerId 暂时保留
        info.cardName = this.cardName;
        info.cardRarity = (int)this.cardRarity;
        info.cardLevel = this.level;
        info.health = this.health;
        info.energy = this.energy;
        info.attack = this.attack;
        info.speed = this.speed;

        //获取技能
        int[] array = SkillManager.Instance.GetSkillArrayByList(this.cardOwnSkill);
        info.cardOwnSkill = IntArray.IntArrayToString(array);

        return info;
    }

    public void SetAvailable(bool available)
    {
        this.isAvailabel = available;
        if (available)
        {
            //变正常
            Transform cardObject = this.container.transform;
            UISprite sprite = cardObject.FindChild("Character").GetComponent<UISprite>();
            if (sprite != null)
            {
                sprite.color = Color.white;
            }
        }
        else
        {
            //变黑
            Transform cardObject = this.container.transform;
            UISprite sprite = cardObject.FindChild("Character").GetComponent<UISprite>();
            if (sprite != null)
            {
                sprite.color = Color.black;
            }
        }
    }
    public bool GetAvailable()
    {
        return this.isAvailabel;
    }
    #endregion

    public struct Equipment
    {
        public EquipmentCard weapon;//武器
        public EquipmentCard armor;//盔甲
        public EquipmentCard jewelry1;//首饰1
        public EquipmentCard jewelry2;//首饰2
    }
}
