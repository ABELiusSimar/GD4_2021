using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardGate : MonoBehaviour
{
    private PlayerMovement playerItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Gate)
    {
        playerItem = Gate.GetComponent<PlayerMovement>();

        if(Gate.gameObject.tag == "Player")
        {
            if(playerItem.inventory.itemList.Count == 3)
            {
                Destroy(gameObject);
                Debug.Log("Open");
            }
            else
            {
                Debug.Log("Close");
            }
        }
        Debug.Log(playerItem.inventory.itemList.Count);
    }
}
