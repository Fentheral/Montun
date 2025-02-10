using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrap : Trap
{


    public int dmg;

    UiText interactUi;

    private void Awake()
    {
    }

    public override void DoLogic()
    {
        Player player = FindObjectOfType<Player>().GetComponent<Player>();
        player.stunbreak = 0;
        player.canMove = false;
        player.speed = player.unabledSpeed;
        LifeManager lm = FindObjectOfType<LifeManager>().GetComponent<LifeManager>();
        lm.LifeRed(dmg);
    }
}
