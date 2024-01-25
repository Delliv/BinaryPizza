using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{

    public bool active;
    public string title;
    public string desription;
    public int gold;

    public Goals goal;

    public void Complete()
    {
        active = false;
        Debug.Log(title + " completed!");
    }
}
