using System;
public class CommonDTO
{
    public string timestamp;

    public CommonDTO()
    {
        timestamp = GetTimeStamp().ToString();
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

