using System;using System.Collections;using UnityEditor.Timeline.Actions;using UnityEngine;public class GroundSensor : MonoBehaviour{    [SerializeField] private string groundTag = "Ground";    [SerializeField] private int faceOnDice = -1;    [SerializeField] private Rigidbody rb;    [SerializeField] private float waitSeconds = 0.1f;    [SerializeField] private Dice dice;

    private bool isGrounded = false;
    private const float epsilon = 0.1f;

    private void OnTriggerEnter(Collider other)    {        if(other.CompareTag(groundTag))            StartCoroutine(Grounded());    }    private void UpdateIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        dice.rollNumber=faceOnDice;
    }    private IEnumerator Wait(float seconds)    {        yield return new WaitForSeconds(seconds);    }    private IEnumerator Grounded()    {        yield return Wait(waitSeconds);        GroundedData groundedData = new GroundedData(rb.velocity.magnitude<epsilon            &&rb.angularVelocity.magnitude<epsilon);        UpdateIsGrounded(groundedData.GetIsGrounded());    }}public struct GroundedData
{
    private bool isGrounded;
    public GroundedData(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }
    public bool GetIsGrounded() { return isGrounded; }

    public override string ToString()
    { return ""+GetIsGrounded(); }
}