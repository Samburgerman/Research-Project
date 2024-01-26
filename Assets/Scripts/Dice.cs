using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
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
            lerpValues.Add(UnityEngine.Random.Range(0.0f,1.0f));
        //we need f so it returns a float
        return lerpValues;
    }

    private void UpdateGameObjectToMovementData(MovementData movementData)
    {
        transform.SetLocalPositionAndRotation(movementData.position,movementData.rotation);
        rb.velocity=movementData.velocity;
        rb.angularVelocity=movementData.angularVelocity;
    }

    public void ActionOnLanding(GroundedData groundedData)
    {
        rollNumber=ConvertBottomSideToTop(groundedData.FaceNumOnBottom);
        StartCoroutine(DisableDice());
    }

    public IEnumerator DisableDice()
    {
        yield return new WaitForSeconds(diceFaceDisplayTime);
        print("rollNumber: "+rollNumber);
        gameObject.SetActive(false);
    }

    public static int ConvertBottomSideToTop(int bottom)
    {
        return 6+1-bottom;//6 is the number of faces on the dice
    }
}

public class Range
{
    private MovementData lower;//the values of lower are correct
    private MovementData upper;//the values of upper are correct

    private List<float> lerpValues = new();
    //how the lerping is done is incorrect

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
        Vector3 position = LerperUtility.LerpPosition(lower,upper,lerpValues[0]);
        Vector3 eulerAngles = LerperUtility.LerpRotation(lower,upper,lerpValues[1]);
        Vector3 velocity = LerperUtility.LerpVelocity(lower,upper,lerpValues[2]);
        Vector3 angularVelocity = LerperUtility.LerpAngularVelocity(lower,upper,lerpValues[3]);
        MovementData middle = CreateMovementData(position,eulerAngles,velocity,angularVelocity);
        return middle;
    }

    private static MovementData CreateMovementData(Vector3 position,Vector3 eulerAngles,Vector3 velocity,Vector3 angularVelocity)
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
        string msg2 = " ";
        foreach(float lerpValue in lerpValues)
            msg2+=lerpValue+" ";
        return msg0+msg1+msg2;
    }
}