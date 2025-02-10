using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOfVision : MonoBehaviour
{
    public bool inVision = false;

    void Start()
    {
        inVision = false;
       
    }
    private void Update()
    {
       
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            inVision = true;
            player.doTheySeeMe = true;


        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Patroller");

            foreach (GameObject obj in objects)
            {
                PatrollerTest patroller = obj.GetComponent<PatrollerTest>();

                if (patroller != null && patroller.onChase)
                {
                    inVision = true;
                }
            }
            Player player = collision.gameObject.GetComponent<Player>();

            inVision = false;
            player.doTheySeeMe = false;

        }
    }
}
