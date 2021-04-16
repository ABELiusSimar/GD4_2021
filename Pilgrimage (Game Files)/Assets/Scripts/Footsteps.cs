using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Footsteps : MonoBehaviour
{
    private PlayerMovement player;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip LandSound;           // the sound played when character touches back on ground.

    //add the vars for pitch change of footsteps, jump and landing sounds
    private float walkPitch;
    private float runningPitch;
    private float jumpPitch;
    private float landPitch;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        walkPitch = Random.Range(0.5f, 1.5f);
        runningPitch = Random.Range(1.5f, 2f);
        jumpPitch = Random.Range(0.5f, 1.5f);
        landPitch = Random.Range(0.25f, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        //Walking
        if(player.grounded == true && player.isMoving == true && audioSource.isPlaying == false && player.maxSpeed != 20f)
        {
            PlayFootStepAudio();
        }
        //Running
        else if(player.grounded == true && player.maxSpeed == 20f && audioSource.isPlaying == false)
        {
            PlayRunningAudio();
        }
        //Jumping
        else if(player.grounded == false && player.jumping == true)
        {
            PlayJumpSound();
        }
        //Landing
        else if(player.grounded == true && player.land == false)
        {
            PlayLandingSound();
            player.land = true;
        }
        //Sliding
        else if(player.crouching == true && player.grounded == true)
        {
            //Still working on it
            PlayFootStepAudio();
        }
    }

    private void PlayFootStepAudio()
    {
        if (!player.grounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, FootstepSounds.Length);
        audioSource.clip = FootstepSounds[n];
        //add walkPitch value here
        audioSource.pitch = walkPitch;
        audioSource.PlayOneShot(audioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        FootstepSounds[n] = FootstepSounds[0];
        FootstepSounds[0] = audioSource.clip;
    }

    private void PlayRunningAudio()
    {
        if (!player.grounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, FootstepSounds.Length);
        audioSource.clip = FootstepSounds[n];
        //add walkPitch value here
        audioSource.pitch = runningPitch;
        audioSource.PlayOneShot(audioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        FootstepSounds[n] = FootstepSounds[0];
        FootstepSounds[0] = audioSource.clip;
    }

    private void PlayJumpSound()
    {
        audioSource.clip = JumpSound;
        //add jumpPitch here
        audioSource.pitch = jumpPitch;
        audioSource.Play();
        //recalculate jumpPitch value
        jumpPitch = Random.Range(0.5f, 1.5f);
    }

    private void PlayLandingSound()
    {
        audioSource.clip = LandSound;
        //add landPitch here
        audioSource.pitch = landPitch;
        audioSource.Play();
        //recalculate landPitch value
        landPitch = Random.Range(0.25f, 1.25f);
    }

    private void PlaySlideSound()
    {
        //Still working on it
    }
}
