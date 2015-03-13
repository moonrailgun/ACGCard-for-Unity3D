using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShortMessagesSystem {
	private GameObject shortMessageRoot;
	private List<GameObject> MessageList;
	private static ShortMessagesSystem _instance;

	public ShortMessagesSystem()
	{
		MessageList = new List<GameObject>();
		CreateMessagePanel();
	}

	public static ShortMessagesSystem Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new ShortMessagesSystem();
			}
			
			return _instance;
		}
	}

	public void ShowShortMessage(string message)
	{
		GameObject label = CreateLabel(message);
		AddMessageList(label);
	}

	private void CreateMessagePanel()
	{
		if(this.shortMessageRoot == null)
		{
			LogsSystem.Instance.Print("创建小消息窗口中....");

			//anchor
			this.shortMessageRoot = new GameObject("ShortMessagePanel");
			shortMessageRoot.transform.parent = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform;
			shortMessageRoot.transform.localScale = new Vector3(1, 1, 1);
			shortMessageRoot.layer = 8;
		}
	}

	private GameObject CreateLabel(string message)
	{
		//gameobject
		GameObject label = new GameObject("ShortMessage");
		label.transform.parent = shortMessageRoot.transform;
		label.layer = 8;

		//uilabel script
		UILabel uilabel = label.AddComponent<UILabel>();
		uilabel.bitmapFont = ((GameObject)Resources.Load("Fonts/Font_yahei")).GetComponent<UIFont>();
		uilabel.text = string.Format("{0}", message);
		uilabel.pivot = UIWidget.Pivot.BottomLeft;

		//tween
		TweenAlpha ta = label.AddComponent<TweenAlpha>();
		ta.from = 1f;
		ta.to = 0f;
		ta.method = UITweener.Method.EaseInOut;
		ta.enabled = false; 

		label.AddComponent<ShortMessageControl>();

		return label;
	}

	private void AddMessageList(GameObject label)
	{
		float x = - Screen.width / 2;
		float y = - Screen.height / 2;
		float offsetX = 5;
		float offsetY = 10;

		if(MessageList.Count != 0)
		{
			foreach(GameObject go in MessageList)
			{
				float addedY = go.GetComponent<UILabel>().fontSize;
				float goY = go.transform.localPosition.y;
				go.transform.localPosition = new Vector3( go.transform.localPosition.x, goY + addedY, go.transform.localPosition.z);
			}
		}
		MessageList.Add(label);
		label.transform.localPosition = new Vector3(x + offsetX, y + offsetY, 0f);
	}

	public void RemoveMessageList(GameObject go)
	{
		MessageList.Remove(go);
	}
}
