using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Obj : MonoBehaviour
{
    public GameObject obj_to_Follow;
    public Vector3 offset;

    void Update()
    {
        transform.position = obj_to_Follow.transform.position + offset;
    }
}
