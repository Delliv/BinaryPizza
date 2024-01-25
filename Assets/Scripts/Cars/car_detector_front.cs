using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_detector_front : MonoBehaviour
{
    public car_movement car_controller;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Pedestrian"))
        {
            if (other.gameObject.GetComponent<nav_controller_pedestrians>().crossing && !other.gameObject.GetComponent<nav_controller_pedestrians>().dead)
            {
                //Debug.Log("Soy: " + transform.parent.gameObject.name + " -> Personitaaaaaaaaa!!");
                car_controller.pedestriant_in_front = true;
                car_controller.extra_frenado = 200;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pedestrian"))
        {
            car_controller.pedestriant_in_front = false;
            car_controller.extra_frenado = 0;
        }
    }
}
