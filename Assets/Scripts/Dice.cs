using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private MovementData lower;
    [SerializeField] private MovementData upper;
    [SerializeField] Rigidbody rb;

    [SerializeField] private float diceFaceDisplayTime = 1.5f;

    private int rollNumber = -1;//if the roll number is used without being set it should generate an error

    private void Start()
    {
        Roll();
    }

    public void Roll()
    {
        gameObject.SetActive(true);
        //step one is to activate the gameObject
        Range range = new(lower,upper,GenerateLerpValues());
        print("range: "+range);
        UpdateGameObjectToMovementData(range.GetMovementDataForRoll());
    }

    private List<float> GenerateLerpValues()
    {
        List<float> lerpValues = new();
        for(int i = 0; i<4; i++)
            lerpValues.Add(UnityEngine.Random.Range(0,1));
        return lerpValues;
    }

    private void UpdateGameObjectToMovementData(MovementData movementData)
    {
        print("movementData: "+movementData);
        transform.SetLocalPositionAndRotation(movementData.position,movementData.Rotation);
        rb.velocity=movementData.velocity;
        rb.angularVelocity=movementData.angularVelocity;
    }

    public void ActionOnLanding(GroundedData groundedData)
    {
        rollNumber=groundedData.RollNumber;
        StartCoroutine(DisableDice());
    }

    public IEnumerator DisableDice() 
    {
        yield return new WaitForSeconds(diceFaceDisplayTime);
        print("rollNumber: "+rollNumber);
        gameObject.SetActive(false);
    }
}

public class Range
{
    private MovementData lower;
    private MovementData upper;

    private List<float> lerpValues = new();

    public Range(MovementData lower,MovementData upper,List<float> lerpValues)
    {
        this.lower=lower;
        this.upper=upper;
        this.lerpValues=lerpValues;
        if(lerpValues.Count!=4)
            throw new Exception("4 floats must be contained in the list<float> lerpValues.");
    }

    public MovementData GetMovementDataForRoll()
    {
        //the indexes of the lerp values have no signifigance as the elements are random
        Vector3 position = LerperUtility.LerpPosition(lower, upper, lerpValues[0]);
        Vector3 rotation = LerperUtility.LerpRotation(lower,upper,lerpValues[1]);
        Vector3 velocity = LerperUtility.LerpVelocity(lower,upper,lerpValues[2]);
        Vector3 angularVelocity = LerperUtility.LerpAngularVelocity(lower,upper,lerpValues[3]);
        MovementData movementData = new(position,rotation,velocity,angularVelocity);
        return movementData;
    }

    public override string ToString()
    {
        string msg0 = "Lower: "+lower;
        string msg1 = "Upper: "+upper;
        return msg0+msg1;
    }
}