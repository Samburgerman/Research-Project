using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void PlaySound(AudioClip audioClip)
    {
        source.clip = audioClip;
        source.Play();
    }
}