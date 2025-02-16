using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour,ICommand
{

    //SINGLETON
    public static Player playerInstance { get; private set; }

    // 


    public float AxV, AxH, speed, unabledSpeed, enabledSpeed, stunbreak;
    public bool isHiding, canMove, died, doTheySeeMe;
    public Rigidbody2D RB2D;
    public SpriteRenderer SR;
    Vector2 dir;
    public Color newColor;
    public GameObject bulletPrefab;
    Patroller patroller;

    /////////Varibles para el sigilo/////////
    public Color stealthColor;
    public bool stealthEnabled;
    public List<GameObject> Cones1;
    public float stealthCooldown;
    public float counter;
    /////////////////////////////////////////

    /////////Varibles para el escape/////////
    public GameObject scratch;
    public Transform scratchSpawn;
    /////////////////////////////////////////


    /////////Varibles para el Salto/////////
    public bool jumpEnabled;
    public float jumpCooldown;
    public float counterJump;

    /////////////////////////////////////////

    //////////// Animaciones ////////////////
    public Animator enemyAnimator;
    public Animator playerAnimator;
    [SerializeField] private string[] lastDir = new string[] { "Up","Down","Left","Right","UpLeft","UpRight","DownLeft","DownRight"} ;
    [SerializeField] private int lastInput;


    //////////// Sonidos ////////////////
    public delegate void CharacterSounds();
    public event CharacterSounds OnWalk;
    public event CharacterSounds OnStunBreak;
    public float walkCounter, footstepInterval;
  


    private void Awake()
    {
        if (playerInstance != null && playerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        playerInstance = this;



        died = false;
        canMove = true;
        RB2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        unabledSpeed = 0;
        enabledSpeed = speed;
        isHiding = false;
        stealthEnabled = false;
        counter = 20;
        counterJump = 10;
        doTheySeeMe = false;
    }

   

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }
    void Update()
    {
        counter += Time.deltaTime;
        counterJump+= Time.deltaTime;

        AxV = Input.GetAxisRaw("Vertical");
        AxH = Input.GetAxisRaw("Horizontal");
        if(AxH < 0)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }

        //Para el Idle, por ahora es provisional 
        if(AxH < 0 && AxV == 0)
        {
            lastInput = 3;
        }else if (AxH > 0 && AxV == 0)
        {
            lastInput = 2;
        }else if (AxH == 0 && AxV < 0)
        {
            lastInput = 1;
        }else if (AxH == 0 && AxV > 0)
        {
            lastInput = 0;
        }

        walkCounter += Time.deltaTime;

        playerAnimator.SetFloat("X",AxH);
        playerAnimator.SetFloat("Y",AxV);

        dir = new Vector2(AxH, AxV).normalized;
        //aca lo solucione gato
        BreakStun();

        Hide();
        StealthModeEnter();
        InputHandler.HandleInput(this, dir);
        Jump();


    }

    private void FixedUpdate()
    {
    }

    public void Walk(Vector2 direction)
    {
        if (died) return;

        dir = direction.normalized;

        if (dir != Vector2.zero)
        {
            if (walkCounter >= footstepInterval)
            {
                OnWalk?.Invoke();
                walkCounter = 0;
            }

            RB2D.AddForce(dir * speed * Time.deltaTime, ForceMode2D.Impulse);
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
            SetIdleAnimation();
        }
    }

    private void SetIdleAnimation()
    {
        switch (lastDir[lastInput])
        {
            case "Down":
                playerAnimator.SetFloat("IdleDir", 0);
                break;
            case "Up":
                playerAnimator.SetFloat("IdleDir", 1);
                break;
            case "Left":
                playerAnimator.SetFloat("IdleDir", 2);
                break;
            case "Right":
                playerAnimator.SetFloat("IdleDir", 3);
                break;
            case "UpLeft":
                playerAnimator.SetFloat("IdleDir", 4);
                break;
            case "UpRight":
                playerAnimator.SetFloat("IdleDir", 5);
                break;
            case "DownLeft":
                playerAnimator.SetFloat("IdleDir", 6);
                break;
            case "DownRight":
                playerAnimator.SetFloat("IdleDir", 7);
                break;
        }
    }

    public void Hide()
    {
        if (isHiding && died == false)
        {
           // hideCounter = 0;
            //hideCounter += Time.deltaTime;
            SR.enabled = false;
            speed = unabledSpeed;
        }
        else
        {
            if (canMove)
            {
                SR.enabled = true;
                speed = enabledSpeed;
            }
        }
    }
 
    public void BreakStun()
    {
        if (canMove == false && Input.GetKeyDown(KeyCode.E)&&died==false)
        {
            int percentage = Random.Range(0,101);
            if (stunbreak < 5)
            {
                OnStunBreak?.Invoke();
                stunbreak++;
                InstantiateScratches(percentage);
            }
            if (stunbreak == 5)
            {
                enemyAnimator.SetBool("playerTrapped", false);
                playerAnimator.SetBool("isTrapped", false);
                canMove = true;
                stunbreak = 0;
                GetComponent<SpriteRenderer>().color = newColor;
                speed = enabledSpeed;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
    }
    public void StealthModeEnter()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && counter >= stealthCooldown && canMove && stealthEnabled && died == false )
        {
            Cones1 = GameObject.FindGameObjectsWithTag("Normal Cone").ToList();
            foreach (GameObject cone in Cones1)
            {
                cone.GetComponent<SpriteRenderer>().enabled = false;
                cone.GetComponent<PolygonCollider2D>().enabled = false;
                this.GetComponent<SpriteRenderer>().color = stealthColor;
            }
            counter = 0;
            StartCoroutine(StealthModeExit());
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && counterJump >= jumpCooldown && canMove && jumpEnabled && died == false)
        {
            Debug.Log("presione Salto");
            counterJump = 0;
        }
    }
    IEnumerator StealthModeExit()
    {
        yield return new WaitForSeconds(6f);
        foreach (GameObject cone in Cones1)
        {
            cone.GetComponent<SpriteRenderer>().enabled = true;
            cone.GetComponent<PolygonCollider2D>().enabled = true;
            this.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        }
    }

    void InstantiateScratches(int percentage)
    {
        if(percentage > 50)
        {
            Instantiate(scratch, scratchSpawn);
        }
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }
}
/*ToDo
 *deshabilitar los controles cuando estas en cinematica
 *Bloquear los demas controles
 *Deshabilitar las "hidden walls"
 *1 Verificar si el player esta parado en un jump point 
 *1 Verificar si el player esta mirando en la direccion correcta
 *1 Transportarlo con la animacion de salto
 *2 Moverse en direccion a la cual estoy faceando
 *2 Chocar contra objetos solidos sin buggearse
 *Rehabilitar controles
 *
 Done
 *Iniciar cooldown
 *Verificar si tengo el cooldown disponible
 *Verificar si tengo la habilidad aprendida
 *Activar la habilidad de salto
 *Tomar el input
 
 */