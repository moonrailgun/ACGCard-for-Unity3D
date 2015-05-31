using System;
using System.Collections.Generic;

public class SkillManager
{
    #region 单例模式
    private static SkillManager _instance;
    public static SkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SkillManager();
            }
            return _instance;
        }
    }
    #endregion

    private Dictionary<int, Skill> skillList;

    public SkillManager()
    {
        Init();
        RegisterSkill();
    }

    private void Init()
    {
        this.skillList = new Dictionary<int, Skill>();
    }

    private void RegisterSkill()
    {
        //AttackSkill瞬间伤害类技能
        AddSkill(new AttackSkill("ArcaneMissiles"));
        AddSkill(new AttackSkill("Fireball"));
        AddSkill(new AttackSkill("FireArrow"));
        AddSkill(new AttackSkill("Meteorites"));
        AddSkill(new AttackSkill("Thunderbolt"));
        AddSkill(new AttackSkill("MeteoriteCut"));
        AddSkill(new AttackSkill("Yaya01"));
        AddSkill(new AttackSkill("Yaya02"));
        AddSkill(new AttackSkill("Yaya03"));
        AddSkill(new AttackSkill("Rukia01"));
        AddSkill(new AttackSkill("Rukia02"));
        AddSkill(new AttackSkill("Rukia03"));
        AddSkill(new AttackSkill("Illyasviel01"));
        AddSkill(new AttackSkill("Asuna01"));
        AddSkill(new AttackSkill("Asuna02"));
        AddSkill(new AttackSkill("Haruhi01"));
        AddSkill(new AttackSkill("Haruhi02"));
        AddSkill(new AttackSkill("Kurumi01"));
        AddSkill(new AttackSkill("Kurumi02"));
        AddSkill(new AttackSkill("Kurumi03"));
        AddSkill(new AttackSkill("Lucy01"));
        AddSkill(new AttackSkill("Lucy02"));
        AddSkill(new AttackSkill("Lucy03"));
    }

    /// <summary>
    /// 添加技能到技能列表
    /// </summary>
    private void AddSkill(Skill skill)
    {
        int availableID = skillList.Count + 1;

        AddSkill(availableID, skill);
    }
    private void AddSkill(int skillID, Skill skill)
    {
        if (!skillList.ContainsKey(skillID))
        {
            skillList.Add(skillID, skill);
        }
        else
        {
            LogsSystem.Instance.Print("已经有了该技能的ID:" + skillID + "，该ID的技能无法被注册");
        }
    }

    /// <summary>
    /// 根据技能ID获取对应技能的拷贝
    /// </summary>
    public Skill GetSkillByID(int skillID, string skillAppendData = "")
    {
        if (skillList.ContainsKey(skillID))
        {
            Skill skill = skillList[skillID].Clone() as Skill;
            skill.SetAppendData(skillAppendData);

            return skill;
        }
        LogsSystem.Instance.Print("无法通过ID获取到该技能，可能是ID不存在" + skillID);
        return null;
    }

    /// <summary>
    /// 根据技能ID数组获取技能实例列表
    /// </summary>
    public List<Skill> GetSkillListByIDArray(int[] array)
    {
        List<Skill> returnList = new List<Skill>();

        foreach (int id in array)
        {
            Skill skill = GetSkillByID(id);
            if (skill != null)
            {
                returnList.Add(skill);
            }
        }

        return returnList;
    }
}
