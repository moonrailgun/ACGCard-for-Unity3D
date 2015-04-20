using UnityEngine;

/// <summary>
/// 技能基类
/// </summary>
public abstract class Skill : ISkill
{
    public string skillCommonName;
    protected static GameScene gameScene;

    protected Skill()
    {
        if (gameScene == null && Global.Instance.scene == SceneType.GameScene)
        {
            gameScene = GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<GameScene>();
        }
    }

    /*
    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="icon"></param>
    public virtual void SetIcon(Texture2D icon)
    {
        this.icon = icon;
    }*/

    /// <summary>
    /// 创建技能图标按钮
    /// </summary>
    public virtual void CreateSkillButton()
    {
        string path = "SkillList/Grid";
        GameObject grid = GameObject.Find(path);

        //创建按钮
        GameObject prefab = Resources.Load<GameObject>("SkillButton");
        GameObject button = NGUITools.AddChild(grid, prefab);
        grid.GetComponent<UIGrid>().Reposition();

        //修改按钮信息
        button.GetComponent<UIButton>().onClick.Add(new EventDelegate(OnUse));//添加回调
        button.transform.FindChild("SkillIcon").GetComponent<UISprite>().spriteName = string.Format("Skill_Icon_{0}", skillCommonName);//设置图集名
        button.transform.FindChild("Name").GetComponent<UILabel>().text = SkillNames.Instance.GetSkillName(skillCommonName);//修改技能名
    }

    public abstract void OnUse();

    public abstract void OnUse(GameObject target);
}
