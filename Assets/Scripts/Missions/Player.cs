using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;
    public int gold;
    public Mission current_mission;

    public void PizzaDone()
    {
        if (current_mission.active)
        {
            current_mission.goal.PizzaGiven();
            if (current_mission.goal.IsCompleted())
            {
                gold += current_mission.gold;
                current_mission.Complete();
                current_mission.goal.currentPizzas = 0;
                current_mission = null;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MissionGiver>() != null)
        {
            other.gameObject.GetComponent<MissionGiver>().action();
        }

        if (other.gameObject.GetComponent<Costumer>() != null)
        {
            other.gameObject.GetComponent<Costumer>().action();
        }
    }
}
