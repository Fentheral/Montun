using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerTest : MonoBehaviour
{
    public Transform[] waypoints;
    public float movementSpeed = 5f;
    public float waitTime = 2f;

    private int currentWaypointIndex = 0;
    private Transform currentWaypoint, lastWayPoint;
    private bool isWaiting = false;
    public int damage;
    public bool pushed, paralized, paused;
    private float lastSpeed;
    public float searchRadius;
    public LayerMask targetLayer;
    public bool onChase;
    public List<GameObject> collectedObjects = new List<GameObject>();
    Shout shout;
    private Coroutine untrapCoroutine;
    public delegate void EnemySounds();
    public event EnemySounds OnShout;
    public List<AudioClip> musicList;
    public float shoutCounter, shoutInverval;
    public bool isPlaying;

   



    private bool isColliding = false;
    private float collisionResetDelay = 0.5f; // Tiempo de retraso antes de restablecer la colisión

    //Anims
    public Animator patrollerAnims;
    public SpriteRenderer SR;

    private void Start()
    {

        paused = false;
        shout = FindObjectOfType<Shout>().GetComponent<Shout>();
        onChase = false;
        lastSpeed = movementSpeed;
        paralized = false;
        pushed = false;
        SetNextWaypoint();
    }

    private void FixedUpdate()
    {
        isPlaying = false;
        shoutCounter += Time.deltaTime;

        Paralisis(paralized);
        Push();
        HidingVerifier();

        if (isColliding)
        {
            movementSpeed = 0;
            // Realiza las acciones adicionales necesarias cuando el objeto está colisionando
            StartCoroutine(ResetCollision()); // Restablecer la colisión después de un tiempo
            return; // Salir del método Update para evitar que el objeto se mueva mientras está colisionando
        }
        if (paused == false) 
        {
            if (paralized)
            {
                patrollerAnims.SetBool("isMoving", false);
                StartCoroutine(Untrap());

                untrapCoroutine = StartCoroutine(Untrap());
            }
            if(untrapCoroutine!=null)
            {
                StopCoroutine(Untrap());
                untrapCoroutine = null;
            }
            else
            {
                if (currentWaypoint != null)
                {
                    ConeOfVision isVisible = GetComponentInChildren<ConeOfVision>();


                    lastWayPoint = currentWaypoint;
                    if (isVisible.inVision == true)
                    {
                        Chase();
                    }
                    else
                    {
                        GetNextWaypoint();
                        SetNextWaypoint();


                        if (!isWaiting)
                        {
                            patrollerAnims.SetBool("isMoving", true);
                            patrollerAnims.SetBool("isRotating", false);
                            PivotRotation pivotRotation = transform.GetChild(0).GetComponent<PivotRotation>();
                            if (pivotRotation != null)
                            {
                                pivotRotation.target = currentWaypoint;
                            }
                            // Mover hacia el waypoint actual
                            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, movementSpeed * Time.deltaTime);

                            float distanceXDiff = Mathf.Abs(transform.position.x - currentWaypoint.transform.position.x);
                            float distanceYDiff = Mathf.Abs(transform.position.y - currentWaypoint.transform.position.y);

                            if (distanceXDiff > distanceYDiff)
                            {
                                patrollerAnims.SetFloat("Y", 0);
                                if (currentWaypoint.position.x < transform.position.x)
                                {
                                    patrollerAnims.SetFloat("X", -1);
                                }
                                else
                                {
                                    patrollerAnims.SetFloat("X", 1);
                                }
                            }
                            else if (distanceXDiff < distanceYDiff)
                            {
                                patrollerAnims.SetFloat("X", 0);
                                if (currentWaypoint.position.y < transform.position.y)
                                {
                                    patrollerAnims.SetFloat("Y", -1);
                                }
                                else
                                {
                                    patrollerAnims.SetFloat("Y", 1);
                                }
                            }

                            // Verificar si hemos llegado al waypoint actual
                            if (transform.position == currentWaypoint.position)
                            {
                                patrollerAnims.SetBool("isRotating", true);

                                float distanceXDiffForRot = Mathf.Abs(transform.position.x - currentWaypoint.transform.position.x);
                                float distanceYDiffForRot = Mathf.Abs(transform.position.y - currentWaypoint.transform.position.y);

                                if (distanceXDiffForRot > distanceYDiffForRot)
                                {
                                    patrollerAnims.SetFloat("Y", 0);
                                    if (currentWaypoint.position.x < transform.position.x)
                                    {
                                        patrollerAnims.SetFloat("X", -1);
                                    }
                                    else
                                    {
                                        patrollerAnims.SetFloat("X", 1);
                                    }
                                }
                                else if (distanceXDiffForRot < distanceYDiffForRot)
                                {
                                    patrollerAnims.SetFloat("X", 0);
                                    if (currentWaypoint.position.y < transform.position.y)
                                    {
                                        patrollerAnims.SetFloat("Y", -1);
                                    }
                                    else
                                    {
                                        patrollerAnims.SetFloat("Y", 1);
                                    }
                                }

                                // Iniciar tiempo de espera
                                StartCoroutine(WaitAtWaypoint());

                                // Obtener el componente PivotRotation y asignar el próximo waypoint como objetivo
                                if (pivotRotation != null)
                                {
                                    pivotRotation.target = GetNextWaypoint();
                                }
                            }
                        }
                    }
                }
            }
        }
        

    }
    private Transform GetNextWaypoint()
    {
        int nextIndex = currentWaypointIndex + 1;
        if (nextIndex >= waypoints.Length)
        {
            nextIndex = 0;
        } 
        print(this.name+" "+waypoints[nextIndex]);
        return waypoints[nextIndex];
    }

    private void SetNextWaypoint()
    {
        if (waypoints.Length > 0)
        {
            currentWaypoint = waypoints[currentWaypointIndex];
        }
    }

    private void Chase()
    {
        patrollerAnims.SetBool("isMoving", true);

        currentWaypoint = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, movementSpeed * Time.deltaTime);
        PivotRotation pivotRotation = transform.GetChild(0).GetComponent<PivotRotation>();
        if (pivotRotation != null)
        {
            pivotRotation.target = currentWaypoint;
        }

        float distanceXDiff = Mathf.Abs(transform.position.x - currentWaypoint.transform.position.x);
        float distanceYDiff = Mathf.Abs(transform.position.y - currentWaypoint.transform.position.y);

        if (distanceXDiff > distanceYDiff)
        {
            patrollerAnims.SetFloat("Y", 0);
            if (currentWaypoint.position.x < transform.position.x)
            {
                patrollerAnims.SetFloat("X", -1);
            }
            else
            {
                patrollerAnims.SetFloat("X", 1);
            }
        }
        else if (distanceXDiff < distanceYDiff)
        {
            patrollerAnims.SetFloat("X", 0);
            if (currentWaypoint.position.y < transform.position.y)
            {
                patrollerAnims.SetFloat("Y", -1);
            }
            else
            {
                patrollerAnims.SetFloat("Y", 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isColliding = true;
        }
        if (collision.gameObject.tag == "StunTrap")
        {
            paralized = true;
        }
    }
    public void Paralisis(bool paralisis)
    {
        if (paralisis == true) 
        {
            movementSpeed = 0;

        }
    }

    private IEnumerator ResetCollision()
    {
        yield return new WaitForSeconds(collisionResetDelay);
        isColliding = false;
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        SetNextWaypoint();
    }

    public void Push()
    {
        if (pushed == true)
        {
            BehaviourBullet push = FindObjectOfType<BehaviourBullet>().GetComponent<BehaviourBullet>();
            Vector2 difference = (transform.position - push.transform.position).normalized;
            transform.position = new Vector2(transform.position.x + difference.x * 8, transform.position.y + difference.y * 8);
            paralized = true;
            pushed = false;
            movementSpeed = 0;
        }
    }

    IEnumerator Untrap()
    {
        if (isPlaying != true)
        {
            isPlaying = true;
            if (shoutCounter > shoutInverval)
            {
               
                AudioSource.PlayClipAtPoint(musicList[0], transform.position);
                shoutCounter = 0;
                shoutCounter += Time.deltaTime; shout.EnableImg();
                yield return new WaitForSeconds(2f);
                Shout();
                movementSpeed = 5f; // Restablecer la velocidad original
                paralized = false;
            }
            
            isPlaying = false;
        }
       

    }
    private void Shout()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, targetLayer);

        collectedObjects.Clear();

        foreach (Collider2D collider in colliders)
        {
            GameObject obj = collider.gameObject;
            collectedObjects.Add(obj);

        }
        foreach (GameObject item in collectedObjects)
        {
            item.GetComponentInChildren<ConeOfVision>().inVision = true;
            onChase = true;
            shout.UnableImg();
        }

    }
    public void HidingVerifier()
    {
        Player player = FindObjectOfType<Player>().GetComponent<Player>();
        if (player.isHiding == true)
        {
            foreach (GameObject item in collectedObjects)
            {
                item.GetComponentInChildren<ConeOfVision>().inVision = false ;
            }
        }
    }
    
} 


