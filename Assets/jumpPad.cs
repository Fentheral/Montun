using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] Transform landingPad;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player.playerInstance.cinematicJumpCoroutine == null) // Asegúrate de que el Player tiene este tag
        {
            Player.playerInstance.cinematicJump = true;
            Player.playerInstance.targetPosition = landingPad;
           

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&Player.playerInstance.cinematicJumpCoroutine==null)
        {
            Player.playerInstance.cinematicJump = false;
            Player.playerInstance.targetPosition = null;
        }
        else
        {
            Player.playerInstance.targetPosition = landingPad;

        }
    }
}
