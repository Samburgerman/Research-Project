using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] TextMesh text;
    [SerializeField] private List<TextMesh> playerTextMeshes = new List<TextMesh>();

    public void SetGameText(string str)
    {
        text.text = str;
    }

    public void SetPlayerText(int i, string str)
    {
        playerTextMeshes[i].text = str;
    }
}