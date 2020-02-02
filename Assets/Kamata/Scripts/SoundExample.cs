using System;
using UnityEngine;

public class SoundExample : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip seClip;
    
    private void OnGUI()
    {
        if (GUILayout.Button("Play BGM"))
        {
            SoundManager.Instance.PlayBGM(bgmClip);
        }

        if (GUILayout.Button("Stop BGM"))
        {
            SoundManager.Instance.StopBGM();
        }

        if (GUILayout.Button("Play SE"))
        {
            SoundManager.Instance.PlaySE(seClip);
        }
    }
}
