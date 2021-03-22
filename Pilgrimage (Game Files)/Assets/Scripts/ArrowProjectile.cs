using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private GameObject arrowPrefab;
    public Transform projectileStart;
    private bool canShoot = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        arrowPrefab = Resources.Load ("arrow") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Invoke("WillShoot", 0.5f);
            animator.SetBool("Charging", true);




        }
        if (canShoot && Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("Shot");
            Shoot();
            Shot();
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
}
