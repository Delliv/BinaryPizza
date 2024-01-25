using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Points : MonoBehaviour
{
    public GameObject[] allPoints;
    public GameObject[] pedestrianPoints;
    void Start(){
        allPoints = GameObject.FindGameObjectsWithTag("Point_path");
        pedestrianPoints = GameObject.FindGameObjectsWithTag("pedestrian_point");
    }

    public GameObject find_near_point(Transform tr, string tag_find = "all")
    {
        //In the start of the game, the car find the near point to start moving.
        float near_point = 9999;
        GameObject aux_point = null;
        for (int i = 0; i < allPoints.Length; i++)
        {
            if (tag_find == "all")
            {
                if (Vector3.Distance(tr.position, allPoints[i].transform.position) <= near_point)
                {
                    near_point = Vector3.Distance(tr.position, allPoints[i].transform.position);
                    aux_point = allPoints[i];
                }
            }
            else
            {
                if (allPoints[i].name == tag_find)
                {
                    if (Vector3.Distance(tr.position, allPoints[i].transform.position) <= near_point)
                    {
                        near_point = Vector3.Distance(tr.position, allPoints[i].transform.position);
                        aux_point = allPoints[i];
                    }
                }
            }
        }

        return aux_point;
    }

    public GameObject find_random_point(Transform tr)
    {
        return pedestrianPoints[Random.Range(0, pedestrianPoints.Length)];
    }
}


