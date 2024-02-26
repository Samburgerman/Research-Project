using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private string axisName;

    void Update()
    {
        if(Input.GetAxis(axisName)>0.1f||Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.Mouse0)||Input.GetMouseButtonDown(0))
            gameManager.Select();
    }
}