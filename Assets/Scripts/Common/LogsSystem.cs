using System;
using System.IO;
using UnityEngine;

public class LogsSystem
{
    #region 单例模式
    private static LogsSystem _instance;
    public static LogsSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LogsSystem();
                _instance.Print("==============开始记录==============");
            }
            return _instance;
        }
    }
    #endregion
    private string logDate;
    private string logPath;
    private string logFileName;
    private object writeFileLock;//文件读写锁

    public LogsSystem()
    {
        SetLogFileInfo();
        writeFileLock = new object();
    }

    /// <summary>
    /// 设置文件IO的信息
    /// logDate:日期
    /// logPath:文件夹地址
    /// logFileName:日志文件完整地址
    /// </summary>
    private void SetLogFileInfo()
    {
        try
        {
            logDate = DateTime.Now.ToString("yyyy-MM-dd");
            logPath = Application.dataPath + "/logs/";
            logFileName = logPath + logDate + ".log";

            //如果不存在就创建目录文件
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("发生异常:" + ex);
        }
    }


    /// <summary>
    /// 用于跨天数的日志记录更改
    /// 每次调用文件时先调用该函数检查一遍日期是否更换
    /// </summary>
    private void CheckLogFileInfo()
    {
        if (logDate != DateTime.Now.ToString("yyyy-MM-dd"))
        {
            SetLogFileInfo();//重新设置文件信息
        }
    }

    public void Print(string mainLog, LogLevel level = LogLevel.INFO)
    {
        Debug.Log(mainLog);//给unity编辑器另外传一份log

        CheckLogFileInfo();//检查是否已经更换日期了

        try
        {
            string log = string.Format("[{0} {1}] : {2}", DateTime.Now.ToString("HH:mm:ss"), level.ToString(), mainLog);

            //写入数据
            lock (writeFileLock)
            {
                //写入数据
                FileStream fs = new FileStream(logFileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(log);
                sw.Close();
                fs.Close();
            }
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}

public enum LogLevel
{
    INFO = 0,
    WARN = 1,
    ERROR = 2,
    GAMEDETAIL = 3,
    DEBUG = -1
}