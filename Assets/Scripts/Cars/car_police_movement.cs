using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class car_police_movement : MonoBehaviour
{
    game_controller gm_controller;
    Manager_Points gm_points;
    public GameObject car_infront = null;

    public float mov_max_speed = 25;
    public float mov_aceleration = 2;
    public float mov_frenado = 2;
    public float extra_frenado = 3;
    float extra_actual_frenado = 0;
    bool frenando = false;
    public float mov_max_speed_chasing = 5;

    float actual_speed;
    public float rotate_speed = 5;
    public float distance_check_point;
    GameObject next_point_to_go;
    //GameObject next_next_point_to_go;

    [Header("Check Car Forward")]
    public float distance_car_forward = 1.0f;
    public LayerMask layermask_check_car;
    public float time_to_autodestroy = 10.0f;
    public bool coche_enfrente = false;
    float timer_auto_destroy = 0;

    Vector3 rotation_Direction;

    public bool in_traffic_light = false;

    [Header("Chasing")]
    public bool chasing_player = false;
    public float impulse_force = 5.0f;
    private NavMeshAgent nav_agent;
    private GameObject player_target;
    public LayerMask layermask_chasing;
    public bool seeingPlayer = false;


    [Header("Pedestrians")]
    public Collider pedestrians_detector;
    public bool pedestriant_in_front = false;

    private void Start()
    {
        gm_controller = GameObject.Find("GM_Controller").GetComponent<game_controller>();
        gm_points = GameObject.Find("GM_Controller").GetComponent<Manager_Points>();

        //actual_speed = mov_max_speed / 10;
        next_point_to_go = gm_points.find_near_point(transform);

        nav_agent = gameObject.GetComponent<NavMeshAgent>();
        nav_agent.speed = (mov_max_speed / 10) + mov_max_speed_chasing;
        nav_agent.enabled = false;
        player_target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!chasing_player)
        {
            if (next_point_to_go == null)
            {
                nav_agent.enabled = false;
                next_point_to_go = gm_points.find_near_point(transform, "Enter");
            }
            else
            {
                if (Vector3.Distance(transform.position, next_point_to_go.transform.position) <= distance_check_point)
                {
                    path_point actual = next_point_to_go.GetComponent<path_point>();
                    if (actual.next_points.Length > 0)
                    {
                        next_point_to_go = actual.next_points[Random.Range(0, actual.next_points.Length)]; 
                    }
                    else
                    {
                        next_point_to_go = gm_points.find_near_point(transform, "Enter");
                    }
                }

                if (!frenando)
                {
                    if (actual_speed < mov_max_speed / 10)
                    {
                        actual_speed += mov_aceleration * Time.deltaTime;
                    }
                }
                else
                {
                    if (actual_speed > 0)
                    {
                        actual_speed -= (mov_frenado + extra_actual_frenado) * Time.deltaTime;
                    }
                    else
                    {
                        actual_speed = 0;
                    }
                }

                if (next_point_to_go != null)
                {
                    //Rotate de car to direction of the new point
                    rotation_Direction = Vector3.RotateTowards(transform.forward, next_point_to_go.transform.position - transform.position, rotate_speed * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(rotation_Direction);

                    //Move the car to the new point
                    Vector3 point_to_go = new Vector3(next_point_to_go.transform.position.x, 0, next_point_to_go.transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, point_to_go, actual_speed * Time.deltaTime);
                }
            }
        }
        else
        {
            if(nav_agent.enabled == false)
            {
                nav_agent.enabled = true;
                next_point_to_go = null;
                //next_next_point_to_go = null;
            }
            nav_agent.destination = player_target.transform.position;
        }


        if (coche_enfrente || in_traffic_light || pedestriant_in_front)
        {
            frenando = true;
        }
        else
        {
            frenando = false;
        }

        //Si ha pasado demasiado tiempo que estoy atascado con un coche adelante, me auto destruyo.
        if (timer_auto_destroy >= time_to_autodestroy)
        {
            Debug.Log("Soy " + gameObject.name + " y desaparezco");
            timer_auto_destroy = 0;
            actual_speed = 0;
            next_point_to_go = null;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!chasing_player)
        {
            RaycastHit ray_hit;
            Vector3 offset_pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);

            if (Physics.Raycast(offset_pos, transform.TransformDirection(Vector3.forward), out ray_hit, 2.5f, layermask_check_car))
            {
                Debug.DrawRay(offset_pos, transform.TransformDirection(Vector3.forward) * 2.5f, Color.green);
                extra_actual_frenado = 10;
                //coche_enfrente = true;
            }
            else
            {
                //Debug.DrawRay(offset_pos, transform.TransformDirection(Vector3.forward) * 2.5f, Color.blue);
                if (extra_actual_frenado > 0)
                {
                    extra_actual_frenado = 0;
                }
                //coche_enfrente = false;
            }

            if (Physics.Raycast(offset_pos, transform.TransformDirection(Vector3.forward), out ray_hit, distance_car_forward, layermask_check_car))
            {
                Debug.DrawRay(offset_pos, transform.TransformDirection(Vector3.forward) * distance_car_forward, Color.red);

                //if (ray_hit.collider.CompareTag("Car"))
                //{
                    coche_enfrente = true;
                    if (car_infront == null)
                    {
                        car_infront = ray_hit.collider.gameObject;
                    }
                //}
                //frenando = true;
            }
            else if (!Physics.Raycast(offset_pos, transform.TransformDirection(Vector3.forward), out ray_hit, distance_car_forward, layermask_check_car))
            {
                //Debug.DrawRay(offset_pos, transform.TransformDirection(Vector3.forward) * distance_car_forward, Color.blue);

                coche_enfrente = false;
                car_infront = null;
                timer_auto_destroy = 0;
                //frenando = false;
            }

            //Si tengo un coche delante, y ese coche me tiene a mi delante, es que estoy atascado.
            if (car_infront != null)
            {
                if (car_infront.CompareTag("Car"))
                {
                    if (car_infront.GetComponent<car_movement>().coche_enfrente == true)
                    {
                        if (car_infront.GetComponent<car_movement>().car_infront.name == gameObject.name)
                        {
                            timer_auto_destroy += Time.deltaTime;
                            //Debug.Log(gameObject.name + ": Soy yo delante de -> " + car_infront.GetComponent<car_movement>().car_infront.name);
                        }
                    }
                }
                else if (car_infront.CompareTag("Car_Police"))
                {
                    if (car_infront.GetComponent<car_police_movement>().coche_enfrente == true)
                    {
                        if (car_infront.GetComponent<car_police_movement>().car_infront.name == gameObject.name)
                        {
                            timer_auto_destroy += Time.deltaTime;
                            //Debug.Log(gameObject.name + ": Soy yo delante de -> " + car_infront.GetComponent<car_movement>().car_infront.name);
                        }
                    }
                }
            }
        }
        else
        {
            //Si esta cazando al player, comprobaremos si lo esta viendo.
            //Habra que comprobar tambien la distancia, y que entonces empiece el contador.
            //Comprobar que este dentro de un cono de vision? clampear el limite hacia los lados des del forward?
            RaycastHit ray_hit;
            Vector3 offset_aux = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            Vector3 offset_player = new Vector3(player_target.transform.position.x, player_target.transform.position.y + 1.0f, player_target.transform.position.z);
            Vector3 direction_player = offset_player - offset_aux;

            if (Physics.Raycast(transform.position, direction_player, out ray_hit, Mathf.Infinity, layermask_chasing)) {
                if (ray_hit.collider.CompareTag("Player") && Vector3.Distance(offset_aux, offset_player) < gm_controller.distanceToLeavePolice)
                { //Si no hay nada que intercepte la vision al player... Y estoy a menos de la distancia de vision...
                    seeingPlayer = true;
                    Debug.DrawLine(offset_aux, ray_hit.point, Color.red);
                }
                else
                {
                    seeingPlayer = false;
                    Debug.DrawLine(offset_aux, ray_hit.point, Color.green);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Traffic_Light"))
        {
            if (other.GetComponent<Traffic_Light>().light_actual == 0 || other.GetComponent<Traffic_Light>().light_actual == 2) //Red o ambar
            {
                //Debug.Log("Esta rojo");
                in_traffic_light = true;
                extra_actual_frenado = extra_frenado;
                //frenando = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Traffic_Light"))
        {
            //Debug.Log("Detecto el semaforo");
            if (other.GetComponent<Traffic_Light>().light_actual == 1 || (other.GetComponent<Traffic_Light>().light_actual == 2 && !frenando)) //Green o Ambar
            {
                if (!coche_enfrente)
                {
                    //Debug.Log("Esta verde");
                    in_traffic_light = false;
                    //frenando = false;
                    extra_actual_frenado = 0;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Traffic_Light"))
        {
            in_traffic_light = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car")){
            //Debug.Log("Choco contra un coche");
            if (chasing_player){
                collision.gameObject.GetComponent<car_movement>().GetComponent<Rigidbody>().isKinematic = false;
                //collision.gameObject.GetComponent<car_movement>().GetComponent<Rigidbody>().freezeRotation = false;
                collision.gameObject.GetComponent<car_movement>().pushed = true;
                collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.position - transform.position).normalized * impulse_force, ForceMode.Impulse);
            }
        }
    }
}
