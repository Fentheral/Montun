using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    Player player;
    public bool canInteract;
    int interactTime = 0;
    public ParticleSystem hideParticles;
    UiText interactUi;
    Transform onStartPos;
    public delegate void CharacterSounds();
    public event CharacterSounds OnHide;
    public List<AudioClip> musicList;

    private void Awake()
    {
        interactUi = FindObjectOfType<UiText>().GetComponent<UiText>();
        onStartPos = this.transform;
    }

    private void Update()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();

        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E) && player.doTheySeeMe == false)
            {
                AudioSource.PlayClipAtPoint(musicList[0],transform.position);
                OnHide?.Invoke();
                StartCoroutine(Shake());
                hideParticles.Play();
                if (interactTime == 0)
                {
                    player.isHiding = true;
                    GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

                    foreach (GameObject obj in objects)
                    {
                        PatrollerTest patroller = obj.GetComponent<PatrollerTest>();
                        if (patroller != null)
                        {
                            patroller.onChase = false;
                        }
                    }

                    interactUi.uiText.text = "Presiona [E] para salir del escondite";
                    interactTime = 1;
                }else if(interactTime == 1)
                {
                    player.isHiding = false;
                    interactUi.uiText.text = "Presiona [E] para esconderte";
                    interactTime = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            canInteract = true;
            interactUi.EnableImg();
            interactUi.uiText.text = "Presiona [E] para esconderte";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            canInteract = false;
            interactUi.UnableImg();
            interactUi.uiText.text = "";
        }
    }

    private IEnumerator Shake()
    {
        this.transform.position = new Vector3(this.transform.position.x + 0.1f, this.transform.position.y, this.transform.position.z);
        yield return new WaitForSeconds(0.2f);
        this.transform.position = new Vector3(this.transform.position.x - 0.1f, this.transform.position.y + 0.1f, this.transform.position.z);
        yield return new WaitForSeconds(0.2f);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z + 0.1f);
        yield return new WaitForSeconds(0.2f);
        this.transform.position = onStartPos.position;
    }
}
