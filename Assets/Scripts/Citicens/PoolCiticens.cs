using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCiticens : MonoBehaviour
{
    public GameObject prefab_citicen;
    public GameObject point_spawn;
    public int max_citicens;
    public List<GameObject> pool_citicens;
    public GameObject[] pedestrianSpawn;

    void Start()
    {
        GameObject[] instantiatePedestrians = GameObject.FindGameObjectsWithTag("Pedestrian");
        pedestrianSpawn = GameObject.FindGameObjectsWithTag("pedestrian_spawn");
        pool_citicens = new List <GameObject>();
        for(int i = 0; i < instantiatePedestrians.Length; i++)
        {
            //GameObject guy = (GameObject)Instantiate(prefab_citicen);
            //instantiatePedestrians[i].SetActive(false);
            pool_citicens.Add(instantiatePedestrians[i]);
        }
    }

    public void GetCiticen()
    {
        for(int i = 0; i < pool_citicens.Count; i++)
        {
            if (!pool_citicens[i].activeInHierarchy)
            {
                pool_citicens[i].transform.position = chooseSpawnPoint().transform.position;
                init(pool_citicens[i]);
                
                //return pool_citicens[i];
            }
        }
        //return null;
    }

    public GameObject chooseSpawnPoint()
    {
        return pedestrianSpawn[Random.Range(0, pedestrianSpawn.Length)];
    }

    public void init(GameObject c)
    {
        c.SetActive(true);
        c.GetComponent<nav_controller_pedestrians>().given = false;
        //c.transform.position = point_spawn.transform.position;
    }

    private void Update()
    {
        GetCiticen();
    }
}
