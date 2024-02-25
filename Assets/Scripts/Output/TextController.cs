using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] TextMesh text;
    [SerializeField] private List<TextMesh> playerTextMeshes = new();

    public void SetGameText(string str, Color color)
    {
        text.text = str;
        text.color= color;
    }

    public void SetPlayerText(int i, string str, Color color)
    {
        playerTextMeshes[i].text = str;
        playerTextMeshes[i].color=color;
    }
}