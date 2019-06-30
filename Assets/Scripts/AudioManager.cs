using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip pickup_Sound, dead_Sound;

    private void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    public void Play_Pickup_Sound()
    {
        AudioSource.PlayClipAtPoint(pickup_Sound, transform.position);
    }
    public void Play_Dead_Sound()
    {
        AudioSource.PlayClipAtPoint(dead_Sound, transform.position);
    }
}
