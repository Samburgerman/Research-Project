using System;
using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private int faceOnDice = -1;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float waitSeconds = 0.1f;
    [SerializeField] private Dice dice;

    private const float epsilon = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(groundTag))
            StartCoroutine(Grounded());
    }

    private IEnumerator Wait(float seconds) { yield return new WaitForSeconds(seconds); }

    private IEnumerator Grounded()
    {
        yield return Wait(waitSeconds);
        GroundedData groundedData = new(rb.velocity.magnitude<epsilon
            &&rb.angularVelocity.magnitude<epsilon,faceOnDice);
        dice.ActionOnLanding(groundedData);
    }
}

public struct GroundedData
{
    public bool IsGrounded { get; private set; }
    public int RollNumber { get; private set; }

    public GroundedData(bool isGrounded,int rollNumber)
    {
        this.IsGrounded=isGrounded;
        this.RollNumber=rollNumber;
    }

    public override string ToString()
    { return "isGrounded: "+IsGrounded+" RollNumber: "+RollNumber; }
}