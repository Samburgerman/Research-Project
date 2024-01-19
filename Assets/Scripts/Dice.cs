using UnityEngine;

public class Dice:MonoBehaviour
{
    public void Roll()
    {
        //apply some form of force and or torque idk
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision);
    }
}