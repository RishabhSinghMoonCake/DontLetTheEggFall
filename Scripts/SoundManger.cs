using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> clipList;

    private bool inLoop = false;

    #region Singleton

    public static SoundManger instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int soundID)
    {
        audioSource.clip = clipList[soundID];
        audioSource.Play();
        audioSource.loop = false;
    }

    public void StartLoop(int soundID)
    {
        if (inLoop) return;
        audioSource.clip = clipList[soundID];

        audioSource.Play();
        inLoop = true;
        audioSource.loop = true;
    }

    public void StopLoop()
    {
        if(!inLoop) return;
        inLoop = false;
        audioSource.loop = false;
    }
}
