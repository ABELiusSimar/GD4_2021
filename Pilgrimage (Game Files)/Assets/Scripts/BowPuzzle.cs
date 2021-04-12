using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPuzzle : MonoBehaviour
{
    void OnTriggerEnter(Collider Box)
    {
        if(Box.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
    }
}
