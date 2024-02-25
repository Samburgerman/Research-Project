using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    private IEnumerator RunParticles(float duration,Color color)
    {
        particleSystem.startColor=color;
        particleSystem.Play();
        yield return new WaitForSeconds(duration);
        particleSystem.Stop();
    }
}