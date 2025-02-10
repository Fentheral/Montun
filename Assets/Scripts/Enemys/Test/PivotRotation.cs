using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.z = 0f; // Mantener la rotaci�n solo en el eje Z
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calcular el �ngulo en grados
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward); // Crear la rotaci�n en base al �ngulo
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
