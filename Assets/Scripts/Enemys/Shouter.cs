using System.Collections.Generic;
using UnityEngine;

public class Shouter : MonoBehaviour
{
    public float searchRadius = 10f;
    public LayerMask targetLayer;

    public  List<GameObject> collectedObjects = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CollectObjectsInRadius();
            PrintCollectedObjects();
        }
    }

    private void CollectObjectsInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, targetLayer);

        collectedObjects.Clear();

        foreach (Collider2D collider in colliders)
        {
            GameObject obj = collider.gameObject;
            collectedObjects.Add(obj);
        }
    }

    private void PrintCollectedObjects()
    {
        Debug.Log("Objects within radius:");

        foreach (GameObject obj in collectedObjects)
        {
            Debug.Log(obj.name);
        }
    }
}