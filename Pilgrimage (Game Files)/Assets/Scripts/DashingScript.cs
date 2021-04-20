using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingScript : MonoBehaviour
{
    public float dashSpeed;
    public PlayerMovement PlayerMovement;
    Rigidbody rg;
    bool isDashing;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    public float dashDuration = 2f;
    private bool canDash;

    public GameObject dashEffect;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            Dashing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isDashing = true;
        }
        if (PlayerMovement.grounded || PlayerMovement.isWallRunning)
        {
            canDash = true;
        }
    }




    //GameObject effect = Instantiate(dashEffect, Camera.main.transform.position, dashEffect.transform.rotation);
    //effect.transform.parent = Camera.main.transform;
    //    effect.transform.LookAt(transform);
    private void Dashing()
    {
        if (canDash)
        {
        rg.velocity = Vector3.zero;
        rg.AddForce(Camera.main.transform.forward * dashSpeed, ForceMode.Impulse);
        isDashing = false;
        canDash = false;
        }
        



    }
    private void CancelDashing()
    {
        rg.velocity = Vector3.zero;
    }
    
}
