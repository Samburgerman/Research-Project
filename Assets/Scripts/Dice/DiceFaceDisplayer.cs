using UnityEngine;

[RequireComponent(typeof(DiceFaceDisplayer))]
public class DiceFaceDisplayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void DisplayDiceFace(DiceFace diceFace) => spriteRenderer.sprite=diceFace.sprite;
}