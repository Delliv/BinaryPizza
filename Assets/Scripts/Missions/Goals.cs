using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goals
{
    public int requiredPizzas;
    public int currentPizzas;
    public GameObject[] Adress;

    public Goals()
    {
        GameObject[] Adress = new GameObject[requiredPizzas];
    }
    public bool IsCompleted()
    {      
        return (currentPizzas >= requiredPizzas);
    }

    public void PizzaGiven()
    {
        currentPizzas++;
    }
}
