using UnityEngine;
using System.Collections;

public class SkillAnimation : MonoBehaviour
{
    public int spriteNum = 10;
    public string format = "image {0}";
    public float intervalSecond = 0.5f;//每帧间隔时间

    private void Start()
    {
        Play(true);
    }
    public void Play(bool loop = false)
    {
        StartCoroutine("AnimationCoroutine",loop);
    }
    public void Play(int num, string format, float intervalSecond, bool loop = false)
    {
        this.spriteNum = num;
        this.format = format;
        this.intervalSecond = intervalSecond;
        this.Play(loop);
    }

    private IEnumerator AnimationCoroutine(bool loop)
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
            } while (loop);
            
        }
        yield return 0;
    }
}
