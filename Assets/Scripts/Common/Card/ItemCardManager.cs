using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;


public class ItemCardManager
{
    #region 单例模式
    private static ItemCardManager _instance;
    public static ItemCardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemCardManager();
            }
            return _instance;
        }
    }
    #endregion

    public static string itemResourceJSONPath = "Item";

    public void ReadItemJsonDate()
    {
        string itemJSONString = Resources.Load(itemResourceJSONPath).ToString();

        JsonData jd = JsonMapper.ToObject(itemJSONString);
        if (jd.IsArray)
        {
            foreach (JsonData data in jd)
            {
                int ID = (int)data["ID"];
                string Name = (string)data["Name"];
                string Type = (string)data["Type"];
                int Rarity = (int)data["Rarity"];
                string Des = (string)data["Des"];
                string Pic = (string)data["Pic"];
                JsonData Action = (JsonData)data["Action"];
                ReadAction(Action);
            }
        }
        else
        {
            LogsSystem.Instance.Print("JSON数据不合法，不是预料中的格式", LogLevel.WARN);
        }
    }

    private void ReadAction(JsonData action)
    {
        throw new System.NotImplementedException();
    }
}
