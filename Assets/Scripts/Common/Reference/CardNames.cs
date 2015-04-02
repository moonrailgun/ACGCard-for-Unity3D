using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardNames
{
    #region 单例模式
    private static CardNames _instance;
    public static CardNames Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardNames();
            }
            return _instance;
        }
    }
    #endregion

    public Dictionary<string, string> CardNamesList = new Dictionary<string, string>();

    private CardNames()
    {
        ReadCardNameList();
    }

    /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <returns></returns>
    private void ReadCardNameList()
    {
        //Resources.Load("CardNameList")
        string cardNameText = Resources.Load("CardNameList").ToString();
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
                CardNamesList.Add(commonName, chineseName);
            }
        }
        LogsSystem.Instance.Print("卡片名称列表配置读取完毕");
    }

    /// <summary>
    /// 传入英文通用名
    /// 返回中文名
    /// </summary>
    /// <param name="commonName"></param>
    /// <returns></returns>
    public string GetCardName(string commonName)
    {
        if (CardNamesList.ContainsKey(commonName))
        {
            return CardNamesList[commonName];
        }
        else
        {
            return "";
        }
    }
}
