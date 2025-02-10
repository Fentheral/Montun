using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiText : MonoBehaviour
{
    public TMP_Text uiText;
    public Image img;

    void Start()
    {
        UnableImg();
        uiText.text = "";
    }

    public void UnableImg()
    {
        img.enabled = false;
    }

    public void EnableImg()
    {
        img.enabled = true;
    }
}
