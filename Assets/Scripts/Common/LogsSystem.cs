using UnityEngine;
using System;
using System.IO;

public class LogsSystem
{
	private static LogsSystem _instance;
	private string logPath;
	private string logFileName;

	public LogsSystem()
	{
		string logDate = DateTime.Now.ToString("yyyy-MM-dd");
		logPath = Application.dataPath + "/logs/";

		logFileName = logPath + logDate + ".log";
	}

	public static LogsSystem Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new LogsSystem();
			}

			return _instance;
		}
	}

	public void Print(string mainLog,LogLevel level = LogLevel.INFO)
	{
		if(Application.isEditor) {Debug.Log(mainLog);}//给unity编辑器另外传一份log
		//ShortMessagesSystem.Instance.ShowShortMessage(mainLog);//给用户交互界面发送一份

		try
		{
			if(!Directory.Exists(logPath))
			{
				Directory.CreateDirectory(logPath);
			}
			if(!File.Exists(logFileName))
			{
				File.Create(logFileName);
			}
			StreamWriter sw = new StreamWriter(logFileName, true);

			sw.WriteLine(string.Format("[{0} {1}]:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), level.ToString(),mainLog));

			sw.Close();
		}
		catch(IOException ex)
		{
			Debug.Log(ex.ToString());
		}

	}
}

public enum LogLevel
{
	INFO = 0,
	WARN = 1,
	ERROR = 2
}