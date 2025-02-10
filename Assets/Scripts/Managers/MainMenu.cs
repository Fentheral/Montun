using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    public void LoadScene(int id)
    {
        StartCoroutine(FadeOut(id));


    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator FadeOut(int id)
    {
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(id);
    }
}
