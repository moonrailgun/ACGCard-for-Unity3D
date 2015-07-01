using UnityEngine;
using System.Collections;
using System;

public class FrameAnimation : MonoBehaviour
{
    public int spriteNum = 10;
    public string format = "image {0}";
    public float intervalSecond = 0.15f;//每帧间隔时间
    public bool playAwake = false;//是否一开始就播放
    public bool loop = false;//是否循环

    private Coroutine animation;

    private void Start()
    {
        if (playAwake)
        {
            Play();
        }
    }

    //开始播放帧动画
    public void Play()
    {
        StartCoroutine("AnimationCoroutine");
    }
    public void Play(int num, string format, float intervalSecond)
    {
        this.spriteNum = num;
        this.format = format;
        this.intervalSecond = intervalSecond;
        this.Play();
    }

    /// <summary>
    /// 停止帧动画
    /// </summary>
    public void Stop()
    {
        if (this.animation != null)
        {
            StopCoroutine(this.animation);
            this.animation = null;
        }
    }

    private IEnumerator AnimationCoroutine()
    {
        UISprite sprite = GetComponent<UISprite>();
        if (sprite != null)
        {
            do
            {
                for (int i = 0; i < spriteNum; i++)
                {
                    sprite.spriteName = string.Format(format, i);
                    sprite.MakePixelPerfect();//设定为默认大小
                    yield return new WaitForSeconds(intervalSecond);
                }
            } while (this.loop);

        }
        yield return 0;
    }
}
