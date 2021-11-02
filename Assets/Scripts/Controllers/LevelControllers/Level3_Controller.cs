using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class Level3_Controller : MonoBehaviour
{
    public bool hasNurseID = false;
    public bool hasMidozalam = false;

    public bool doorIsLocked = true;
    public bool NSL = false;
    public bool startNextLvl = false;

    [SerializeField] private Slider timerDisplay;
    [SerializeField] private TMP_Text interaction_indicator;
    [SerializeField] private GameObject medCabinet;
    [SerializeField] private GameObject DoorLock;
    [SerializeField] private GameObject NurseID;

    [SerializeField] private AudioSource doorSound;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    //PhotonView view;

    //PATH
    //Find Evidence
    //Look for exit
    //Find something to attack a nurse
    //Get Out

    void Start()
    {
        timerDisplay = GameObject.Find("TimerIndicator").GetComponent<Slider>();
        interaction_indicator = GameObject.Find("InteractionIndicator").GetComponent<TMP_Text>();
        medCabinet = GameObject.Find("ZoomContents");
        
        DoorLock = GameObject.Find("DoorLock");
        NurseID = GameObject.Find("NurseID");

        medCabinet.SetActive(false);
        timerDisplay.gameObject.SetActive(false);
        interaction_indicator.gameObject.SetActive(false);

        NurseID.SetActive(false);
        DoorLock.SetActive(false);

        doorIsLocked = true;
        GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Find Evidence");

        dialogueBox = GameObject.Find("DialogueBox");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        descText = GameObject.Find("DescText").GetComponent<TMP_Text>();

        dialogueBox.SetActive(false);

        //view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (NSL) return;

        if (startNextLvl)
            //view.RPC("GoNext3", RpcTarget.MasterClient);
            GoNext3();
    }

    public void resetDialogue()
    {
        dialogueBox.SetActive(true);
    }

    public void GiveCard()
    {

    }

    public void showMedicineCabinet()
    {
        medCabinet.SetActive(true);
    }

    public void hideMedicineCabinet()
    {
        medCabinet.SetActive(false);
    }

    public void AttackNurse()
    {

    }

    public void toggleInteraction(bool val)
    {
        interaction_indicator.gameObject.SetActive(val);
    }
    public void toggleInteraction(bool val, string text)
    {
        interaction_indicator.gameObject.SetActive(true);
        interaction_indicator.text = text;

        interaction_indicator.gameObject.SetActive(val);
    }

    public void ShowDoorLock()
    {
        if (!doorIsLocked)
            //view.RPC("GN3", RpcTarget.MasterClient);
            GN3();

        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(true);
        DoorLock.SetActive(true);
        if (hasNurseID) NurseID.SetActive(true);
        else
        {
            NurseID.SetActive(false);
            //view.RPC("showObjective", RpcTarget.AllBuffered);
            showObjective();
        }
    }

    public void HideDoorLock()
    {
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(false);
        NurseID.SetActive(false);
        DoorLock.SetActive(false);
    }

    //================

    [PunRPC]
    public void showObjective()
    {
        GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Obtain a nurse's ID");
    }

    //=============

    [PunRPC]
    public void GiveMidozalam()
    {

        //view.RPC("GMidozalam", RpcTarget.AllBuffered);
        GMidozalam();
    }

    [PunRPC]
    public void GMidozalam()
    {
        hasMidozalam = true;
        GameObject.Find("Midozalam").SetActive(false);
        hideMedicineCabinet();
    }

    //=============

    [PunRPC]
    public void UnlockDoor()
    {
        //view.RPC("UDoor_RPC", RpcTarget.AllBuffered);
        UDoor_RPC();
    }

    [PunRPC]
    public void UDoor_RPC()
    {
        GameObject.Find("LevelExitDoor").GetComponent<DoorScript>().isOpen = true;

        doorSound.Play();
        doorIsLocked = false;
        HideDoorLock();
    }

    //=============

    [PunRPC]
    public void GiveFiles()
    {
        GameObject.Find("Alarm").GetComponent<AlarmScript>().StartAlarm();
        GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Find Exit");
    }

    //=============

    [PunRPC]
    public void GN3()
    {
        startNextLvl = true;
    }

    [PunRPC]
    public void GoNext3()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Stage 4");
        }*/

        FindObjectOfType<LevelLoader>().LoadScene("Stage 4");
    }

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }

    //=============

    public void UseFunction(string fName)
    {
        switch (fName)
        {
            case "showMedicineCabinet": showMedicineCabinet(); break;
            case "AttackNurse": AttackNurse(); break;
            case "ShowDoorLock": ShowDoorLock(); break;
            case "HideDoorLock": HideDoorLock(); break;
            case "UnlockDoor": UnlockDoor(); break;
            case "GiveFiles":
                //view.RPC("GiveFiles", RpcTarget.AllBuffered); 
                GiveFiles();
                break;
            case "GiveCard": GiveCard(); break;
        }
    }
}
