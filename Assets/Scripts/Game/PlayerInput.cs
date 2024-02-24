using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] KeyCode roll = KeyCode.Space;

    void Update()
    {
        Debug.Log(Input.GetKeyDown(roll));
    }
}