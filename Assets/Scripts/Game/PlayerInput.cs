using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private string axisName;

    void Update()
    {
        print(Input.GetAxis(axisName));
        if(Input.GetAxis(axisName)>0.1f)
            gameManager.Select();
    }
}