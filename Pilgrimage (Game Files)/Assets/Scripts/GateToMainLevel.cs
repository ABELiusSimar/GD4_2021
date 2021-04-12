using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateToMainLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider Gate)
    {
        if(Gate.gameObject.tag == "Player" || Gate.gameObject.tag == "Untagged")
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
