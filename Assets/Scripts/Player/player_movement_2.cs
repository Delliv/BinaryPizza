using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement_2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody RB;
    public Transform TR_pivote;
    public float speed = 0;
    public float rotate_angle, speed_angle =110; //speed angle es la variable que usamos para medir la velocidad a la que la moto tumba. rotate_angle es la variable que guarda el grado maximo al que gira la moto
    public float top_speed = 10, acceleration = 2, deceleration = 2, break_force = 1, rotation_grade = 50, turn_speed = 50;
    public bool acelerando = false, backwards; //Backwards para saber cuando estamos yendo marcha atras
    public bool pushed, on_floor;
    public float time_pushed, time_idle;
    RaycastHit gravity_check_front, gravity_check_back;
    public float scope_raycast; //Tamañó del raycast para saber cuando estamos tocando el suelo
    void Start()
    {
        time_idle = 2;
        scope_raycast = 0.2f;
        on_floor = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Acelerar y decelerar
        if(Input.GetAxis("Vertical") > 0)
        {
            acelerando = true;
        }
        else
        {
            acelerando = false;     
        }

        if (acelerando && speed < top_speed)
        {
            speed += acceleration * Time.deltaTime;
        }
        else if(!acelerando && speed > -2)
        {
            speed -= deceleration * Time.deltaTime;
        }

        //Marcha atras
        if(Input.GetAxis("Vertical") < 0)
        {
            backwards = true;
        }
        else
        {
            backwards = false;
        }
        if (backwards && speed > -2)
        {
            speed -= break_force * Time.deltaTime;
        }

        //Girar la moto
        TR_pivote.localRotation = Quaternion.Euler(0F, 0f, -rotate_angle);
        if (speed > 0)  
        {
            if (Input.GetAxis("Horizontal") > 0 && rotate_angle < rotation_grade) //Si estamos acelerando y el angulo de la moto es menor al grado maximo al que puede girar podremos girar hacia un lado u otro
            {
                rotate_angle += speed_angle * Time.deltaTime;
            }
            else if(Input.GetAxis("Horizontal") < 0 && rotate_angle > -rotation_grade) {
                rotate_angle -= speed_angle * Time.deltaTime;
            }
            else
            {
                if (rotate_angle > 0 && Input.GetAxis("Horizontal")==0) // Si la moto esta rotada hacia uno de los lados y hemos dejado de pulsar un lado la moto rotara automaticamente al centro
                {
                    rotate_angle -= 80 * Time.deltaTime;
                    if(rotate_angle < 0)
                    {
                        rotate_angle = 0;
                    }
                }else if (rotate_angle < 0 && Input.GetAxis("Horizontal") == 0)
                {
                    rotate_angle += 80 * Time.deltaTime;
                    if (rotate_angle > 0)
                    {
                        rotate_angle = 0;
                    }
                }
            }
            
            transform.Rotate(0, turn_speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0, Space.Self);

           
        }
        else if (speed < 0 && backwards) //Si estamos yendo marcha atras no podremos rotar la moto y giraras mas lento que yendo hacia delante
        {
            float rotation_pivote = Input.GetAxis("Horizontal") * rotation_grade * -1;
            transform.Rotate(0, turn_speed/2 * Input.GetAxis("Horizontal") * -1 * Time.deltaTime, 0, Space.Self);
            if (rotate_angle > 0)
            {
                rotate_angle -= 30 * Time.deltaTime;
            }
            else if (rotate_angle < 0)
            {
                rotate_angle += 30 * Time.deltaTime;
            }
        }

        
      
        if(!acelerando && speed < 0 && !backwards)
        {
            speed = 0;
        }

        if(time_pushed > 0) 
        {
            time_pushed -= Time.deltaTime * time_idle;
        }
        else
        {
            if (pushed){
                pushed = false;
            }
        }

        if (speed == 0) //Aqui la moto cuando no este moviendose rotara un poco haia la derecha para imitar a los motoristas en los semaforos inclinando la moto para apoyarse.
        {
            if (rotate_angle < 10 )
            {
                rotate_angle += 50 * Time.deltaTime;
                if (rotate_angle > 10)
                {
                    rotate_angle = 10;
                }
            }
            else if(rotate_angle > 10)
            {
                rotate_angle -= 50 * Time.deltaTime;
                if(rotate_angle < 10)
                {
                        rotate_angle = 10;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pushed) {
            RB.velocity = transform.forward * speed;
        }
        Debug.DrawRay(transform.position, -transform.up * scope_raycast, Color.blue);
        
        if (!Physics.Raycast(transform.position, -transform.up ,out gravity_check_front,scope_raycast))
        {
            RB.velocity = new Vector3(0, -10, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pedestrian")) //Si colisionamos con un pedestrian le pasamos nuestro transform.forward y el speed para impulsarlo en esa direccion
        {
            collision.gameObject.GetComponent<run_over>().interact(transform.forward, speed);
        }
    }
}
