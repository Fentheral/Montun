using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    void Start()
    {
        Quaternion rot = Quaternion.Euler(0,0,Random.Range(0f,360f));
        this.transform.rotation = rot;
        Destroy(this.gameObject, 0.2f);
    }
}
