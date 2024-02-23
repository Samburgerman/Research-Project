using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DiceFaceDisplayer))]
public class DiceFaceDisplayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void DisplayDiceFace(DiceFace diceFace) => spriteRenderer.sprite=diceFace.sprite;
}