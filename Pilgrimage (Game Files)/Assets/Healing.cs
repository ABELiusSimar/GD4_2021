using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healing : MonoBehaviour
{
    public Text HealText;
    public Text cooldownText;
    public DamageScript healing;

    public bool canHeal;

    public bool cooldown = false;
    public float CooldownLeft = 30;
    public float nextCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        HealText.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextCooldown)
        {
            cooldown = false; 
            if (Input.GetKeyDown(KeyCode.E))
            {

                healing.RestoreHealth();
                cooldown = true;
            nextCooldown += CooldownLeft;
            }

        }
        else
        {
            cooldownText.text = (nextCooldown - (int)Time.time)+ " Seconds Cooldown";
        }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!cooldown)
            {
                HealText.gameObject.SetActive(true);
                cooldownText.gameObject.SetActive(false);

            }
            else
            {
                HealText.gameObject.SetActive(false);
                cooldownText.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            HealText.gameObject.SetActive(false);
            cooldownText.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!cooldown)
            {
                HealText.gameObject.SetActive(true);
                cooldownText.gameObject.SetActive(false);

            }
            else
            {
                HealText.gameObject.SetActive(false);
                cooldownText.gameObject.SetActive(true);
            }
        }
    }
}
