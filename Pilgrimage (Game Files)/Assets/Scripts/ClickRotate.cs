using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Rotate picture on hit
    void OnTriggerEnter(Collider Pic)
    {
        if(!BowPuzzleCorrect.CorrectAns)
        {
            if(Pic.gameObject.tag == "Untagged")
            {
                transform.Rotate(0f, 0f, 90f);
            }
        }
    }
}
