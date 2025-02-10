using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneFader : MonoBehaviour
{
    public List<GameObject> comicPage;
    // Start is called before the first frame update
    public Animator animator;
    public bool hasFadeOutEnded;
    public int counter;
    private float cooldown = 2f;
    private float timeCounter;

    private void Awake()
    {
        timeCounter = cooldown;
        counter = 0;
        hasFadeOutEnded = false;
        comicPage = GameObject.FindGameObjectsWithTag("Cinematic").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0))

        {
            if (hasFadeOutEnded == false)
            {
                if (cooldown <= timeCounter)
                {
                    counter++;
                    timeCounter = 0;
                    StartCoroutine(FadeOut());
                }

            }
        }
        if (hasFadeOutEnded == true)
        {
            StartCoroutine(FadeIn());

            print(counter);
            StartGame();
        }
        StartCoroutine(Skip());
    }
    public IEnumerator FadeOut()
    {
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);

        yield return new WaitForSeconds(1f);
        hasFadeOutEnded = true;
        NextStripe();

    }
    public IEnumerator FadeIn()
    {
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(1f);
        hasFadeOutEnded = false;

    }
    public void NextStripe()
    {

        comicPage[comicPage.Count - counter].GetComponent<Image>().enabled = false;
    }
    public void StartGame()
    {
        if (counter == comicPage.Count)
        {
            SceneManager.LoadScene(2);
        }
    }
    public IEnumerator Skip()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("FadeOut", true);
            animator.SetBool("FadeIn", false);

            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(2);

        }
    }
}