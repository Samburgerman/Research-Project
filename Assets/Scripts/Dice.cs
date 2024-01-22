using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private MovementData lower;
    [SerializeField] private MovementData upper;
    [SerializeField] Rigidbody rb;

    private List<float> lerpValues = new List<float>();

    public int rollNumber { get; set; }

    public void Roll()
    {
        for(int i = 0; i<4; i++)
        {
            lerpValues.Add(UnityEngine.Random.Range(0,1));
        }
        Range range = new(lower,upper,lerpValues);
        MovementData movementData = range.GetMovementDataForRoll();
        UpdateGameobjectToMovementData(movementData);
    }

    private void UpdateGameobjectToMovementData(MovementData movementData)
    {
        transform.position=movementData.position;
        transform.rotation=movementData.rotation;
        rb.velocity=movementData.velocity;
        rb.angularVelocity=movementData.angularVelocity;
    }
}

public class Range
{
    private MovementData lower;
    private MovementData upper;
    private const bool useSmoothLerping = true;

    private List<float> lerpValues = new List<float>();

    public Range(MovementData lower,MovementData upper,List<float> lerpValues)
    {
        this.lower=lower;
        this.upper=upper;
        this.lerpValues=lerpValues;
        if(lerpValues.Count!=4)
        {
            throw new Exception("4 lerp values must be in lerpValues");
        }
    }

    public MovementData GetMovementDataForRoll()
    {
        //the indexes of the lerp values have no signifigance as the elements are random
        Vector3 position = LerpPosition(lerpValues[0]);
        Quaternion rotation = LerpRotation(lerpValues[1]);
        Vector3 velocity = LerpVelocity(lerpValues[2]);
        Vector3 angularVelocity = LerpAngularVelocity(lerpValues[3]);
        MovementData movementData = new(position,rotation,velocity,angularVelocity);
        return movementData;
    }

    private Vector3 LerpPosition(float t)
    {
        if(useSmoothLerping)
            return SmoothLerp(lower.position,upper.position,t);
#pragma warning disable CS0162 // Unreachable code detected
        return Vector3.Lerp(lower.position,upper.position,t);
#pragma warning restore CS0162 // Unreachable code detected
    }

    private Quaternion LerpRotation(float t)
    {
        Quaternion rotation = Quaternion.Lerp(lower.rotation,upper.rotation,t);
        return rotation;
    }

    private Vector3 LerpVelocity(float t)
    {
        if(useSmoothLerping)
        return SmoothLerp(lower.velocity,upper.velocity,t);
#pragma warning disable CS0162 // Unreachable code detected
        return Vector3.Lerp(lower.velocity,upper.velocity,t);
#pragma warning restore CS0162 // Unreachable code detected
    }

    private Vector3 LerpAngularVelocity(float t)
    {
        Vector3 angularVelocity = SmoothLerp(lower.angularVelocity,upper.angularVelocity,t);
        return angularVelocity;
    }

    private Vector3 SmoothLerp(Vector3 lower,Vector3 upper,float t)
    { return Vector3.Lerp(lower,upper,t); }
}