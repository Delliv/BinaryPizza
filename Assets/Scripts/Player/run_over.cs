using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class run_over : MonoBehaviour, interfaces
{
    public Rigidbody RB;
    public Transform TR;
    public GameObject particles;
    public void interact(Vector3 v, float s)
    {

        //Accedo al controller, para hacer que la policia te persiga.
        if (s > 0)
        {
            gameObject.GetComponent<nav_controller_pedestrians>().dead = true;
            GameObject.FindObjectOfType<game_controller>().makeAllPolicesChace();

            //gameObject.GetComponent<nav_controller_pedestrians>().enabled = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            RB.constraints = RigidbodyConstraints.None;
            RB.AddForce(v * s * 2, ForceMode.Impulse);
            Instantiate(particles, TR.transform.position, Quaternion.identity);
            
        }
        /*if(s >= 5)// Si queremos activar que la peña explote hay que ponerles un rigidbody, ponerlo en kinematic y activar tambien la gravedad y por ultimo activar los box colliders de sus partes
        {
            for(int i = 0; i < gameObject.transform.GetChild(0).transform.childCount; i++)
            {
                //gameObject.transform.GetChild(0).transform.GetChild(i).DetachChildren();
                gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            }
            gameObject.transform.GetChild(0).DetachChildren();
        }*/
    }
}
