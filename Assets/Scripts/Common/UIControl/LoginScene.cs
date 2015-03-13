using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginScene : MonoBehaviour {
	[SerializeField]
	private GameObject indexPanel;
	[SerializeField]
	private GameObject serverSelectPanel;
	[SerializeField]
	private GameObject loginPanel;

	void Start () {
		indexPanel.GetComponent<TweenPosition>().to.y = - Screen.height / 2 - 50;

		if(indexPanel == null || serverSelectPanel == null || loginPanel == null)
		{
			Debug.LogError("没有给面板赋值");
		}
		else
		{
			indexPanel.SetActive(true);
			serverSelectPanel.SetActive(false);
			loginPanel.SetActive(false);
		}
	}

	public void EnterGame()
	{
		if(indexPanel != null && serverSelectPanel != null)
		{
			serverSelectPanel.SetActive(true);

			//获取服务器列表
			gameObject.GetComponent<ServerLink>().ReadServerList();

			indexPanel.GetComponent<TweenAlpha>().PlayForward();
			indexPanel.GetComponent<TweenPosition>().PlayForward();
			serverSelectPanel.GetComponent<TweenAlpha>().PlayForward();
		}
	}

	public void BackFromServerList()
	{
		if(indexPanel != null && serverSelectPanel != null)
		{
			serverSelectPanel.SetActive(false);

			indexPanel.GetComponent<TweenAlpha>().PlayReverse();
			indexPanel.GetComponent<TweenPosition>().PlayReverse();
			serverSelectPanel.GetComponent<TweenAlpha>().PlayReverse();
		}
	}

	public void NoContent()
	{
		ShortMessagesSystem.Instance.ShowShortMessage("暂无内容,敬请期待!");
	}

	public void ConnectSuccess()
	{
		if(serverSelectPanel != null && loginPanel != null)
		{
			serverSelectPanel.SetActive(false);
			loginPanel.SetActive(true);

			loginPanel.GetComponent<TweenAlpha>().PlayForward();
		}
	}

	public void BackToServerSelectFromLoginPanel()
	{
		if(serverSelectPanel != null && loginPanel != null)
		{
			serverSelectPanel.SetActive(true);
			loginPanel.SetActive(false);

			loginPanel.GetComponent<TweenAlpha>().PlayReverse();
		}
	}

	/// <summary>
	/// 登陆.
	/// </summary>
	public void OnLogin()
	{
		string account = GameObject.Find("LoginPanel/background/Container/Account").GetComponentInChildren<UIInput>().label.text;
		string password = GameObject.Find("LoginPanel/background/Container/Password").GetComponentInChildren<UIInput>().label.text;

		if(string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
		{
			ShortMessagesSystem.Instance.ShowShortMessage("请输入账号或者密码");
		}
		else
		{
			//登陆请求
			ShortMessagesSystem.Instance.ShowShortMessage("正在发送登陆请求...请稍后");







		}
	}
}
