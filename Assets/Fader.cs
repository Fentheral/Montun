using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;



public class Fader : MonoBehaviour
{
    public int counter;
    private float cooldown = 2f;
    private float timeCounter;
    public Animator animator;
    public bool hasFadeOutEnded;
    public List<GameObject> comicPage;
    Player player;


    // Start is called before the first frame update
    void Start()
    {
        timeCounter = cooldown;
        counter = 0;
        hasFadeOutEnded = false;
        comicPage = GameObject.FindGameObjectsWithTag("Cinematic").ToList();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public IEnumerator FadeIn()
    {
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
        yield return new WaitForSeconds(1f);
        comicPage[0].GetComponent<Image>().enabled = true;
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true);
    }
    public IEnumerator FadeOut()
    {        
            animator.SetBool("FadeOut", true);
            animator.SetBool("FadeIn", false);
            yield return new WaitForSeconds(1f);
            comicPage[0].GetComponent<Image>().enabled = false;
            animator.SetBool("FadeOut", false);
            animator.SetBool("FadeIn", true);  
    }

    public void ResumetGame()
    {

    }
}
