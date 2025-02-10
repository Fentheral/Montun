using UnityEngine;

public class CircleSize : MonoBehaviour
{
    public Transform parent;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Obtener el valor de searchRadius del padre
        float searchRadius = parent.GetComponent<PatrollerTest>().searchRadius;

        // Escalar el círculo hijo en función del searchRadius
        float scale = searchRadius * 2f; // Ajusta el valor 2f según tus necesidades
        transform.localScale = new Vector3(scale, scale, 1f);

        
    }
}