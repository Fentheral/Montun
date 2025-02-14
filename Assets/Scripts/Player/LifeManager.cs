using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LifeManager : MonoBehaviour
{
    public Image hpFillBar;
    public Animator animator;

    public RectTransform lifeUIRectTransform;
    public float scaleFactor = 0.9f;
    public static LifeManager Instance;
    [SerializeField] int life;
    public int currLife;


    //////////// Animaciones ////////////////
    public Animator enemyAnimator;
    public Animator playerAnimator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        currLife = life;
    }

    void Start()
    {
        RefreshBar();
    }

    private void Update()
    {
        if (currLife == 0)
        {
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Patroller")
        {
            enemyAnimator.SetBool("playerTrapped", true);
            playerAnimator.SetBool("isTrapped", true);
            PatrollerTest patroller = collision.gameObject.GetComponent<PatrollerTest>();
            LifeRed(patroller.damage);
            StunTrap stun = FindObjectOfType<StunTrap>().GetComponent<StunTrap>();
            stun.DoLogic();
        }
    }

    public void LifeRed(int dmg)
    {
        currLife -= dmg;
        if (currLife <= 0)
        {
            StartCoroutine(LoadGameOver());
        }
        RefreshBar();
    }

    public void LifeHeal(int heal)
    {
        if (currLife < 100)
        {
            currLife += heal;
        }
        RefreshBar();
    }

    public bool IsMaxHP()
    {
        if (currLife >= life)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private  IEnumerator LoadGameOver()
    {
        Player player = GetComponent<Player>();
        player.died = true;
        yield return new WaitForSeconds(3f);
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(4);

    }

    public void RefreshBar()
    {
       hpFillBar.fillAmount = (float)currLife / life;
    }
}
