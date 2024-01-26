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
    [SerializeField] private float timeScale = 1.0f;

    [SerializeField] private List<Vector3> eulerRotationMatrices;

    [SerializeField] List<int> monteCarloRollCounts = new();
    //the rotation required on each face to return to (0, 0, 0) rotation
    //do not use rotation in the y-axis

    private int rollNumber = -1;//if the roll number is used without being set it should generate an error
    //the rolls are not statistically independent
    private void Start()
    {
        LerperUtility.LogDiagnostics();
        UnityEngine.Time.timeScale=timeScale;
        InitializeMonteCarloRollCounts();
        Roll();//we will eventually want a game script to call the roll function.
    }

    private void LogChiSquareTestResults()
    {
        int sum = 0;
        foreach (int count in monteCarloRollCounts)
        { sum+= count; }
        float exp = sum/6;
        List<float> differences = new();
        foreach(int count in monteCarloRollCounts)
            differences.Add(Mathf.Pow(count-exp,2.0f)/exp);
        float x2 = 0.0f;
        foreach(int x in differences)
            x2+=x;
        LogMonteCarloResults();
        Debug.Log("x^2 test result: "+x2);
    }

    private void InitializeMonteCarloRollCounts()
    {
        for(int i = 0; i<6; i++)
            monteCarloRollCounts.Add(0);
    }

    public void Roll()
    {
        ActivateDice();
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
        Range range = new(lower,upper,GenerateLerpValues());
        //Debug.Log("range: "+range);
        UpdateGameObjectToMovementData(range.GetMovementDataForRoll());
        //LogChiSquareTestResults();
    }

    private void LogMonteCarloResults()
    {
        string str = "";
        foreach(int count in monteCarloRollCounts)
            str+=count+", ";
        Debug.Log(str);
    }

    private void ActivateDice()
    {
        gameObject.SetActive(true);
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
        rollNumber=GetOppositeSideFaceNumber(groundedData.FaceNumOnBottom);
        int indexOfRollNumber = rollNumber-1;
        if(indexOfRollNumber!=-1)
        {
            //String msg = "";
            //foreach(int count in monteCarloRollCounts)
            //    msg+= count;
            //Debug.Log("Contents of monteCarloRollCounts["+indexOfRollNumber+"]: "+msg);
            monteCarloRollCounts[indexOfRollNumber]+=1;
        }
        else
            throw new Exception("indexOfRollNumber is out of bounds: "+indexOfRollNumber);
        StartCoroutine(DisableDice());
    }

    public IEnumerator DisableDice()
    {
        yield return new WaitForSeconds(diceFaceDisplayTime);
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
    private static void ResetRotationToOrigin(Transform transform, Quaternion initialRotation)
    {
        transform.rotation=initialRotation;
    }

    public static void RotateToFace(
        Transform transform,Quaternion initialRotation,List<Vector3> rotationMatrices,int faceNumOnTop)
    {
        ResetRotationToOrigin(transform, initialRotation);
        Vector3 initalRotationVector = transform.rotation.eulerAngles;
        int indexOfFaceNumber = faceNumOnTop-1;
        initalRotationVector+=rotationMatrices[indexOfFaceNumber];
        Debug.Log("faceNumOnTop: "+faceNumOnTop+" indexOfFaceNumber: "+indexOfFaceNumber);
        transform.rotation=Quaternion.Euler(initalRotationVector);
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