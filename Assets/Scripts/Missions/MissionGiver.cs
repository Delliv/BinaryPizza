using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class MissionGiver : MonoBehaviour, I_MissionGiver
{
    public int current_quest;
    public Mission[] mission;

    public Player player;

    public GameObject missionWindow;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text gold;
    public TMP_Text pizza_num;



    public void openMissionWindow()
    {
        if (player.current_mission.active == false) { 
            missionWindow.SetActive(true);

            title.text = mission[current_quest].title;
            description.text = mission[current_quest].desription;
            gold.text = mission[current_quest].gold.ToString();
            pizza_num.text = (mission[current_quest].goal.requiredPizzas.ToString() + " Pizzas");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void action()
    {
        current_quest = Random.Range(0, mission.Length);print("Ha salido la " + current_quest);
        openMissionWindow();
    }

    public void acceptMission()
    {
        missionWindow.SetActive(false);
        mission[current_quest].active = true;
        player.current_mission = mission[current_quest];
        for (int i = 0; i < mission[current_quest].goal.requiredPizzas; i++)
        {
            mission[current_quest].goal.Adress[i].SetActive(true);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

}
