using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SmoothLerperUtility
{
    //smooth lerping makes position, velocity, and angular velocity be a more uniform distribution

    public static Vector3 CISmoothLerp(Vector3 lower, Vector3 upper)
        //vector component independent smooth liner extrapolation
    {
        LerpValues3D lerpValues = new();
        Vector3 vector3 = lerpValues.GetSmoothXYZ();
        float x = LerpOneDimension(lower.x,upper.x,vector3.x);
        float y = LerpOneDimension(lower.y,upper.y,vector3.y);
        float z = LerpOneDimension(lower.z,upper.z,vector3.z);
        return new Vector3(x,y,z);
    }

    private static float LerpOneDimension(float lower,float upper,float t)
    {
        return (upper-lower)*t+lower;
    }
}

public class LerpValues2D
{
    public static float GenerateRandomLerpValue()
    {
        float lerpValue = UnityEngine.Random.Range(0.0f,1.0f);
        //we need f so it returns a float
        return lerpValue;
    }

    public float x;
    public float z;

    public LerpValues2D() { x=GenerateRandomLerpValue();z=GenerateRandomLerpValue();}

    public LerpValues2D(float x, float z){this.x = x; this.z = z;}
    //this constructor can be used if you dont want random lerp values

    protected static float multiplier = 1.313f;

    //these are hyperbolic trigonometry functions
    protected static float Sinh(float x) { return (Mathf.Exp(x)-Mathf.Exp(-x))/2; }

    protected static float Cosh(float x) { return (Mathf.Exp(x)+Mathf.Exp(-x))/2; }

    protected static float Tanh(float x) { return Sinh(x)/Cosh(x); }

    protected float GetSmoothValue(float t)
    {
        float smoothT = Tanh(t)*multiplier;//this adjusts t to be more unimodal and uniform
        if(smoothT>1||smoothT<0)
            throw new Exception("The smoothT value of "+smoothT+
                " makes the smooth lerp go out of bounds."+
                " This smoothT value was generated using the float t paramater of: "+
                t+".");
        return smoothT;
    }

    private float GetSmoothX()
    {
        return GetSmoothValue(x);
    }

    private float GetSmoothZ()
    {
        return GetSmoothValue(z);
    }

    public Vector2 GetSmoothXZ() { return new Vector2(GetSmoothX(),GetSmoothZ()); }
}

public class LerpValues3D : LerpValues2D
{
    public float y;

    private float GetSmoothY()
    {
        return GetSmoothValue(y);
    }

    public LerpValues3D(float x, float y, float z)
    { this.x = x; this.y = y; this.z = z; }

    public LerpValues3D()
    { x=GenerateRandomLerpValue(); y=GenerateRandomLerpValue(); z=GenerateRandomLerpValue(); }

    public Vector3 GetSmoothXYZ()
    {
        Vector2 smoothXZ = GetSmoothXZ();
        return new Vector3(smoothXZ.x,GetSmoothY(),smoothXZ.y);
        //smoothXZ.y actually represents a z coordinate
    }
}