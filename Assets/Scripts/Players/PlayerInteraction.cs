using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    private int index = 0;

    private string name;

    private string[] desc;
    private string[] emptyArray;

    private bool used;
    private bool inUse = false;
    private bool isExit = false;
    private bool hasData = false;
    private bool isRandom = false;
    private bool onlyOnce = false;
    private bool isTrigger = false;
    private bool isCollectible = false;
    private bool dialogueIsActive = false;

    private DialogueData temp;

    //Link to UI
    private void Start()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        descText = GameObject.Find("DescText").GetComponent<TMP_Text>();

        dialogueBox.SetActive(false);
    }

    //User Input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.GetComponent<PlayerScript>().ToggleMovement(true);
            ResetData();
        }

        if(dialogueBox == null)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Hallway 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<HallwayController>().resetDialogue(); break;

                case "Stage 2": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level2_Controller>().resetDialogue(); break;
                case "Stage 3": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().resetDialogue(); break;
                case "Stage 4": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level4_Controller>().resetDialogue(); break;


                //case "Ending": GameObject.Find("LEVEL_CONTROLLER").GetComponent<EndingController>().resetDialogue(); break;
            }

            dialogueBox = GameObject.Find("DialogueBox");
            nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
            descText = GameObject.Find("DescText").GetComponent<TMP_Text>();

            dialogueBox.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Stage 1")
        {
            if (GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().isHiding && !GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().nurseSpawned) return;
        }

        if (!dialogueIsActive) {
            if (SimpleInput.GetKeyDown(KeyCode.E) && hasData)
            {
                ActivateDialogueBox();

                if (isRandom) RandomDialogue();
                else UseDialogue();
            }
        }
        else {
            if (SimpleInput.GetKeyDown(KeyCode.Space))
            {
                if (isRandom) RandomDialogue();
                else UseDialogue();
            }
        }
    }

    //Get and Reset Item Information
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") && collision.GetComponent<DialogueData>())
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Stage 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().toggleInteraction(true, "Press 'E' to Interact"); break;

                case "Hallway 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<HallwayController>().toggleInteraction(true, "Press 'E' to Interact"); break;

                case "Stage 2": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level2_Controller>().toggleInteraction(true, "Press 'E' to Interact"); break;
                case "Stage 3": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().toggleInteraction(true, "Press 'E' to Interact"); break;
                case "Stage 4": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level4_Controller>().toggleInteraction(true, "Press 'E' to Interact"); break;


                case "Ending": GameObject.Find("LEVEL_CONTROLLER").GetComponent<EndingController>().toggleInteraction(true, "Press 'E' to Interact"); break;
            }


            name = collision.GetComponent<DialogueData>().name;
            desc = collision.GetComponent<DialogueData>().desc;
            isCollectible = collision.GetComponent<DialogueData>().isCollectible;
            isRandom = collision.GetComponent<DialogueData>().isRandom;
            isTrigger = collision.GetComponent<DialogueData>().isTrigger;
            isExit = collision.GetComponent<DialogueData>().isExit;
            onlyOnce = collision.GetComponent<DialogueData>().onlyOnce;

            temp = collision.GetComponent<DialogueData>();

            hasData = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().toggleInteraction(false); break;

            case "Hallway 1": GameObject.Find("LEVEL_CONTROLLER").GetComponent<HallwayController>().toggleInteraction(false); break;

            case "Stage 2": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level2_Controller>().toggleInteraction(false); break;
            case "Stage 3": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().toggleInteraction(false); break;
            case "Stage 4": GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level4_Controller>().toggleInteraction(false); break;

            case "Ending": GameObject.Find("LEVEL_CONTROLLER").GetComponent<EndingController>().toggleInteraction(false); break;
        }

        if (!collision.CompareTag("Player"))
        {
            name = string.Empty;
            desc = emptyArray;

            isCollectible = false;
            isRandom = false;
            isExit = false;
            isTrigger = false;

            hasData = false;
        }
    }

    //Access UI

    public void ActivateDialogueBox()
    {
        if(desc == null)
        {
            if (isTrigger)
            {
                temp.UseTrigger();
                return;
            }
        }
        if (used) return;
        transform.GetComponent<PlayerScript>().inInteraction = true;
        transform.GetComponent<PlayerScript>().ToggleMovement(false);
        dialogueBox.SetActive(true);
        dialogueIsActive = true;

        nameText.text = "";
        descText.text = "";

        if (onlyOnce) temp.used = true;
    }

    public void UseDialogue()
    {
        if (index < desc.Length)
        {
            nameText.text = name;
            descText.text = desc[index];
        }

        index++;

        if(index > desc.Length)
        {
            dialogueBox.SetActive(false);
            dialogueIsActive = false;
            transform.GetComponent<PlayerScript>().ToggleMovement(true);
            transform.GetComponent<PlayerScript>().inInteraction = false;
            index = 0;

            if (isCollectible) CollectItem();
            if (isTrigger) temp.UseTrigger();
            if (isExit) GameObject.FindGameObjectWithTag("Finish").GetComponent<ExitPoints>().NextStage();

            ResetData();

        }
    }

    public void RandomDialogue()
    {
        if (!inUse)
        {
            int rand = Random.Range(0, desc.Length-1);
            nameText.text = name;
            descText.text = desc[rand];
            inUse = true;
        }
        else
        {
            inUse = false;

            dialogueBox.SetActive(false);
            dialogueIsActive = false;
            transform.GetComponent<PlayerScript>().ToggleMovement(true);
            transform.GetComponent<PlayerScript>().inInteraction = false;
            index = 0;

            if (isCollectible) CollectItem();
            if (isTrigger) temp.UseTrigger();
            if (isExit) GameObject.FindGameObjectWithTag("Finish").GetComponent<ExitPoints>().NextStage();

            ResetData();
        }
    }

    private void ResetData()
    {
        used = false;
        inUse = false;
        isExit = false;
        hasData = false;
        isRandom = false;
        onlyOnce = false;
        isTrigger = false;
        isCollectible = false;
        dialogueIsActive = false;
    }

    private void CollectItem()
    {
        string item_name = temp.GetName();

        if(item_name == "card")
        {
            GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("GET OUT");
            GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().hasNurseID = true;
        }

        if(item_name != "AttackTell")
        {
            temp.destroySelf();
            temp = null;
        }
        else if(item_name == "AttackTell")
        {
            temp.GetComponentInParent<NursePatrol>().DestroySelf();
        }
    }


}
