using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDescriptions
{
    #region 单例模式
    private static CardDescriptions _instance;
    public static CardDescriptions Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardDescriptions();
            }
            return _instance;
        }
    }
    #endregion

    private Dictionary<string, string> CardDescriptionsList = new Dictionary<string, string>();

    private CardDescriptions()
    {
        ReadCardDescriptionList();
    }

        /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <returns></returns>
    private void ReadCardDescriptionList()
    {
        string cardNameText = Resources.Load("CardDescriptionList").ToString();
        if (cardNameText == "")
        {
            LogsSystem.Instance.Print("读取卡片描述列表出错");
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
                string Description = data[1];
                CardDescriptionsList.Add(commonName, Description);
            }
        }
        LogsSystem.Instance.Print("卡片描述列表配置读取完毕");
    }

    /// <summary>
    /// 传入英文通用名
    /// 返回描述
    /// </summary>
    /// <param name="commonName"></param>
    /// <returns></returns>
    public string GetCardDescription(string commonName)
    {
        if (CardDescriptionsList.ContainsKey(commonName))
        {
            return CardDescriptionsList[commonName];
        }
        else
        {
            return "";
        }
    }
}
