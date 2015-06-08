using UnityEngine;
using System;

/// <summary>
/// 技能基类
/// </summary>
public abstract class Skill : ISkill , ICloneable
{
    protected int skillID;
    protected string skillCommonName;
    protected string skillIconName;
    protected int consumedEnergy = -1;//消耗的能量
    protected string skillAppendData;//技能附加数据，可以为空
    protected CharacterCard skillOwner;//技能拥有者

    protected GameObject skillButtonObject;//技能在游戏中的按钮实例

    public Skill(int skillID, string skillCommonName, bool haveIcon = false, string specialIconName = "")
    {
        this.skillID = skillID;
        this.skillCommonName = skillCommonName;

        if (haveIcon)
        {
            if (string.IsNullOrEmpty(specialIconName))
            { SetIconName(skillCommonName); }
            else
            { SetIconName(specialIconName); }
        }
        else
        { SetIconName("Unknown"); }
    }

    /// <summary>
    /// 场景管理器
    /// </summary>
    protected GameScene gameScene
    {
        get
        {
            if (Global.Instance.scene == SceneType.GameScene)
            {
                return GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<GameScene>();
            }
            else
            {
                LogsSystem.Instance.Print("无法获得场景管理器");
                return null;
            }
        }
    }

    /// <summary>
    /// 检查施法条件
    /// 如果不合法则返回false
    /// </summary>
    /// <param name="from">施法对象</param>
    protected virtual bool CheckConjureCondition(GameObject from)
    {
        //消耗能量
        if (this.consumedEnergy >= 0)
        {
            Card card = from.GetComponent<CardContainer>().GetCardData();
            if (card is CharacterCard)
            {
                CharacterCard character = card as CharacterCard;
                if (!character.TryConsumeEnergy(this.consumedEnergy))
                {
                    //能量不足
                    ShortMessagesSystem.Instance.ShowShortMessage("能量不足以放出技能");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("没有配置消耗能量");
        }
        return false;
    }

    /// <summary>
    /// 创建技能图标按钮
    /// </summary>
    public virtual void CreateSkillButton(string path)
    {
        GameObject grid = GameObject.Find(path);

        //创建按钮
        GameObject prefab = Resources.Load<GameObject>("SkillButton");
        GameObject button = NGUITools.AddChild(grid, prefab);
        grid.GetComponent<UIGrid>().Reposition();//更新坐标

        this.skillButtonObject = button;//对象指向赋值

        //修改按钮信息
        button.GetComponent<UIButton>().onClick.Add(new EventDelegate(SetSelectedSkill));//添加回调
        //设置精灵名
        if (string.IsNullOrEmpty(skillIconName) && !string.IsNullOrEmpty(skillCommonName))
        { button.transform.FindChild("SkillIcon").GetComponent<UISprite>().spriteName = string.Format("Skill_Icon_{0}", skillCommonName); }
        else
        { button.transform.FindChild("SkillIcon").GetComponent<UISprite>().spriteName = skillIconName; }
        button.transform.FindChild("Name").GetComponent<UILabel>().text = SkillNames.Instance.GetSkillName(skillCommonName);//修改技能名
    }

    /// <summary>
    /// 设置技能名
    /// </summary>
    public Skill SetIconName(string name)
    {
        this.skillIconName = string.Format("Skill_Icon_{0}", name);
        return this;
    }

    public void SetAppendData(string skillAppendData)
    {
        this.skillAppendData = skillAppendData;
    }

    public void SetSkillID(int skillID)
    {
        this.skillID = skillID;
    }

    /// <summary>
    /// 获取技能显示的名字
    /// </summary>
    public virtual string GetSkillShowName()
    {
        return SkillNames.Instance.GetSkillName(this.skillCommonName);
    }

    /// <summary>
    /// 获取技能ID
    /// </summary>
    public int GetSkillID()
    { return this.skillID; }

    /// <summary>
    /// 获取技能通用名
    /// </summary>
    /// <returns></returns>
    public string GetSkillCommonName()
    {
        return this.skillCommonName;
    }

    public virtual string GetSkillAppendData()
    {
        return this.skillAppendData;
    }

    /// <summary>
    /// 获取实例化的按钮
    /// </summary>
    public GameObject GetButtonObject()
    {
        return this.skillButtonObject;
    }

    /*
    public abstract void OnUse();//被点击
    public abstract void OnUse(GameObject target);
    public abstract void OnUse(GameObject from, GameObject target);// 被点击后有施法目标
    */

    /// <summary>
    /// 设置当前选中的技能为该技能
    /// </summary>
    protected void SetSelectedSkill()
    {
        gameScene.SetSelectedSkill(this);
    }

    /// <summary>
    /// 只能被服务器返回的响应调用
    /// 技能来源必为技能拥有者（skillOwner)
    /// toCard为施法对象可以为null,
    /// skillAppendData为附加数据（伤害等，格式为JSON）
    /// </summary>
    public abstract void OnUse(CharacterCard toCard, string skillAppendData);

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}