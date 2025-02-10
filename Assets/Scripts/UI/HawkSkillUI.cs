using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HawkSkillUI : MonoBehaviour


{
    public Image img;
    CameraManager hawkSight;

    private void Awake()
    {
        hawkSight = FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
    }

    private void Update()
    {
        if (hawkSight.counter <= hawkSight.cooldown)
        {
            EnableImg();
        }
        else
        {
            UnableImg();
        }
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

