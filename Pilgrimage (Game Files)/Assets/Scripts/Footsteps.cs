using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private PlayerMovement player;
    private AudioSource footsteps;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        footsteps = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.grounded == true && player.isMoving == true && footsteps.isPlaying == false && player.maxSpeed != 20f)
        {
            footsteps.pitch = Random.Range(0.8f, 1f);
            footsteps.Play();
        }
        else if(player.grounded == true && player.maxSpeed == 20f && footsteps.isPlaying == false)
        {
            footsteps.pitch = Random.Range(1.2f, 1.5f);
            footsteps.Play();
        }
    }
}
