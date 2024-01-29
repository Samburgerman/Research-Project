using UnityEngine;

[CreateAssetMenu()]
public class MovementData : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public override string ToString()
    {
        string msg0 = "position: "+position;
        string msg1 = " eulerAngles: "+rotation.eulerAngles;
        string msg2 = " Rotation: "+rotation;
        string msg3 = " velocity: "+velocity;
        string msg4 = " angularVelocity: "+velocity;
        return msg0+msg1+msg2+msg3+msg4;
    }
}

public class Range
{
    private MovementData lower;//the values of lower are correct
    private MovementData upper;//the values of upper are correct

    public Range(MovementData lower,MovementData upper)
    {
        this.lower=lower;
        this.upper=upper;
    }

    public MovementData GetMovementDataForRoll()
    {
        //the indexes of the lerp values have no signifigance as the elements are random
        Vector3 position = SmoothLerperUtility.CISmoothLerp(lower.position,upper.position);
        Vector3 eulerAngles = SmoothLerperUtility.CISmoothLerp(lower.rotation.eulerAngles,upper.rotation.eulerAngles);
        Vector3 velocity = SmoothLerperUtility.CISmoothLerp(lower.velocity,upper.velocity);
        Vector3 angularVelocity = SmoothLerperUtility.CISmoothLerp(lower.angularVelocity,upper.angularVelocity);
        MovementData middle = CreateMovementData(position,eulerAngles,velocity,angularVelocity);
        return middle;
    }

    private static MovementData CreateMovementData(
        Vector3 position,Vector3 eulerAngles,Vector3 velocity,Vector3 angularVelocity)
    {
        MovementData middle = ScriptableObject.CreateInstance<MovementData>();
        //constructor params for movementData: position,rotation,velocity,angularVelocity
        middle.position=position;
        middle.rotation=Quaternion.Euler(eulerAngles);
        middle.velocity=velocity;
        middle.angularVelocity=angularVelocity;
        return middle;
    }

    public override string ToString()
    {
        string msg0 = "Lower: "+lower;
        string msg1 = " Upper: "+upper;
        return msg0+msg1;
    }
}