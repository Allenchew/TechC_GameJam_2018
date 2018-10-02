using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour {
    public static SoundManager SoundInstance;
    public AudioClip[] BgmClip= new AudioClip[4];
    public AudioClip[] SEClip = new AudioClip[12];
    public AudioSource BgmSource;
    public AudioSource SESource;

    private string[] BgmNameToNum = new string[4] { "Title", "CharacterSelect", "Racing","Result" };
    private string[] SENameToNum = new string[12] { "Explode", "FixButton", "Icy","Jump","Kick","Landing","Laugh","RollFinish","SelectingButton","Switch","SwordIcy","Walking" };
    
    void Awake()
    {
        if (SoundInstance == null)
        {
            SoundInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (SoundInstance != this)
        {
            Destroy(gameObject);
        }
    }
    public void PlayBgm(string BgmName,bool OnAwake,bool OnLoop)
    {
        BgmSource.clip = (BgmClip[Array.IndexOf(BgmNameToNum, BgmName)]);
        BgmSource.playOnAwake = OnAwake;
        BgmSource.loop = OnLoop;
        BgmSource.Play();
    }
    public void PlaySE(string SEName, bool OnAwake, bool OnLoop)
    {
        SESource.clip = (SEClip[Array.IndexOf(SENameToNum, SEName)]);
        SESource.playOnAwake = OnAwake;
        SESource.loop = OnLoop;
        SESource.Play();
    }
    public void StopPlay()
    {
        if (SESource.isPlaying)
        {
            SESource.Stop();
        }
        if (BgmSource.isPlaying)
        {
            BgmSource.Stop();
        }
    }
}
