using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr inst;
    public AudioClip nullSound;
    public AudioClip winSound;
    public AudioClip heldSound;
    public AudioClip selectSound;
    public AudioSource speakerSource;

    private void Awake()
    {
        inst = this;
    }

    public void playNullSound ()
    {
        speakerSource.PlayOneShot(nullSound);
    }

    public void playHeldSound ()
    {
        speakerSource.PlayOneShot(heldSound);
    }

    public void playWinSound ()
    {
        speakerSource.PlayOneShot(winSound);
    }

    public void playSelectSound ()
    {
        speakerSource.PlayOneShot(selectSound);
    }
}
