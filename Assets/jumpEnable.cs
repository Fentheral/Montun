using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpEnable : abilityEnabler
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E) && hasCinematicStarted == false)
            {
                Debug.Log(1);
                StartCoroutine(FadeIn());
                Player.playerInstance.jumpEnabled = true;

            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && Player.playerInstance.jumpEnabled == true)
            {
                StartCoroutine(FadeOut());
            }
        }
    }
}

