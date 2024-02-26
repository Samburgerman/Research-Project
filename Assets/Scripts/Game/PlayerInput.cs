using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private KeyCode roll = KeyCode.Space;

    void Update()
    {
        if(Input.GetKeyDown(roll))
            gameManager.Select();
    }
}