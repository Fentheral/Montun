using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stealthEnabler : abilityEnabler
{
    // Start is called before the first frame update
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

                StartCoroutine(FadeIn());
                Player.playerInstance.stealthEnabled = true;
                Player.playerInstance.canMove = false;



            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && Player.playerInstance.stealthEnabled == true)
            {
                StartCoroutine(FadeOut());
                Player.playerInstance.canMove = true;

            }
        }
    }
}
