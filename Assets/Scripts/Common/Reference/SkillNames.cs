using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillNames
{
    #region 单例模式
    private static SkillNames _instance;
    public static SkillNames Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SkillNames();
            }
            return _instance;
        }
    }
    #endregion

    public Dictionary<string, string> SkillNamesList = new Dictionary<string, string>();

    private SkillNames()
    {
        ReadSkillNameList();
    }

    /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <returns></returns>
    private void ReadSkillNameList()
    {
        string cardNameText = Resources.Load("SkillNameList").ToString();
        if (cardNameText == "")
        {
            LogsSystem.Instance.Print("读取卡片名称列表出错");
            return;
        }
        string[] lists = cardNameText.Split(new char[] { '\n' });
        foreach (string list in lists)
        {
            string text = list.Replace("\r", "");
            //如果以#开头则跳过（注释）
            if (text.StartsWith("#"))
            {
                continue;
            }

            string[] data = text.Split(new char[] { '=' });

            //只添加正常的数据
            if (data.Length == 2)
            {
                string commonName = data[0];
                string chineseName = data[1];
                SkillNamesList.Add(commonName, chineseName);
            }
        }
        LogsSystem.Instance.Print(string.Format("技能名称列表配置读取完毕,共读取名称{0}个", SkillNamesList.Count));
    }

    /// <summary>
    /// 传入英文通用名
    /// 返回中文名
    /// </summary>
    public string GetSkillName(string commonName)
    {
        if (SkillNamesList.ContainsKey(commonName))
        {
            return SkillNamesList[commonName];
        }
        else
        {
            return "";
        }
    }
}
