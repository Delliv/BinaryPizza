using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_colors : MonoBehaviour
{
    private void Awake()
    {
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.SetColor("_Color", Random.ColorHSV());

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.name == "Chasis")
            {
                child.GetComponent<MeshRenderer>().material = newMaterial;   
            }
        }
    }
}
