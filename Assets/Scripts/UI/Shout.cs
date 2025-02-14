using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shout : MonoBehaviour
{
    public Image img;

    void Start()
    {
        UnableImg();
    }

    public void UnableImg()
    {
        img.enabled = false;
    }

    public void EnableImg()
    {
        print("imagenHabilitada");

        img.enabled = true;
    }
}