using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void PlaySound(AudioClip audioClip, float duration)
    {
        source.clip=audioClip;
        source.Play();
        StopSoundAfter(duration);
    }

    private IEnumerator StopSoundAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        source.Stop();
    }
}