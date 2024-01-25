using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash_bike : MonoBehaviour
{
    public Rigidbody RB;
    public Transform TR;

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Build")){
            RB.AddForce(TR.forward * -5, ForceMode.Impulse);
            gameObject.GetComponent<player_movement_2>().pushed = true;
            gameObject.GetComponent<player_movement_2>().time_pushed = 0.5f;
            RB.velocity = new Vector3(0,0,0);
            gameObject.GetComponent<player_movement_2>().speed = 0;
        }

        if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Car_Police"))
        {
            RB.AddForce((collision.gameObject.transform.position - transform.position).normalized * -3, ForceMode.Impulse);
            gameObject.GetComponent<player_movement_2>().pushed = true;
            gameObject.GetComponent<player_movement_2>().time_pushed = 1.0f;
        }
    }
}
