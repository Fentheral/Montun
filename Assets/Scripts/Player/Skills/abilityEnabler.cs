using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public abstract class abilityEnabler : MonoBehaviour
{
    Player player;
    public bool canInteract;
    public Animator animator;
    public bool hasFadeOutEnded;
    public List<GameObject> comicPage;
    public int counter;
    public float cooldown = 2f;
    public float timeCounter;
    public bool hasCinematicStarted;





    //UI
    UiText interactUi;



    public void Awake()
    {
        hasCinematicStarted = false;
        timeCounter = cooldown;
        counter = 0;
        hasFadeOutEnded = false;
        comicPage = GameObject.FindGameObjectsWithTag("Cinematic").ToList();
        player = FindObjectOfType<Player>().GetComponent<Player>();
        interactUi = FindObjectOfType<UiText>().GetComponent<UiText>();
        
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            canInteract = true;
            interactUi.EnableImg();
            interactUi.uiText.text = "Presiona [E] para liberar al animal";
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            canInteract = false;
            interactUi.UnableImg();
            interactUi.uiText.text = "";
        }
    }

    public IEnumerator FadeIn()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Patroller");

        foreach (GameObject objeto in objetos)
        {
            objeto.GetComponent<PatrollerTest>().paused = true; 
        }
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
        yield return new WaitForSeconds(1f);
        comicPage[0].GetComponent<Image>().enabled = true;
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true);
        player.isHiding = true;
        hasCinematicStarted = true;

    }
    public IEnumerator FadeOut()
    {
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
        yield return new WaitForSeconds(1f);
        comicPage[0].GetComponent<Image>().enabled = false;
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Patroller");

        foreach (GameObject objeto in objetos)
        {
            objeto.GetComponent<PatrollerTest>().paused = false;
        }
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true);
        player.isHiding = false;

        Destroy(gameObject);
    }
}
