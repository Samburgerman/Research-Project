using UnityEngine;

[CreateAssetMenu()]
public class MovementData : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    /*
    public MovementData(Vector3 position,Quaternion rotation,Vector3 velocity,Vector3 angularVelocity)
    {
        this.position=position;
        this.rotation=rotation;
        this.velocity=velocity;
        this.angularVelocity=angularVelocity;
    }

    public MovementData(Vector3 position, Vector3 eulerAngles, Vector3 velocity,Vector3 angularVelocity)
    {
        this.position=position;
        rotation=Quaternion.Euler(eulerAngles);
        this.velocity=velocity;
        this.angularVelocity=angularVelocity;
    }
    */

    public override string ToString()
    {
        string msg0 = "position: "+position;
        string msg1 = " eulerAngles: "+rotation.eulerAngles;
        string msg2 = " Rotation: "+rotation;
        string msg3 = " velocity: "+velocity;
        string msg4 = " angularVelocity: "+velocity;
        return msg0+msg1+msg2+msg3+msg4;
    }
}