using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueData : MonoBehaviour
{
    public bool isCollectible;
    public bool isTrigger;
    public bool isRandom;
    public bool isExit;
    public bool onlyOnce;
    public bool used;

    public string itemName;
    public string fName;
    public string name;

    public string[] desc;
    public string[] funcRequest; 

    public void destroySelf()
    {
        Destroy(gameObject);
    }

    public string GetName()
    {
        return itemName;
    }

    public void UseTrigger()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Stage 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().UseFunction(fName); break;
            case "Stage 2": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level2_Controller>().UseFunction(fName); break;
            case "Stage 3": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().UseFunction(fName); break;
            case "Stage 4": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level4_Controller>().UseFunction(fName); break;

            case "Hallway 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<HallwayController>().UseFunction(fName); break;
            case "Ending": GameObject.Find("LEVEL_CONTROLLER").GetComponent<EndingController>().UseFunction(fName); break;
        }
    }


}
