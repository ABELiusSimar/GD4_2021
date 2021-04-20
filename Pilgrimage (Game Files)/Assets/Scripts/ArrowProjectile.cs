using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private GameObject arrowPrefab;
    public Transform projectileStart;
    private bool canShoot = false;
    private Animator animator;

    //Abraham Addition for sound
    private AudioSource audioSource;
    [SerializeField] private AudioClip ShootSound;           // the sound played when character leaves the ground.
    private float shootPitch;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        arrowPrefab = Resources.Load ("arrow") as GameObject;

        //Abraham addition for sound
        audioSource = GetComponent<AudioSource>();
        shootPitch = Random.Range(1f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Invoke("WillShoot", 0.5f);
            animator.SetBool("Charging", true);
            //Abraham addition for sound
            PlayArrowShotSound();
        }
        if (canShoot && Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("Shot");
            Shoot();
            Shot();
            //Abraham addition for sound
            PlayArrowShotSound();
        }
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            animator.SetTrigger("Shot");
            Shoot();
            Shot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("Charging", false);
            CancelInvoke("WillShoot");
        }
    }
    void WillShoot()
    {
        canShoot = true;
        
    }
    void Shot()
    {
        canShoot = false;
    }
    void Shoot()
    {
        GameObject newArrow = Instantiate(arrowPrefab, projectileStart.position, projectileStart.rotation);
        Rigidbody instFoamRB = newArrow.GetComponent<Rigidbody>();
        instFoamRB.AddRelativeForce(Vector3.forward * 50);
        instFoamRB.AddForce(Vector3.down * 30.81f, ForceMode.Acceleration);
    }

    private void PlayArrowShotSound()
    {
        audioSource.clip = ShootSound;
        //add jumpPitch here
        audioSource.pitch = shootPitch;
        audioSource.Play();
        //recalculate jumpPitch value
        shootPitch = Random.Range(1f, 1.5f);
    }
}
