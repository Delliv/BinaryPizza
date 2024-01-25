using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=iuWxTPwEMys&ab_channel=gipsle 

public class path_point : MonoBehaviour
{
    public GameObject[] next_points;
    //public List<GameObject> next_points;

    void OnDrawGizmos(){
        if (next_points.Length > 0){
            for (int i = 0; i < next_points.Length; i++){
                if (next_points[i] != null)
                {
                    Gizmos.color = Color.green;
                    Vector3 direction = next_points[i].transform.position - transform.position;

                    Gizmos.DrawRay(transform.position, direction);

                    Gizmos.color = Color.red;
                    Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 0, 1);
                    Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, 0, 1);
                    Gizmos.DrawRay(transform.position + (direction * 0.5f), right * 0.50f);
                    Gizmos.DrawRay(transform.position + (direction * 0.5f), left * 0.50f);
                }
                else
                {
                    //next_points.RemoveAt(i);
                }
            }
        }
    }
}