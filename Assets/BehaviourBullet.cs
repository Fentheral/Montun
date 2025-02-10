using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourBullet : MonoBehaviour
{
    public float lifeTime;
    private GameObject coll;

    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Patroller")
        {
            GameObject coll = collision.gameObject;
            PatrollerTest state = coll.GetComponent<PatrollerTest>();
            state.pushed = true;
            print("pase");
        }
    }
    

}