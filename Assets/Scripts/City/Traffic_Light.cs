using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic_Light : MonoBehaviour
{
    public Material[] material_color;
    public int light_actual = 0;

    private void Update()
    {
        if (light_actual == 0) //Red
        {
            //Ponemos el color del cubo en rojo, para verlo
            gameObject.GetComponent<MeshRenderer>().material = material_color[0];
        }
        else if (light_actual == 2) //Ambar
        {
            //Ponemos el color del cubo en verde, para verlo
            gameObject.GetComponent<MeshRenderer>().material = material_color[2];
        }
        else if (light_actual == 1) //Green
        {
            //Ponemos el color del cubo en verde, para verlo
            gameObject.GetComponent<MeshRenderer>().material = material_color[1];
        }
    }

}
