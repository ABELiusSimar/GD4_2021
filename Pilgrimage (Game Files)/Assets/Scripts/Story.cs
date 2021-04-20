using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Event)
    {
        if(Event.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Instantiate(spawn, spawnPos.position, spawnPos.rotation);
        }
    }
}
