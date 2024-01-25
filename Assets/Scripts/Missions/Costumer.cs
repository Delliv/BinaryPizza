using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Costumer : MonoBehaviour, I_MissionGiver
{
    public Player player;
    public void action()
    {
        player.PizzaDone();
        gameObject.SetActive(false);
    }
}
