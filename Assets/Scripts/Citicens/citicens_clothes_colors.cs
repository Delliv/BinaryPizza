using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class citicens_clothes_colors : MonoBehaviour
{
    public Material[] skin_colors;
    private void Awake()
    {
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.SetColor("_Color", Random.ColorHSV());

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.name == "body" || child.name == "left_arm" || child.name == "right_arm")
            {
                child.GetComponent<MeshRenderer>().material = newMaterial;
            }
            if (child.name == "head")
            {
                child.GetComponent<MeshRenderer>().material = skin_colors[Random.Range(0, skin_colors.Length)];
            }
        }
    }
}
