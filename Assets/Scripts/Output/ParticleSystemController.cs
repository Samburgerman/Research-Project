using System.Collections;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;

    public void RunParticles(Vector3 moveTo,float duration,Color color)
    {
        transform.position=moveTo;
        StartCoroutine(RunParticlesFor(duration,color));
    }

    private IEnumerator RunParticlesFor(float duration,Color color)//add vector3 postion to input
    {
        system.startColor=color;
        system.Play();
        yield return new WaitForSeconds(duration);
        system.Stop();
    }
}