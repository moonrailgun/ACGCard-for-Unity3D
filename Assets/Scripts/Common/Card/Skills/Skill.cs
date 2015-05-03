using UnityEngine;

/// <summary>
/// 技能基类
/// </summary>
public abstract class Skill : ISkill
{
    protected string skillCommonName;
    protected static GameScene gameScene;
    protected string skillIconName;

    protected GameObject skillButtonObject;//技能在游戏中的按钮实例

    protected Skill()
    {
        if (gameScene == null && Global.Instance.scene == SceneType.GameScene)
        {
            gameScene = GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<GameScene>();
        }
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
        button.GetComponent<UIButton>().onClick.Add(new EventDelegate(OnUse));//添加回调
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

    /// <summary>
    /// 获取技能显示的名字
    /// </summary>
    public virtual string GetSkillShowName()
    {
        return SkillNames.Instance.GetSkillName(this.skillCommonName);
    }

    /// <summary>
    /// 获取技能通用名
    /// </summary>
    /// <returns></returns>
    public string GetSkillCommonName()
    {
        return this.skillCommonName;
    }

    /// <summary>
    /// 获取实例化的按钮
    /// </summary>
    public GameObject GetButtonObject()
    {
        return this.skillButtonObject;
    }

    public abstract void OnUse();
    public abstract void OnUse(GameObject target);
    public abstract void OnUse(GameObject from, GameObject target);
}
