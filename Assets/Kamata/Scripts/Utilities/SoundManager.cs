using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private const short SoundEffectAudioSourceCnt = 10;
    
    private AudioSource _bgmAudioSource;
    private AudioSource[] _seAudioSources;
    private int _audioSrcPtr = 0;

    private float _audioBGMPausingTime;
    
    private void Awake()
    {
        if (_bgmAudioSource == null)
        {
            _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (_seAudioSources == null || _seAudioSources.Length < SoundEffectAudioSourceCnt)
        {
            _seAudioSources = new AudioSource[SoundEffectAudioSourceCnt];
            for (var i = 0; i < SoundEffectAudioSourceCnt; i++)
            {
                if (_seAudioSources != null) _seAudioSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Debug.Log($"_seAudioSources.length: {_seAudioSources.Length}, SoundEffectAudioSourceCnt: {SoundEffectAudioSourceCnt}");
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();
    }

    public void StopBGM() {
        _bgmAudioSource.Stop();
    }

    public void PauseBGM() {
        _bgmAudioSource.Pause();
        this._audioBGMPausingTime = _bgmAudioSource.time;
    }

    public void ResumeBGM() {
        _bgmAudioSource.time = this._audioBGMPausingTime;
        _bgmAudioSource.Play();
    }

    public void PlaySE(AudioClip clip)
    {
        _seAudioSources[_audioSrcPtr]?.PlayOneShot(clip);
        _audioSrcPtr = (_audioSrcPtr + 1) % 10;
    }
}
