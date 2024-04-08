using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public int channels;
    public float sfxVolume;
    AudioSource[] sfxPlayers;
    int channelsIndex;

    public enum Sfx {Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win}

    void Awake(){
        instance = this;
        Init();
    }

    void Init(){
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < sfxPlayers.Length; i++){
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay){
        if(isPlay){
            bgmPlayer.Play();
        }else{
            bgmPlayer.Stop();
        }
    }

    public void PlaySfx(Sfx sfx){

        for(int i=0; i<sfxPlayers.Length; i++){
            int loopIndex = (i + channelsIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying){
                continue;
            }

            channelsIndex = i;
            sfxPlayers[0].clip = sfxClips[(int)sfx];
            sfxPlayers[0].Play();
            break;
        }
    }
}
