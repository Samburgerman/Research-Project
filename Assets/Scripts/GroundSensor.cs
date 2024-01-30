using System.Collections;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private readonly string groundTag = "Ground";
    [SerializeField] private readonly int faceOnDice = -1;
    [SerializeField] private readonly Rigidbody rb;
    [SerializeField] private readonly float waitSeconds = 0.1f;
    [SerializeField] private readonly Dice dice;

    private const float epsilon = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Physics.simulationMode.ToString());
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
    public int FaceNumOnBottom { get; private set; }

    public GroundedData(bool isGrounded,int faceNumOnBottom)
    { IsGrounded=isGrounded; FaceNumOnBottom=faceNumOnBottom; }

    public override readonly string ToString()
    { return "isGrounded: "+IsGrounded+" RollNumber: "+FaceNumOnBottom; }
}