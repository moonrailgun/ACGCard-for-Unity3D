using UnityEngine;
using System.Collections;

public class ShortMessageControl : MonoBehaviour {
	private float animationStartTime = 3f;

	// Use this for initialization
	void Start () {
		Invoke("ShowAnimation", animationStartTime);
	}

	void ShowAnimation()
	{
		TweenAlpha ta = gameObject.GetComponent<TweenAlpha>();
		ta.AddOnFinished(DestoryGameobject);
		ta.PlayForward();
	}

	void DestoryGameobject()
	{
		Destroy(gameObject);
		ShortMessagesSystem.Instance.RemoveMessageList(gameObject);
	}
}
