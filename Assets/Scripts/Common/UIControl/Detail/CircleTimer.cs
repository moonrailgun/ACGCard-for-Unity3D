using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CircleTimer : MonoBehaviour
{
    public UISprite timerBackground;
    public UILabel numLabel;
    public bool isInfinite = false;
    public float intervalTime = 1f;//一次循环间隔时间
    public int beginTime = 60;//开始时间
    private int showTime;
    private Coroutine TimerCoroutine;

    private void Awake()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    public void StartTimer()
    {
        if (TimerCoroutine == null)
        {
            TimerInit();//初始化设定
            TimerCoroutine = StartCoroutine(Timer(intervalTime));

            //显示动画
            UITweener tweener = GetComponent<UITweener>();
            if (tweener != null)
            {
                tweener.PlayForward();
            }
        }
    }

    /// <summary>
    /// 结束计时
    /// </summary>
    public void StopTimer()
    {
        if (TimerCoroutine != null)
        {
            StopCoroutine(TimerCoroutine);
        }
    }

    private int addDir = 1;
    IEnumerator Timer(float intervalTime)
    {
        if (timerBackground == null || numLabel == null) { yield return 0; }

        while (true)
        {
            float _fillAmount = timerBackground.fillAmount;//当前值
            float addedAmount = addDir * intervalTime * Time.fixedDeltaTime;//附加值
            float toChangeAmount = _fillAmount + addedAmount;//将要变化的值

            //不在正常范围内，调整参数
            if (!(toChangeAmount < 1 && toChangeAmount > 0))
            {
                if (toChangeAmount <= 0)
                {
                    toChangeAmount = -toChangeAmount;
                }
                else if (toChangeAmount >= 1)
                {
                    toChangeAmount = 2 - toChangeAmount;
                }
                timerBackground.invert = !timerBackground.invert;//取反
                addDir = -addDir;//取反

                if (!isInfinite)
                    TimeDown();//时间变化
            }

            timerBackground.fillAmount = toChangeAmount;
            yield return new WaitForFixedUpdate();
        }

    }

    private void TimeDown()
    {
        showTime -= 1;
        numLabel.text = showTime.ToString();

        if (showTime <= 0)
        {
            LogsSystem.Instance.Print("时间终了");
            Finished();
            StopTimer();
        }
    }

    private void Finished()
    {
        //完成后参数设置，强制中断后续背景变化操作
        if (beginTime % 2 == 0)
        {
            timerBackground.alpha = 0;
        }
        else
        {
            timerBackground.type = UIBasicSprite.Type.Simple;
        }

        //完成动画
        UITweener tweener = GetComponent<UITweener>();
        if (tweener != null)
        {
            tweener.PlayReverse();
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void TimerInit()
    {
        if (timerBackground != null && numLabel != null)
        {
            //this
            this.transform.localScale = new Vector3(1, 1, 1);

            //timerBackground
            timerBackground.alpha = 1.0f;
            timerBackground.type = UIBasicSprite.Type.Filled;
            timerBackground.flip = UIBasicSprite.Flip.Nothing;
            timerBackground.fillDirection = UIBasicSprite.FillDirection.Radial360;
            timerBackground.fillAmount = 0f;
            timerBackground.invert = false;

            //numLabel
            showTime = beginTime;
            numLabel.text = showTime.ToString();

            if (!isInfinite)
                numLabel.gameObject.SetActive(true);
            else
                numLabel.gameObject.SetActive(false);
        }
        else
        {
            LogsSystem.Instance.Print("计时器初始化失败，无法取得对象", LogLevel.WARN);
        }
    }
}
