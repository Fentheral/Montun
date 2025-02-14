using System.Collections;
using UnityEngine;

public class Sniper : MonoBehaviour, ISnipe
{
    private bool _canSeePlayer;
    private bool _isAiming;

    [SerializeField] float maxDistance;
    public LayerMask obstructionMask;
    [SerializeField] Transform _playerPosition;
    [SerializeField] Player player;

    [SerializeField] float _aimTime;
    private SpriteRenderer _bullsEyeImage;

    private void Awake()
    {
        _bullsEyeImage = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _bullsEyeImage.enabled = false;
        _canSeePlayer = false;
        _isAiming = false;
    }

    void Update()
    {
        _canSeePlayer = CanSeePlayer();  
        Snipe();
    }

    private bool CanSeePlayer()
    {
        Vector2 direction = _playerPosition.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, maxDistance, obstructionMask);

        if (hit.collider != null)
        {
            return false;
        }

        return direction.magnitude <= maxDistance;
    }

    public void Snipe()
    {
        if (_canSeePlayer && !_isAiming)
        {
            _bullsEyeImage.enabled = true;
            StartCoroutine(Aim());
        }
        else if (!_canSeePlayer)
        {
            StopCoroutine(Aim());
            _bullsEyeImage.enabled = false;
            _isAiming = false; 
        }
    }

    IEnumerator Aim()
    {
        _isAiming = true;

        yield return new WaitForSeconds(_aimTime);

        if (_canSeePlayer)
        {
            AudioManager.Instance.PlaySFX("GunShot");
            DamagePlayer();
            yield return new WaitForSeconds(AudioManager.Instance.GetClipLength("GunShot"));
            AudioManager.Instance.PlaySFX("Reload");
        }

        _isAiming = false; 
    }

    private void DamagePlayer() {
        Debug.Log(LifeManager.Instance.currLife);

        LifeManager.Instance.currLife -=50;
        LifeManager.Instance.RefreshBar();

        Debug.Log(LifeManager.Instance.currLife);
    
    }
}



//TODO
//4.2 activar particula

//DONE
//4.1 play sonido
//4.3 apagar mira
//4.4 hacer danio
//4.5 hacer ruido de recarga
//1 detectar al player
//2 empezar a contar el tiempo
//3 instanciar la mira
//4 si el tiempo se cumple y el player sigue en la mira
//5 si el player sale de la vista del sniper
//5.1 apagar la mira
//6 terminar el conteo