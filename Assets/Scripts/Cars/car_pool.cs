using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_pool : MonoBehaviour
{
    public string tagObj;
    public GameObject goPrefab;
    public int maxPoolSize = 50;
    List<GameObject> elementsPoolList;

    GameObject[] spawn_points;

    void Start()
    {
        GameObject parentCars = GameObject.Find("Cars");
        elementsPoolList = new List<GameObject>();
        GameObject[] instantiateCars = GameObject.FindGameObjectsWithTag(tagObj);
        for (int i = 0; i < instantiateCars.Length; i++)
        {
            elementsPoolList.Add(instantiateCars[i]);
        }

        if (elementsPoolList.Count < maxPoolSize){
            for (int i = elementsPoolList.Count; i < maxPoolSize; i++){
                GameObject obj = (GameObject)Instantiate(goPrefab, parentCars.transform);
                obj.SetActive(false);
                elementsPoolList.Add(obj);
            }
        }

        spawn_points = GameObject.FindGameObjectsWithTag("Spawn_cars");
    }

    public GameObject GetPoolObject(){

        for (int i = 0; i < elementsPoolList.Count; i++){
            if (!elementsPoolList[i].activeInHierarchy){
                return elementsPoolList[i];
            }
        }
        return null;
    }

    private void Update()
    {
        if(GetPoolObject() != null)
        {
            GameObject car = GetPoolObject();
            car.SetActive(true);
            car.transform.position = spawn_points[Random.Range(0, spawn_points.Length)].transform.position;
        }
    }
}
