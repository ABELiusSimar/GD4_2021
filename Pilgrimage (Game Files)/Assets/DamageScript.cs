using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{

    [SerializeField]
    private float HP = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage()
    {
        HP -= 1;
        if (HP<0)
        {
            HP = 0;
        }
    }
    public void RestoreHealth()
    {

    }

    private void Die()
    {

    }

    private void Respawn()
    {

    }
}
