using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_movement : MonoBehaviour
{
    Rigidbody rb;
    [HideInInspector]
    public GameObject car_infront = null;

    public float mov_max_speed = 25;
    public float mov_aceleration = 2;
    public float mov_frenado = 2;
    public float extra_frenado = 3;
    float extra_actual_frenado = 0;
    bool frenando = false;

    Manager_Points m_points = null;
    float actual_speed;
    public float rotate_speed = 5;
    public float distance_check_point;
    GameObject next_point_to_go;
    GameObject next_next_point_to_go;

    [Header("Check Car Forward")]
    public float distance_car_forward = 1.0f;
    public LayerMask layermask_check_car;
    public float time_to_autodestroy = 10.0f;
    [HideInInspector]
    public bool coche_enfrente = false;
    float timer_auto_destroy = 0;
    
    Vector3 rotation_Direction;

    [HideInInspector]
    public bool in_traffic_light = false;

    public float time_pushed = 0.4f;
    [HideInInspector]
    public bool pushed = false;
    private float timer_pushed;

    [Header("Pedestrians")]
    public Collider pedestrians_detector;
    [HideInInspector]
    public bool pedestriant_in_front = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_points = GameObject.FindObjectOfType<Manager_Points>();
        next_point_to_go = m_points.find_near_point(transform);
        //actual_speed = mov_max_speed / 10;
        mov_max_speed += Random.Range(0, 30);
    }

    void Update()
    {
        if (next_point_to_go != null)
        {
            if (Vector3.Distance(transform.position, next_point_to_go.transform.position) <= distance_check_point)
            {
                path_point actual = next_point_to_go.GetComponent<path_point>();
                if (actual.next_points.Length > 0)
                {
                    if (next_next_point_to_go == null)
                    {
                        next_point_to_go = actual.next_points[Random.Range(0, actual.next_points.Length)];
                    }
                    /*else
                    {
                        next_point_to_go = next_next_point_to_go;
                    }*/

                    /*if (next_point_to_go.GetComponent<path_point>().next_points.Length > 0)
                    {
                        next_next_point_to_go = next_point_to_go.GetComponent<path_point>().next_points[Random.Range(0, next_point_to_go.GetComponent<path_point>().next_points.Length)];
                    }*/
                }
                else
                {
                    next_point_to_go = m_points.find_near_point(transform, "Enter");
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
                Vector3 point_to_go = new Vector3(next_point_to_go.transform.position.x, 0 , next_point_to_go.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, point_to_go, actual_speed * Time.deltaTime);
            }
        }
        else
        {
            next_point_to_go = m_points.find_near_point(transform, "Enter");
        }

        /*if (next_next_point_to_go != null)
        {
            //con esto podemos saber hacia donde girara el coche en el siguiente punto.
            if (next_point_to_go.GetComponent<path_point>().next_points.Length > 1)
            {
                float angle_ = Vector3.SignedAngle(next_point_to_go.transform.position, next_next_point_to_go.transform.position, gameObject.transform.forward);
                //Debug.Log("Esta a un angulo de -> " + Vector3.SignedAngle(next_point_to_go.transform.position, next_next_point_to_go.transform.position, gameObject.transform.forward));
                //Debug.Log("" + angle_);
                if (angle_ > 5.0f)
                {
                    //Debug.Log("Giro a la Izquierda");
                }
                else if (angle_ < -5.0f)
                {
                    //Debug.Log("Giro a la Derecha");
                }
            }
        }*/

        /*if (GameObject.Find("GM_Controller").GetComponent<game_controller>().police_chasing)
        {
            if (rb.isKinematic) { rb.isKinematic = false; }
        }
        else
        {
            if (!rb.isKinematic) { rb.isKinematic = true; }
        }*/

        if (coche_enfrente || in_traffic_light || pedestriant_in_front)
        {
            frenando = true;
        }
        else
        {
            frenando = false;
        }

        if (pushed)
        {
            timer_pushed += Time.deltaTime;
            if (timer_pushed >= time_pushed)
            {
                Debug.Log("Me paro");
                //rb.velocity = Vector3.zero;
                rb.freezeRotation = true;
                rb.isKinematic = true;
                rb.isKinematic = false;
                timer_pushed = 0;
                pushed = false;
                next_point_to_go = null;
                //next_next_point_to_go = null;
                next_point_to_go = m_points.find_near_point(transform);
            }
        }

        if (rb.velocity.magnitude > 0 && !pushed)
        {
            rb.velocity = Vector3.zero;
        }

        //Si ha pasado demasiado tiempo que estoy atascado con un coche adelante, me auto destruyo.
        if(timer_auto_destroy >= time_to_autodestroy){
            Debug.Log("Soy " + gameObject.name + " y desaparezco");
            //Destroy(gameObject);
            timer_auto_destroy = 0;
            actual_speed = 0;
            next_point_to_go = null;
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
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
                else
                {
                    if(car_infront.name != ray_hit.collider.gameObject.name)
                    {
                        car_infront = ray_hit.collider.gameObject;
                    }
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

        /*if (car_infront != null)
        {
            if (car_infront.GetComponent<car_movement>().car_infront != null)
            {
                if (car_infront.GetComponent<car_movement>().car_infront.name == gameObject.name)
                {
                    Debug.Log("Soy yo");
                }
            }
        }*/

        //Si tengo un coche delante, y ese coche me tiene a mi delante, es que estoy atascado.
        if (car_infront != null){
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
            }else if (car_infront.CompareTag("Car_Police"))
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
}