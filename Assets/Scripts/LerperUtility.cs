using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LerperUtility
{
    private const bool useSmoothLerping = true;

    public static Vector3 LerpPosition(MovementData lower, MovementData upper, float t)
    {
        if(useSmoothLerping)
            return SmoothLerp(lower.position,upper.position,t);
#pragma warning disable CS0162 // Unreachable code detected
        return Vector3.Lerp(lower.position,upper.position,t);
#pragma warning restore CS0162 // Unreachable code detected
    }

    public static Quaternion LerpRotation(MovementData lower,MovementData upper,float t)
    {
        Quaternion rotation = Quaternion.Lerp(lower.rotation,upper.rotation,t);
        return rotation;
    }

    public static Vector3 LerpVelocity(MovementData lower,MovementData upper,float t)
    {
        if(useSmoothLerping)
            return SmoothLerp(lower.velocity,upper.velocity,t);
#pragma warning disable CS0162 // Unreachable code detected
        return Vector3.Lerp(lower.velocity,upper.velocity,t);
#pragma warning restore CS0162 // Unreachable code detected
    }

    public static Vector3 LerpAngularVelocity(MovementData lower,MovementData upper,float t)
    {
        Vector3 angularVelocity = SmoothLerp(lower.angularVelocity,upper.angularVelocity,t);
        return angularVelocity;
    }

    private static Vector3 SmoothLerp(Vector3 lower,Vector3 upper,float t)
    {
        //the integral tanh(x) dx = ln( | cosh (x) | )
        //this means that tanh x will produce a smooth curve
        float multiplier = 0.4337808f;
        //to get the integral to be = to 1 we multiply by 1/log(cosh(1))=0.43
        float smoothT = Tanh(t)*multiplier;//this adjusts t to be more unimodal and uniform
        return Vector3.Lerp(lower,upper,smoothT);
    }

    //all of this is in radians
    private static float Sinh(float x) { return (Mathf.Exp(x)-Mathf.Exp(-x))/2; }

    private static float Cosh(float x) { return (Mathf.Exp(x)+Mathf.Exp(-x))/2; }

    private static float Tanh(float x) { return Sinh(x)/Cosh(x); }
}