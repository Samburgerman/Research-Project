using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private MovementData lower;
    [SerializeField] private MovementData upper;
    [SerializeField] Rigidbody rb;

    private int rollNumber = -1;//if the roll number is used without being set it should generate an error

    public void Roll()
    {
        gameObject.SetActive(true);
        //step one is to activate the gameObject
        Range range = new(lower,upper,GenerateLerpValues());
        UpdateGameObjectToMovementData(range.GetMovementDataForRoll());
    }

    private List<float> GenerateLerpValues()
    {
        List<float> lerpValues = new List<float>();
        for(int i = 0; i<4; i++)
            lerpValues.Add(UnityEngine.Random.Range(0,1));
        return lerpValues;
    }

    private void UpdateGameObjectToMovementData(MovementData movementData)
    {
        transform.position=movementData.position;
        transform.rotation=movementData.rotation;
        rb.velocity=movementData.velocity;
        rb.angularVelocity=movementData.angularVelocity;
    }

    public void ActionOnLanding(GroundedData groundedData)
    {
        rollNumber=groundedData.rollNumber;
        DisableDice();
    }

    public void DisableDice() {gameObject.SetActive(false); }
}

public class Range
{
    private MovementData lower;
    private MovementData upper;

    private List<float> lerpValues = new List<float>();

    public Range(MovementData lower,MovementData upper,List<float> lerpValues)
    {
        this.lower=lower;
        this.upper=upper;
        this.lerpValues=lerpValues;
        if(lerpValues.Count!=4)
            throw new Exception("4 floats must be contained in the list<float> lerpValues");
    }

    public MovementData GetMovementDataForRoll()
    {
        //the indexes of the lerp values have no signifigance as the elements are random
        Vector3 position = LerperUtility.LerpPosition(lower, upper, lerpValues[0]);
        Quaternion rotation = LerperUtility.LerpRotation(lower, upper, lerpValues[1]);
        Vector3 velocity = LerperUtility.LerpVelocity(lower,upper,lerpValues[2]);
        Vector3 angularVelocity = LerperUtility.LerpAngularVelocity(lower,upper,lerpValues[3]);
        MovementData movementData = new(position,rotation,velocity,angularVelocity);
        return movementData;
    }
}