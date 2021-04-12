using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPuzzleCorrect : MonoBehaviour
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

    void OnTriggerEnter(Collider Box)
    {
        if(Box.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
            Instantiate(spawn, spawnPos.position, spawnPos.rotation);
        }
    }
}
