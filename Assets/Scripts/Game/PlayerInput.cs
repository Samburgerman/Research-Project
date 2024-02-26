using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            gameManager.Select();
    }
}