using UnityEngine;

[CreateAssetMenu()]
public class MovementData : ScriptableObject
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Quaternion Rotation { get; private set; }
    public Vector3 velocity;
    public Vector3 angularVelocity;

    //the reson for the overloading is its convienent for the designer to work withg euler angles
    //in the editor but be able to call a constructor using transform.rotation
    public MovementData(Vector3 position,Quaternion rotation,Vector3 velocity,Vector3 angularVelocity)
    {
        this.position=position;
        this.Rotation=rotation;
        this.velocity=velocity;
        this.angularVelocity=angularVelocity;
    }

    public MovementData(Vector3 position,Vector3 eulerAngles,Vector3 velocity,Vector3 angularVelocity)
    {
        this.position=position;
        this.eulerAngles=eulerAngles;
        Rotation=Quaternion.Euler(this.eulerAngles);
        this.velocity=velocity;
        this.angularVelocity=angularVelocity;
    }
}