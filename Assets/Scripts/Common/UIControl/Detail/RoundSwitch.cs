using UnityEngine;
using System.Collections;

public class RoundSwitch : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.transform.position = new Vector3(1280, 0, 0);
        //EnterScene();测试用 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ResetToInitValue()
    {
        transform.localPosition = new Vector3(1280, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<UISprite>().alpha = 1;
    }

    //进入场景
    public void EnterScene()
    {
        TweenPosition tp = gameObject.AddComponent<TweenPosition>();
        tp.from = new Vector3(1280, 0, 0);
        tp.to = new Vector3(0, 0, 0);
        tp.animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        tp.duration = 0.2f;
        tp.delay = 1;
        Invoke("OutScene", 2f);
    }

    public void OutScene()
    {
        TweenScale ts = gameObject.AddComponent<TweenScale>();
        ts.from = Vector3.one;
        ts.to = new Vector3(5, 5, 1);
        ts.duration = 0.5f;

        TweenAlpha ta = gameObject.AddComponent<TweenAlpha>();
        ta.from = 1;
        ta.to = 0;
        ta.duration = 0.5f;

        Invoke("ResetToInitValue", 0.5f);
    }
}
