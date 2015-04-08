using UnityEngine;
using System.Collections;

public class GameStartButton : MonoBehaviour
{

    private TweenScale ts;

    private void Awake()
    {
        ts = GetComponent<TweenScale>();
        UIEventListener.Get(gameObject).onHover += ChangeScale;
    }

    private void ChangeScale(GameObject go, bool state)
    {
        bool isHover = state;
        if (isHover == true)
        {
            ts.PlayForward();
        }
        else
        {
            ts.PlayReverse();
        }
    }
}
