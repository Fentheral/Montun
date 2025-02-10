using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesUi : MonoBehaviour
{
    public Image img;
    public TMP_Text uiText;
    Player playerAbilities;

    private void Awake()
    {
        playerAbilities = FindObjectOfType<Player>().GetComponent<Player>();
    }

    private void Update()
    {
        if (playerAbilities.stealthEnabled)
        {
            if (playerAbilities.counter <= playerAbilities.stealthCooldown)
            {
                EnableImg();
                uiText.text = playerAbilities.counter.ToString("F0");
            }
            else
            {
                UnableImg();
                uiText.text = "";
            }
        }
        else
        {
            EnableImg();
            uiText.text = "";
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
