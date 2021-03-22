using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    Rigidbody rb;
    TrailRenderer tr;
    private bool hitSomething = false;
    public BoxCollider collider;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity / 10);
            rb.AddForce((Vector3.down * 1.81f), ForceMode.Acceleration);
        }
        else
            Stick();
    }
    private void Stick()
    {
        rb.constraints -= RigidbodyConstraints.FreezeAll;
        collider.enabled = false; 
    }
    private void OnCollisionEnter(Collision collision)
    { 
        if(collision.gameObject.tag != "Player")
        {
        hitSomething = true;
            rb.useGravity = false;
            tr.enabled = false;

        }
    }
}
