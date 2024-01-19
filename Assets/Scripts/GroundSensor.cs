using System;using System.Collections;using UnityEditor.Timeline.Actions;using UnityEngine;public class GroundSensor : MonoBehaviour{    [SerializeField] private string groundTag = "Ground";    [SerializeField] private int numOnDice = -1;    [SerializeField] private Rigidbody rb;    [SerializeField] private float waitSeconds = 0.1f;    [SerializeField] private Dice dice;

    private bool isGrounded = false;

    private void OnTriggerEnter(Collider other)    {        StartCoroutine(Grounded());        print("collision detected");    }    private void UpdateIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        print(this.isGrounded);
    }    private IEnumerator Wait(float seconds)    {        yield return new WaitForSeconds(seconds);    }    private IEnumerator Grounded()    {        print("begin");        yield return Wait(waitSeconds);        GroundedData groundedData = new GroundedData(rb.velocity.magnitude<0.1f);        UpdateIsGrounded(groundedData.GetIsGrounded());        print("end");    }}public struct GroundedData
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