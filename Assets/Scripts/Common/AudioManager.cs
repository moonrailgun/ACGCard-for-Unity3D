using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource soundEffect;

    public List<AudioClip> audioList;
    private Dictionary<string, AudioClip> audioMap;

    void Awake()
    {
        LoadAudio();
    }

    /// <summary>
    /// 载入音频地图
    /// </summary>
    private void LoadAudio()
    {
        audioMap = new Dictionary<string,AudioClip>();

        foreach(AudioClip audio in audioList)
        {
            audioMap.Add(audio.name, audio);
        }
    }

    /// <summary>
    /// 播放音效
    /// 播放一次
    /// </summary>
    public void PlayerMusic(string soundName)
    {
        if(audioMap.ContainsKey(soundName))
        {
            soundEffect.PlayOneShot(audioMap[soundName]);
        }
    }

    /// <summary>
    /// 播放BGM
    /// 循环
    /// </summary>
    public void PlayerBGM(string soundName)
    {
        if (audioMap.ContainsKey(soundName))
        {
            backgroundMusic.clip = audioMap[soundName];
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }
}
