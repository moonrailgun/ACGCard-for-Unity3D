using UnityEngine;
using System.Collections;

public class VideoManager : MonoBehaviour
{
    public UITexture texture;
    //public GameObject audio;
    public MovieTexture playingMovie;
    private Coroutine coroutine;

    private void Start()
    {
        //测试用
        //PlayLocalMovie(Resources.Load<MovieTexture>("Video/TestVideo"));
        //PlayNetMovie("file:///G:/Videos/Fraps/TestVideo.ogv");
    }

    /// <summary>
    /// 播放本地视频
    /// </summary>
    public void PlayLocalMovie(MovieTexture movie, AudioClip sound = null)
    {
        playingMovie = movie;
        this.coroutine = StartCoroutine("PlayMovie");
    }

    /// <summary>
    /// 播放在线影片
    /// </summary>
    /// <param name="URL"></param>
    public void PlayNetMovie(string URL)
    {
        WWW www = new WWW(URL);
        playingMovie = www.movie;
        this.coroutine = StartCoroutine("PlayMovie");
    }

    /// <summary>
    /// 协程播放影片
    /// </summary>
    IEnumerator PlayMovie()
    {
        do
        {
            if (playingMovie != null && !playingMovie.isPlaying && playingMovie.isReadyToPlay)
            {
                texture.mainTexture = playingMovie;
                playingMovie.Play();
            }

            yield return new WaitForEndOfFrame();
        }
        while (playingMovie != null && playingMovie.isPlaying);

        yield return null;
    }
}
