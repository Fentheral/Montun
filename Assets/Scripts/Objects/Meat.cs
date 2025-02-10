using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    LifeManager lm;
    void Start()
    {
        lm = FindObjectOfType<LifeManager>().GetComponent<LifeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            if (!lm.IsMaxHP())
            {
                lm.LifeHeal(10);
                Destroy(this.gameObject);
            }
        }
    }
}
