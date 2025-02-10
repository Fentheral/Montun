using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FBXManager : MonoBehaviour
{
    public List<AudioClip> musicList;
    //public List<GameObject> bushes;

    // Start is called before the first frame update
    private void Start()
    {
        Player player = FindObjectOfType<Player>().GetComponent<Player>();

       // bushes = GameObject.FindGameObjectsWithTag("Bush").ToList();

        if (player != null)
        {
            player.OnWalk += PlayWalkSound;
            player.OnStunBreak += PlayStunBreakSound;
        }
           }


    private void PlayWalkSound()
    {
        AudioSource.PlayClipAtPoint(musicList[0], transform.position);
    }
    private void PlayStunBreakSound()
    {
        AudioSource.PlayClipAtPoint(musicList[1], transform.position);
    }
    

}
