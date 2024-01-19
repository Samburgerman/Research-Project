using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Dice dice;

    private void OnTriggerEnter(Collider other)
    {
        if(!AtRest())
        { return; }
        if(!other.tag.Equals(groundTag))
        { return; }

    }

    private bool AtRest()
    {
        if(rb.velocity.magnitude<Mathf.Epsilon)
            return true;
        return false;
    }
}