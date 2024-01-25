using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_controller : MonoBehaviour
{
    public bool police_chasing = false;
    public car_police_movement[] polices;

    GameObject police_indicator;

    bool seePlayer = false;
    public float timeToLeavePolice = 10.0f;
    public float distanceToLeavePolice = 50.0f;
    float timerLeavePolice = 0;

    private void Start()
    {
        police_indicator = GameObject.Find("Police_Lights_Indicator");
        police_indicator.SetActive(false);
        polices = GameObject.FindObjectsOfType<car_police_movement>();
    }

    private void Update()
    {
        seePlayer = false;

        for (int i = 0; i < polices.Length; i++){
            if (polices[i].seeingPlayer)
            {
                seePlayer = true;
            }
        }

        if (police_chasing)
        {
            if (!police_indicator.activeSelf)
            {
                police_indicator.SetActive(true);
            }
        }
        else
        {
            if (police_indicator.activeSelf)
            {
                police_indicator.SetActive(false);
            }
        }

        if (!seePlayer){ //Si ningun policia ve al player...
            timerLeavePolice += Time.deltaTime;
        }
        else
        {
            timerLeavePolice = 0;
        }

        if (timerLeavePolice >= timeToLeavePolice)
        {
            makeAllPolicesLosePlayer();
        }
    }

    public void makeAllPolicesChace()
    {    
        for (int i = 0; i < polices.Length; i++)
        {
            polices[i].chasing_player = true;
        }
        police_chasing = true;
        timerLeavePolice = 0;
    }

    public void makeAllPolicesLosePlayer()
    {
        for (int i = 0; i < polices.Length; i++)
        {
            polices[i].chasing_player = false;
        }
        police_chasing = false;
        timerLeavePolice = 0;
    }
}
