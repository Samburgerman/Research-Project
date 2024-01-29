using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private MovementData lower;
    [SerializeField] private MovementData upper;
    [SerializeField] Rigidbody rb;


    [SerializeField] private List<Vector3> eulerRotationMatrices;
    //the rotation required on each face to return to (0, 0, 0) rotation
    //do not use rotation in the y-axis

    private int rollNumber = -1;//if the roll number is used without being set it should generate an error
    //the rolls are not statistically independent
    private void Start()
    {
        Roll();//we will eventually want a game script to call the roll function.
    }

    public void Roll()
    {
        ActivateDice(true);
        /*
        Physics.simulationMode=SimulationMode.Script;
        for(int i = 0; i<500; i++)
        {
            Physics.Simulate(Time.fixedDeltaTime);
        }
        Physics.simulationMode=SimulationMode.FixedUpdate;
        */



        /*  if(rollNumber!=-1)
          {
              rollNumber++;
              if(rollNumber>6)
                  rollNumber-=6;
              DiceRotatorUtility.RotateToFace(
              transform,transform.rotation,eulerRotationMatrices,rollNumber+1);
          }*/
        //step one is to activate the gameObject
        Range range = new(lower,upper);
        UpdateGameObjectToMovementData(range.GetMovementDataForRoll());
    }

    private void ActivateDice(bool willBeActive)
    {
        gameObject.SetActive(willBeActive);
    }

    private void UpdateGameObjectToMovementData(MovementData movementData)
    {
        transform.SetLocalPositionAndRotation(movementData.position,movementData.rotation);
        rb.velocity=movementData.velocity;
        rb.angularVelocity=movementData.angularVelocity;
    }

    public void ActionOnLanding(GroundedData groundedData)
    {
        rollNumber=GetOppositeSideFaceNumber(groundedData.FaceNumOnBottom);
        int indexOfRollNumber = rollNumber-1;
        if(indexOfRollNumber==-1)
            throw new Exception("indexOfRollNumber is out of bounds: "+indexOfRollNumber);
        StartCoroutine(DisableDice());
    }

    public IEnumerator DisableDice()
    {
        yield return new WaitForSeconds(GameManager.waitAfterDiceDisplay);
        gameObject.SetActive(false);
        Roll();
    }

    public static int GetOppositeSideFaceNumber(int bottom)
    {
        return 6+1-bottom;//6 is the number of faces on the dice
    }
}

public static class DiceRotatorUtility
{
    private static void ResetRotationToOrigin(Transform transform,Quaternion initialRotation)
    {
        transform.rotation=initialRotation;
    }

    public static void RotateToFace(
        Transform transform,Quaternion initialRotation,List<Vector3> rotationMatrices,int faceNumOnTop)
    {
        ResetRotationToOrigin(transform,initialRotation);
        Vector3 initalRotationVector = transform.rotation.eulerAngles;
        int indexOfFaceNumber = faceNumOnTop-1;
        initalRotationVector+=rotationMatrices[indexOfFaceNumber];
        Debug.Log("faceNumOnTop: "+faceNumOnTop+" indexOfFaceNumber: "+indexOfFaceNumber);
        transform.rotation=Quaternion.Euler(initalRotationVector);
    }
}