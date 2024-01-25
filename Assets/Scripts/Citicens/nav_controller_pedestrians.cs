using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class nav_controller_pedestrians : MonoBehaviour
{
    game_controller gm_controller;
    public Transform tr_pedestrian;
    private NavMeshAgent nav_pedestrian;
    public float time_to_disapear;
    GameObject point;
    public bool given;

    Animator anim;

    public bool crossing = false;
    public bool dead;
    private void OnEnable()
    {
        if (nav_pedestrian != null)
        {
            nav_pedestrian = gameObject.GetComponent<NavMeshAgent>();
            if (!nav_pedestrian.enabled)
            {
                nav_pedestrian.enabled = true;
            }
        }
        dead = false;
        time_to_disapear = 0;
        given = false;
        if(anim != null)
        {
            anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
            if (!anim.enabled)
            {
                anim.enabled = true;
            }
        }
    }
    void Start()
    {
        time_to_disapear = 0;
        dead = false;
        given = false;
        gm_controller = GameObject.Find("GM_Controller").GetComponent<game_controller>();
        nav_pedestrian = gameObject.GetComponent<NavMeshAgent>();
        nav_pedestrian.speed += Random.Range(0.0f, 2.0f);

        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        find_point();
        //ReachPoint();

        if (anim != null && !dead)
        {
            anim.speed = nav_pedestrian.velocity.magnitude;
        }
        if (dead)
        {
            /*if (!nav_pedestrian.isStopped)
            {
                nav_pedestrian.isStopped = true;
            }*/
            time_to_disapear += Time.deltaTime;
               
            if(time_to_disapear >= 10)
            {
                Debug.Log("Soy " + name);
                given = false;
                gameObject.SetActive(false);
                
            }
            anim.enabled = false;
        }
    }

    public void find_point()
    {
        //Con esta función el civil buscará un punto aleatorio
        if (!given)
        {
            point = gm_controller.GetComponent<Manager_Points>().find_random_point(tr_pedestrian);
            nav_pedestrian.destination = point.transform.position;
            given = true;
        }
        if (Vector3.Distance(tr_pedestrian.transform.position, point.transform.position) < 0.5f)
        {
            given = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.CompareTag("pedestrian_light"))
            {
                if (other.GetComponent<Traffic_Light>().light_actual == 0)
                {
                    if (!nav_pedestrian.isStopped)
                    {
                        nav_pedestrian.isStopped = true;
                    }
                    nav_pedestrian.velocity = Vector3.zero;
                    //anim.GetCurrentAnimatorClipInfo(0).;
                    //Resetear la animacion al tiempo 0.                   
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!dead)
        {
            if (other.CompareTag("pedestrian_light"))
            {
                if (other.GetComponent<Traffic_Light>().light_actual == 1)
                {
                    if (nav_pedestrian.isStopped)
                    {
                        nav_pedestrian.isStopped = false;
                    }
                    crossing = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!dead)
        {
            if (other.CompareTag("pedestrian_light"))
            {
                crossing = false;
            }
        }
    }
}
