using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class Level2_Controller : MonoBehaviour
{
    private bool isUnlocked = false;
    private bool startNext = false;
    private bool NSL = false;

    [SerializeField] private Slider timerDisplay;
    [SerializeField] private TMP_Text interaction_indicator;

    [SerializeField] private AudioSource doorUnlock;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    public bool doorLocked = true;
    //PhotonView view;

    void Start()
    {
        timerDisplay = GameObject.Find("TimerIndicator").GetComponent<Slider>();
        interaction_indicator = GameObject.Find("InteractionIndicator").GetComponent<TMP_Text>();

        timerDisplay.gameObject.SetActive(false);
        interaction_indicator.gameObject.SetActive(false); 
        GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Find Door Password");

        dialogueBox = GameObject.Find("DialogueBox");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        descText = GameObject.Find("DescText").GetComponent<TMP_Text>();

        dialogueBox.SetActive(false);

        NSL = false;
        startNext = false;

        //view = GetComponent<PhotonView>();
    }

    void Update()
    {
        //if (NSL) return;

        if (startNext)
        {
            startNext = false;
            //view.RPC("GotoNextR", RpcTarget.MasterClient);
            GotoNextR();
        }
    }

    public void resetDialogue()
    {
        dialogueBox.SetActive(true);
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

    public void showWordCross()
    {
        transform.GetComponent<CrosswordScript>().ShowCW();
    }

    [PunRPC]
    public void showDoorLock()
    {
        if (isUnlocked)
        {
            //view = GetComponent<PhotonView>();
            //view.RPC("GoNext", RpcTarget.MasterClient);
            GoNext();

            return;
        }

        transform.GetComponent<CombinationScript>().showDoorLock();
    }

    [PunRPC]
    public void unlockDoor()
    {
        //view.RPC("UD", RpcTarget.AllBuffered);
        UD();
    }

    [PunRPC]
    public void UD()
    {
        doorUnlock.Play();
        GameObject.Find("Door").GetComponent<DoorScript>().isOpen = true;
        isUnlocked = true;
    }

    [PunRPC]
    public void GoNext()
    {
        startNext = true;
    }

    [PunRPC]
    public void GotoNextR()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            startNext = false;
            PhotonNetwork.LoadLevel("Stage 3");
        }*/
        FindObjectOfType<LevelLoader>().LoadScene("Stage 3");
    }

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }


    public void UseFunction(string fName)
    {
        switch (fName)
        {
            case "showWordCross": 
                showWordCross();  
                break;

            case "showDoorLock":  
                showDoorLock();
                break;
        }
    }
}
