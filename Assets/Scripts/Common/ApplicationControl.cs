using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationControl : MonoBehaviour
{
    /// <summary>
    /// 当应用退出时
    /// </summary>
    public void OnApplicationQuit()
    {
        LogsSystem.Instance.Print("正在退出游戏...");
        GetComponent<CardClient>().StopListen();//关闭UDP监听
    }
}
