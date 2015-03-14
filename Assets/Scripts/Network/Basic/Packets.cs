using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class Packets{
	public static byte[] PingPacket()
	{
		string text = "";
		AddArguments(ref text, GetTimeStamp().ToString());
		AddArguments(ref text, "ping");
		AddArguments(ref text, "true");
		return Encoding.UTF8.GetBytes(text);
	}

	/// <summary>
	/// 添加参数
	/// </summary>
	/// <param name="addedString">被添加参数的字符串</param>
	/// <param name="arguments">添加的参数</param>
	public static void AddArguments(ref string addedString, string argument)
	{
		addedString += " " + argument;
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
