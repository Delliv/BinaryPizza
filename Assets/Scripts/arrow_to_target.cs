using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_to_target : MonoBehaviour
{
    public GameObject target_;

    void Update()
    {
        if (target_ != null){
            transform.LookAt(target_.transform.position);
        }
    }
}
