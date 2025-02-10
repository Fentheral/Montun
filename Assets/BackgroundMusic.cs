using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;

    private AudioSource audioSource;
    private bool isFadingOut = false;

    private void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Establecer la canción de fondo
        audioSource.clip = backgroundMusic;

        // Iniciar la reproducción de la canción de fondo
        audioSource.Play();

        // Realizar fade-in
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        // Verificar si se debe realizar fade-out
        if (isFadingOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        // Establecer el volumen inicial a cero
        audioSource.volume = 0f;

        // Incrementar gradualmente el volumen hasta llegar a 1
        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime / fadeInDuration;
            yield return null;
        }

        // Asegurarse de que el volumen sea exactamente 1 al final del fade-in
        audioSource.volume = 1f;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        // Decrementar gradualmente el volumen hasta llegar a cero
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        // Detener la reproducción y restablecer el volumen a cero
        audioSource.Stop();
        audioSource.volume = 0f;
    }

    private void OnDestroy()
    {
        // Realizar fade-out al salir de la escena
        isFadingOut = true;
    }
}