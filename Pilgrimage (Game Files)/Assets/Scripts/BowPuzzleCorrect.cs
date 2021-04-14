using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPuzzleCorrect : MonoBehaviour
{
    [SerializeField] private Transform[] pic;
    public Transform spawnPos;
    public GameObject spawn;
    public static bool CorrectAns;
    public int DebugCounter; 

    // Start is called before the first frame update
    void Start()
    {
        CorrectAns = false;
        DebugCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(DebugCounter != 1)
        {
            if(pic[0].rotation.z == 0 && pic[1].rotation.z == 0 && pic[2].rotation.z == 0 && pic[3].rotation.z == 0 && pic[4].rotation.z == 0 && pic[5].rotation.z == 0 && pic[6].rotation.z == 0 && pic[7].rotation.z == 0 && pic[8].rotation.z == 0 )
            {
                CorrectAns = true;
                Debug.Log("Correct");
                Instantiate(spawn, spawnPos.position, spawnPos.rotation);
                DebugCounter = 1;
            }
        }
    }
}
