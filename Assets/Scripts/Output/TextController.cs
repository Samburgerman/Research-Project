using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] TextMesh text;

    public void SetGameText(string str,Color color)
    {
        text.text=str;
        text.color=color;
    }
}