using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LerperUtility
{
    private const bool useSmoothLerping = true;
    //smooth lerping makes position, velocity, and angular velocity be a more uniform distribution

    public static float multiplier { get; private set; } = 0.4337808f;

    //the param t in all of these functions is being passed as 0.
    public static Vector3 LerpPosition(MovementData lower,MovementData upper,float t)
    {
        if(useSmoothLerping)
            return SmoothLerp(lower.position,upper.position,t);
        return Vector3.Lerp(lower.position,upper.position,t);
    }

    public static Vector3 LerpRotation(MovementData lower,MovementData upper,float t)
    {
        return Vector3.Lerp(lower.rotation.eulerAngles,upper.rotation.eulerAngles,t);
    }

    public static Vector3 LerpVelocity(MovementData lower,MovementData upper,float t)
    {
        if(useSmoothLerping)
            return SmoothLerp(lower.velocity,upper.velocity,t);
        return Vector3.Lerp(lower.velocity,upper.velocity,t);
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
        float smoothT = GetSmoothT(t);
        Vector3 result = SmoothLerp(lower,upper,smoothT, true);
        return result;
    }

    public static Vector3 SmoothLerp(Vector3 lower, Vector3 upper, float t, bool isSmoothT)
    {
        if(isSmoothT)
            return Vector3.Lerp(lower,upper,t);
        return SmoothLerp(lower,upper,t);
    }

    private static float GetSmoothT(float t)
    {
        //to get the integral to be equal to 1 we multiply by 1/log(cosh(1))=0.43
        float smoothT = Tanh(t)*multiplier;//this adjusts t to be more unimodal and uniform
        if(smoothT>1||smoothT<0)
            throw new Exception("The smoothT value of "+smoothT+
                " makes the smooth lerp go out of bounds."+
                " This smoothT value was generated using the float t paramater of: "+
                t+".");
        return smoothT;
    }

    public static string ToString(Vector3 lower,Vector3 upper,float t)
    {
        return "Lerping lower: "
                  +lower
                  +" upper: "
                  +upper
                  +" t: "
                  +t
                  +" smoothT: "
                  +GetSmoothT(t)
                  +" result:"
                  +SmoothLerp(lower,upper,t);
    }

    //these are hyperbolic trigonometry functions
    private static float Sinh(float x) { return (Mathf.Exp(x)-Mathf.Exp(-x))/2; }

    private static float Cosh(float x) { return (Mathf.Exp(x)+Mathf.Exp(-x))/2; }

    private static float Tanh(float x) { return Sinh(x)/Cosh(x); }
}