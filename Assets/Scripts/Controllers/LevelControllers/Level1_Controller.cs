using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class Level1_Controller : MonoBehaviour
{
    [SerializeField] private AudioSource steps;
    [SerializeField] private AudioSource knock;
    [SerializeField] private AudioSource keys;
    [SerializeField] private AudioSource open;
    [SerializeField] private AudioSource taps;

    [SerializeField] private GameObject hShow;
    [SerializeField] private GameObject hButton;
    [SerializeField] private GameObject evilNurse;
    [SerializeField] private GameObject evilNurse_dead;
    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private Slider timerDisplay;
    [SerializeField] private TMP_Text interaction_indicator;


    Vector3 locker;
    PhotonView view;

    GameObject nurse;

    float nurseTimer = 10f;
    int nurseStage = 0;
    public bool ntUsed = false;
    public bool nurseSpawned = false;

    float sedationTimer = 5f;
    bool sScene = false;

    bool survived = false;
    bool door_unlocked = false;

    bool dTriggered = false;

    public bool playerHasSyringe = false;
    public bool isHiding = false;

    public bool startTimer = false;
    public bool spawnNurse = false;
    public bool startDeath = false;

    bool NSL = false;
    bool PHS = false;

    public GameObject myPlayer;

    private void Start()
    {
        timerDisplay = GameObject.Find("TimerIndicator").GetComponent<Slider>();
        interaction_indicator = GameObject.Find("InteractionIndicator").GetComponent<TMP_Text>();

        hShow = GameObject.Find("hShow");
        hButton = GameObject.Find("hButton");

        timerDisplay.gameObject.SetActive(false);
        interaction_indicator.gameObject.SetActive(false);

        locker = GameObject.Find("Locker").transform.position;
        //view = GetComponent<PhotonView>();
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (NSL) return;

        if (isHiding)
        {
            hButton.SetActive(true);
            hShow.SetActive(true);
        }
        else
        {
            hButton.SetActive(false);
            hShow.SetActive(false);
        }

        //if (view.IsMine)
        //{

            switch (nurseStage)
            {
                case 2:
                    if (nurseTimer >= 0f) nurseTimer -= Time.deltaTime;
                    else
                    {
                        nurseTimer = 5f;
                        startTimer = true;
                    }
                    break;
                case 3:
                    if (nurseTimer >= 0f) nurseTimer -= Time.deltaTime;
                    else
                    {
                        nurseTimer = 2f;
                        startTimer = true;
                    }
                    break;
                case 4:
                    if (nurseTimer >= 0f) nurseTimer -= Time.deltaTime;
                    else
                    {
                        nurseTimer = 2f;
                        startTimer = true;
                        spawnNurse = true;
                        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            player.GetComponent<PlayerScript>().ToggleMovement(false);
                        }
                        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(true);
                    }
                    break;
                case 5:
                    break;
            //}

            

        }

        if (sScene)
        {
            /*view.RPC("tDisSet", RpcTarget.AllBuffered);
            view.RPC("QuicktimeAction", RpcTarget.AllBuffered);
            view.RPC("timerMove", RpcTarget.AllBuffered);*/

            tDisSet();
            QuicktimeAction();
            timerMove();
        }

        if (startTimer)
        {
            startTimer = false;
            //view.RPC("StartNurseTimer", RpcTarget.AllBuffered);
            StartNurseTimer();

        }
        if (startDeath)
        {
            startDeath = false;
            //view.RPC("TriggerDeath", RpcTarget.MasterClient);
        }
        if (spawnNurse)
        {
            spawnNurse = false;
            //if(PhotonNetwork.IsMasterClient)
            //PhotonNetwork.Instantiate(evilNurse.name, spawnPoint.transform.position, Quaternion.identity);
            Instantiate(evilNurse, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    [PunRPC]
    public void tDisSet()
    {
        timerDisplay.value = sedationTimer;
        interaction_indicator.text = "Press 'E' To Sedate Nurse!";
    }

    [PunRPC]
    public void timerMove()
    {
        if (sedationTimer >= 0f)
        {
            //if (view.IsMine) sedationTimer -= Time.deltaTime;
            sedationTimer -= Time.deltaTime;
        }
        else
        {
            if (!survived)
            {
                startDeath = true;
            }
            else interaction_indicator.text = "Press 'E' to Interact";
        }
    }

    [PunRPC]
    public void QuicktimeAction()
    {
        if (SimpleInput.GetKeyDown(KeyCode.E))
        {
            //sedated
            if(GameObject.FindGameObjectWithTag("L1Nurse"))
            GameObject.FindGameObjectWithTag("L1Nurse").GetComponent<L1NurseScript>().DisableSelf();
            //skip timer
            sedationTimer = 0;

            timerDisplay.gameObject.SetActive(false);

            interaction_indicator.text = "Press 'E' to Interact";

            interaction_indicator.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Finish").GetComponent<ExitPoints>().isUnlocked = true;
            survived = true;
        }
    }

    [PunRPC]
    public void StopHiding()
    {
        //view.RPC("Shiding", RpcTarget.AllBuffered);
        Shiding();
    }

    [PunRPC]
    public void Shiding()
    {
        isHiding = false;
        /*foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.transform.position = GameObject.Find("lockerexit").transform.position;
            player.GetComponent<PlayerScript>().ToggleMovement(true);
        }*/
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        myPlayer.transform.GetComponent<SpriteRenderer>().enabled = true;
        myPlayer.GetComponent<PlayerScript>().ToggleMovement(true);

        toggleInteraction(false);
    }

    [PunRPC]
    private void StartNurseTimer()
    {
        if (!playerHasSyringe) GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Find Syringe");
        nurseStage = nurseStage == 0 ? 1 : nurseStage;
        ntUsed = true;
        if (nurseStage == 0) return;

        print("playing track " + nurseStage);
        //view.RPC("playSound", RpcTarget.AllBuffered, nurseStage);

        playSound(nurseStage);
        nurseStage++;
    }

    [PunRPC]
    private void GiveSyringe()
    {
        if (!playerHasSyringe) playerHasSyringe = true;
        GameObject.Find("Sedation").SetActive(false);
        GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Sedate the nurse");
    }

    [PunRPC]
    private void playSound(int i)
    {
        switch (i)
        {
            case 1: steps.Play(); break; 
            case 2: knock.Play(); steps.Stop();  break; 
            case 3: keys.Play(); break; 
            case 4: open.Play(); break; 
        }
    }

    [PunRPC]
    private void QuickTimeSedation()
    {
        if (playerHasSyringe && !isHiding)
        {
            sScene = true;

            timerDisplay.gameObject.SetActive(true);
            interaction_indicator.gameObject.SetActive(true);
            GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Leave Room");
        }
        else
        {
            //view.RPC("TriggerDeath", RpcTarget.MasterClient);
            startDeath = true;
        }

    }
    
    //Scene Changes
    [PunRPC]
    public void TriggerDeath()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        //PhotonNetwork.LoadLevel("JumpScare");
        if (PhotonNetwork.IsMasterClient)
        {
            startDeath = false;
            PhotonNetwork.LoadLevel("JumpScare");
        }*/

        SceneManager.LoadScene("JumpScare");
    }


    //Hide Player
    [PunRPC]
    public void HidePlayer()
    {
        isHiding = true;

        /*foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.transform.position = new Vector3(0, locker.y, 5f);
            player.GetComponent<PlayerScript>().ToggleMovement(false);
        }*/

        myPlayer = GameObject.FindGameObjectWithTag("Player");
        myPlayer.transform.GetComponent<SpriteRenderer>().enabled = false;
        myPlayer.GetComponent<PlayerScript>().ToggleMovement(false);

        toggleInteraction(false);
    }

    //NSL Trigger/Toggle
    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }

    //=======================================================================

    //Use Functions - External
    [PunRPC]
    public void UseFunction(string fName)
    {
        switch (fName)
        {
            case "StartNurseTimer": 
                if (!ntUsed)
                    //view.RPC("StartNurseTimer", RpcTarget.AllBuffered); 
                    StartNurseTimer();

                break;

            case "GiveSyringe": 
                //view.RPC("GiveSyringe", RpcTarget.AllBuffered);
                GiveSyringe();

                break;

            case "QuickTimeSedation": 
                if (!sScene)
                    //view.RPC("QuickTimeSedation", RpcTarget.AllBuffered);  
                    QuickTimeSedation();

                break;

            case "TriggerDeath": 
                if (!dTriggered)
                    //view.RPC("TriggerDeath", RpcTarget.MasterClient);
                    TriggerDeath();
                break;

            case "HidePlayer":
                //view.RPC("HidePlayer", RpcTarget.AllBuffered);
                HidePlayer();

                break;

        }
    }

    //=======================================================================

    //INTERACTIONS
    [PunRPC]
    public void toggleInteraction(bool val) {
        interaction_indicator.gameObject.SetActive(val);
    }
    [PunRPC]
    public void toggleInteraction(bool val, string text) {
        interaction_indicator.gameObject.SetActive(true);
        interaction_indicator.text = text;

        interaction_indicator.gameObject.SetActive(val);
    }

    //=======================================================================

}
