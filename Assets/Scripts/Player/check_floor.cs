using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check_floor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {/*
        if (!other.CompareTag("Player") && !other.CompareTag("plane"))
        {
            GameObject.FindObjectOfType<player_movement_2>().RB.velocity = new Vector3(0, 10, 0);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {/*
        if (other.CompareTag("plane"))
        {
            GameObject.FindObjectOfType<player_movement_2>().RB.velocity = new Vector3(0, 10, 0);
        }*/
    }
}
