using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic_Light_Manager : MonoBehaviour
{
    public GameObject[] traffic_lights;
    public GameObject[] traffic_lights_pedestrians;
    public float time_change_light = 7.0f;
    public float time_change_ambar = 2.0f; //Que al pasar de verde a rojo, este un momento en ambar.
    private float timer_change;

    int lights_to_manage = 0;

    private void Start()
    {
        timer_change = time_change_light + time_change_ambar;

        //Primero pongo todos los otros semaforos en rojo...
        for (int i = 0; i < traffic_lights.Length; i++)
        {
            traffic_lights[i].GetComponent<Traffic_Light>().light_actual = 0; //Red    
        }
    }

    private void Update()
    {
        if (timer_change >= time_change_light)
        {
            if (timer_change <= time_change_light + time_change_ambar)
            {
                //Ambar
                traffic_lights[lights_to_manage].GetComponent<Traffic_Light>().light_actual = 2; //Ambar
            }
            else
            {
                traffic_lights[lights_to_manage].GetComponent<Traffic_Light>().light_actual = 0; //Red
                lights_to_manage++;
                if (lights_to_manage >= traffic_lights.Length)
                {
                    lights_to_manage = 0;
                }

                //Y uno en verde.
                traffic_lights[lights_to_manage].GetComponent<Traffic_Light>().light_actual = 1; //Green

                timer_change = 0;
            }
        }
        timer_change += Time.deltaTime;

        if(traffic_lights[0].GetComponent<Traffic_Light>().light_actual == 0) {
            traffic_lights_pedestrians[0].GetComponent<Traffic_Light>().light_actual = 1;
        }else if (traffic_lights[0].GetComponent<Traffic_Light>().light_actual == 1)
        {
            traffic_lights_pedestrians[0].GetComponent<Traffic_Light>().light_actual = 0;
        }

        if (traffic_lights[2].GetComponent<Traffic_Light>().light_actual == 0)
        {
            traffic_lights_pedestrians[1].GetComponent<Traffic_Light>().light_actual = 1;
        }
        else if (traffic_lights[2].GetComponent<Traffic_Light>().light_actual == 1)
        {
            traffic_lights_pedestrians[1].GetComponent<Traffic_Light>().light_actual = 0;
        }
    }

   

}
