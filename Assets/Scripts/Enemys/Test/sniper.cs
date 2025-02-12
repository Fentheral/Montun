using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    private bool _canSeePlayer;
    [SerializeField] float maxDistance;
    public LayerMask obstructionMask;
    [SerializeField] Transform _playerPosition;

    void Start()
    {
        _canSeePlayer = false;
    }

    void Update()
    {
        CanSeePlayer();
    }

    private bool CanSeePlayer()
    {
        Vector2 direction = _playerPosition.transform.position - transform.position;
        Debug.LogError(direction.magnitude);

        // Lanzar el Raycast2D
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, maxDistance, obstructionMask);
        // Verificar si el primer objeto que choca es el jugador
        if (hit.collider!=null)
        {
            Debug.Log("Raycast Hit: " + hit.collider.name);
            Debug.Log("no lo veo");
            
            return false;
        }
        if (direction.magnitude <= maxDistance)
        {
            Debug.Log("lo veo");

            return true;
        }
        else
        {
            Debug.Log("no lo veo");

            return false;
        }
    }

    // Dibuja un Gizmo en la escena para visualizar el Raycast2D
    private void OnDrawGizmos()
    {
        if (_playerPosition == null) return;

        Gizmos.color = Color.red; // Color cuando no ve al jugador
        if (_canSeePlayer)
        {
            Gizmos.color = Color.green; // Verde cuando ve al jugador
        }

        // Dibujar la línea del Raycast2D
        Vector2 direction = _playerPosition.transform.position - transform.position;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction.normalized * maxDistance));
    }
}