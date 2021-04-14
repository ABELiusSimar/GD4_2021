using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DamageScript : MonoBehaviour
{

    [SerializeField]
    private Canvas canvas;
    private Color32 color;
    [SerializeField]
    private float HP = 2f;
    // Start is called before the first frame update
    void Start()
    {
        color.r = 255;
        color.a = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage()
    {
        HP -= 1;
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
        Image img = canvas.GetComponentInChildren<Image>();
        var tempcolor = img.color;
        tempcolor.a = 0.15f;
        img.color = tempcolor;
        Invoke("ResetColor", 0.8f);


    }
    public void RestoreHealth()
    {

    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }

    private void Respawn()
    {

    }

    private void ResetColor()
    {
        Image img = canvas.GetComponentInChildren<Image>();
        var tempcolor = img.color;
        tempcolor.a = 0f;
        img.color = tempcolor;
    }
}
