using UnityEngine;

[CreateAssetMenu()]
public class MovementData : ScriptableObject
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Quaternion rotation { get; private set; }
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public MovementData(Vector3 position,Quaternion rotation,Vector3 velocity,Vector3 angularVelocity)
    {
        this.position=position;
        this.rotation=rotation;
        this.velocity=velocity;
        this.angularVelocity=angularVelocity;
    }

    public MovementData(Vector3 position,Vector3 eulerAngles,Vector3 velocity,Vector3 angularVelocity)
    {
        this.position=position;
        this.eulerAngles=eulerAngles;
        rotation=Quaternion.Euler(this.eulerAngles);
        this.velocity=velocity;
        this.angularVelocity=angularVelocity;
    }
}