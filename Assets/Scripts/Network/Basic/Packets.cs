using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class Packets
{
    public static byte[] PingPacket()
    {
        string message = "";
        AddArguments(ref message, GetTimeStamp().ToString());
        AddArguments(ref message, "ping");
        AddArguments(ref message, "true");
        return Encoding.UTF8.GetBytes(message);
    }

    public static byte[] LoginPacket(string username, string password)
    {
        string message = "";
        AddArguments(ref message, GetTimeStamp().ToString());
        AddArguments(ref message, "login");
        AddArguments(ref message, "true");
        AddArguments(ref message, username);
        AddArguments(ref message, password);
        return Encoding.UTF8.GetBytes(message);
    }

    /// <summary>
    /// 构造聊天包
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="toUUID">私聊时发送的对象</param>
    public static byte[] ChatPacket(string text, string toUUID = "")
    {
        string message = "";
        AddArguments(ref message, GetTimeStamp().ToString());
        AddArguments(ref message, "chat");
        AddArguments(ref message, "true");
        AddArguments(ref message, text);
        AddArguments(ref message, toUUID);
        return Encoding.UTF8.GetBytes(message);
    }

    /// <summary>
    /// 添加参数
    /// </summary>
    /// <param name="addedString">被添加参数的字符串</param>
    /// <param name="arguments">添加的参数</param>
    public static void AddArguments(ref string addedString, string argument)
    {
        if (addedString == "")
        {
            addedString = argument;
        }
        else
        {
            addedString += " " + argument;
        }

    }

    /// <summary>
    /// 获得时间戳（格林威治时间）
    /// 精确到ms，有±1ms的误差
    /// </summary>
    /// <returns>时间戳字符串</returns>
    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds * 1000);
    }
}
