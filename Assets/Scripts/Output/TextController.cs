using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] TextMesh text;

    public void SetText(string str)
    {
        text.text = str;
    }
}