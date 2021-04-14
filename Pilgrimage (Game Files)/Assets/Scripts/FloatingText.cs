using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Transform spawnPos;
    public Transform spawnPos2;
    public GameObject spawn;
    public GameObject spawn2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Box)
    {
        if(Box.gameObject.tag == "Player" || Box.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
            Instantiate(spawn, spawnPos.position, spawnPos.rotation);
            Instantiate(spawn2, spawnPos2.position, spawnPos.rotation);
        }
    }
}
