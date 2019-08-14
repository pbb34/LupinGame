using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    GameObject switchToggle;
    [SerializeField]
    GameObject Door;


    void Start()
    {
        Door.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //set the switch to on sprite
        if (col.tag == "Player")
        {
            Door.SetActive(false);
        }

    }
}
