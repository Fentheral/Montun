using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    public float movespeed;
    public float currentSpeed;
    public float paralizedSpeed;
    public bool paralized;
    Transform target;
    Transform impactTarget;
    Vector2 moveDirection;
    public int position2;
    public bool inVision;
    public GameObject[] patrolPoints;
    public float time;
    public int damage;
    public float speedRotationLook;
    public bool lookingAtTarget;
    public float speed;
    public int pointIndex;
    public float speedRotation;
    public bool reachedTarget;
    public bool pushed;

    Player player;

    ///////// Anims
    public Animator enemyAnimator;

    void Start()
    {
        pushed = false;
        reachedTarget = false;
        lookingAtTarget = false;
        pointIndex = 1;
        position2 = 0;
        currentSpeed = movespeed;
        target = patrolPoints[pointIndex].transform;
        paralized = false;
        player = FindObjectOfType<Player>().GetComponent<Player>();
    }
    private void Update()
    {
        if (!paralized)
        {
            InVisionCheck();
        }
        else
        {
            StartCoroutine(Untrap());
        }
        Push();
    }



    public void Patrol()
    {

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        Vector3 dir = patrolPoints[position2].transform.position - transform.position;

        if (dir != Vector3.zero)
        {
            enemyAnimator.SetBool("isMoving", true);

            float distanceXDiff = Mathf.Abs(transform.position.x - target.transform.position.x);
            float distanceYDiff = Mathf.Abs(transform.position.y - target.transform.position.y);

            if (distanceXDiff > distanceYDiff)
            {
                enemyAnimator.SetFloat("Y", 0);
                if (target.position.x < transform.position.x)
                {
                    enemyAnimator.SetFloat("X", -1);
                }
                else
                {
                    enemyAnimator.SetFloat("X", 1);
                }
            }
            else if (distanceXDiff < distanceYDiff)
            {
                enemyAnimator.SetFloat("X", 0);
                if (target.position.y < transform.position.y)
                {
                    enemyAnimator.SetFloat("Y", -1);
                }
                else
                {
                    enemyAnimator.SetFloat("Y", 1);
                }
            }
        }
    }


    public void InVisionCheck()
    {
        ConeOfVision isVisible = GetComponentInChildren<ConeOfVision>();

        inVision = isVisible.inVision;


        if (inVision == false)
        {
            if (lookingAtTarget == false)
            {
                StartCoroutine(LookAround());
            }
            if (lookingAtTarget == true)
            {
                Patrol();
            }
            GetNewTarget();
        }
        if (inVision == true)
        {
            target = player.transform;
            Chase();
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StunTrap")
        {

            currentSpeed = paralizedSpeed;
            paralized = true;
        }

        if (collision.gameObject.tag == "Player")
        {
        }
    }

    public void Chase()
    {
        if (player.isHiding)
        {
            target = null;
            Patrol();
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (target)
        {
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;
            Quaternion lookAt = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, speedRotation * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if (dir != Vector3.zero)
            {
                enemyAnimator.SetBool("isMoving", true);

                float distanceXDiff = Mathf.Abs(transform.position.x - target.transform.position.x);
                float distanceYDiff = Mathf.Abs(transform.position.y - target.transform.position.y);

                if (distanceXDiff > distanceYDiff)
                {
                    enemyAnimator.SetFloat("Y", 0);
                    if (target.position.x < transform.position.x)
                    {
                        enemyAnimator.SetFloat("X", -1);
                    }
                    else
                    {
                        enemyAnimator.SetFloat("X", 1);
                    }
                }
                else if (distanceXDiff < distanceYDiff)
                {
                    enemyAnimator.SetFloat("X", 0);
                    if (target.position.y < transform.position.y)
                    {
                        enemyAnimator.SetFloat("Y", -1);
                    }
                    else
                    {
                        enemyAnimator.SetFloat("Y", 1);
                    }
                }
            }
        }
    } 
    IEnumerator Untrap()
    {
        yield return new WaitForSeconds(2f);
        currentSpeed = movespeed;
        paralized = false;
    }
    IEnumerator LookAround()
    {
        target = patrolPoints[pointIndex].transform;
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;
        Quaternion lookAt = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, speedRotationLook * Time.deltaTime);
        yield return new WaitForSeconds(1.5f);
        lookingAtTarget = true;

    }
    public void GetNewTarget()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < 0.01f)
        {
            lookingAtTarget = false;
            if (pointIndex == 0)
            {
                pointIndex = 1;
            }
            else if (pointIndex == 1)
            {
                pointIndex = 0;
            }

        }
        if (target == player.transform)
        {
            lookingAtTarget = false;
            StartCoroutine(LookAround());
        }
    }
    public void Push()
    {
        if (pushed == true)
        {
            BehaviourBullet push = FindObjectOfType<BehaviourBullet>().GetComponent<BehaviourBullet>();
            Vector2 difference = (transform.position - push.transform.position).normalized;
            transform.position = new Vector2(transform.position.x + difference.x * 3, transform.position.y + difference.y * 3);
            paralized = true;
            pushed = false;
        }

    }
}
