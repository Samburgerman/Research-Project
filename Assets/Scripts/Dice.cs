using System;
using Unity.VisualScripting;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Range position;
    [SerializeField] private Range eulerAngles;
    [SerializeField] private Range velocity;
    [SerializeField] private Range angularVelocity;


    public int rollNumber { get; set; }

    public void Roll()
    {
        //apply some form of force and or torque idk
    }
}

[CreateAssetMenu]
public class Range : ScriptableObject
{
    public Vector3 lower;

    public Vector3 upper;

    public Range(Vector3 lower,Vector3 upper)
    { this.lower = lower; this.upper = upper; }

    public Vector3 Lerp(float amountThroughRange)
    {
        return new Vector3Lerp().Operation(lower,upper,amountThroughRange);
    }
}