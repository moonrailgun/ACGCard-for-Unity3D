using UnityEngine;
using System.Collections;

public class ServerLink : MonoBehaviour {
	public GameObject serverButton;
	private CardClient client;
	private GameObject serverList;
	private bool haveRead = false;

	void Awake()
	{
		client = GameObject.FindGameObjectWithTag(Tags.Networks).GetComponent<CardClient>();
	}

	/// <summary>
	/// 从本地文件读取服务器列表.
	/// </summary>
	public void ReadServerList()
	{
		if(haveRead) {return;}

		serverList = GameObject.Find("ServerList");
		if(serverList == null) { LogsSystem.Instance.Print("服务器列表容器不存在"); return; }

		Object serverListResource = Resources.Load("ServerList");
		if(serverListResource == null) 
		{
			LogsSystem.Instance.Print("读取服务器列表出错");
			return;
		}
		string txt = serverListResource.ToString();
		string[] lists = txt.Split(new char[]{'\n'});
		foreach(string list in lists)
		{
			string[] serverInfo = list.Split(new char[]{','});
			CreateServerCell(serverInfo[2],serverInfo[0],serverInfo[1]);
		}

		serverList.GetComponent<UIGrid>().enabled = true;
		haveRead = true;
	}

	/// <summary>
	/// 创建服务器登陆按钮.
	/// </summary>
	/// <param name="serverName">服务器名.</param>
	/// <param name="IPAddress">IP地址.</param>
	/// <param name="port">端口号,默认5055.</param>
	private void CreateServerCell(string serverName, string IPAddress, string port = "5055")
	{
		if(serverButton == null){Debug.Log("按钮不存在");return;}
		if(serverList == null){ Debug.Log("列表不存在"); return; }

		GameObject go = (GameObject)Instantiate(serverButton);
		go.transform.parent = serverList.transform;
		go.transform.localScale = new Vector3(1,1,1);
		go.GetComponentInChildren<UILabel>().text = serverName;
		go.name = IPAddress + ":" + port;

		UIEventListener.Get(go).onClick += OnLink;
	}

	/// <summary>
	/// 当点击服务器按钮后调用该方法.
	/// </summary>
	/// <param name="go">监听的游戏对象.</param>
	void OnLink(GameObject go)
	{
		string serverAddress = go.name;
		string serverName = go.GetComponentInChildren<UILabel>().text;
		ShortMessagesSystem.Instance.ShowShortMessage("正在登陆服务器：" + serverName);
		Global.Instance.serverName = serverName;

		client.SetHost(serverAddress);
	}
}
