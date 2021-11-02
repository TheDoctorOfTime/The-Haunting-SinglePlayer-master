using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class HallwayController : MonoBehaviour
{
    [SerializeField] private Slider timerDisplay;
    [SerializeField] private TMP_Text interaction_indicator;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    //PhotonView view;
    bool NSL = false;
    bool startDeath = false;
    bool startRoom = false;

    void Start()
    {
        timerDisplay = GameObject.Find("TimerIndicator").GetComponent<Slider>();
        interaction_indicator = GameObject.Find("InteractionIndicator").GetComponent<TMP_Text>();

        timerDisplay.gameObject.SetActive(false);
        interaction_indicator.gameObject.SetActive(false); 
        GameObject.Find("Objective").GetComponent<Tasklist>().SetObjective("Find Recreational Room");


        dialogueBox = GameObject.Find("DialogueBox");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        descText = GameObject.Find("DescText").GetComponent<TMP_Text>();

        dialogueBox.SetActive(false);

        //view = GetComponent<PhotonView>();
    }

    void Update()
    {
        //if (NSL) return;

        if (startDeath)
        {
            //view.RPC("GotoJ", RpcTarget.MasterClient);
            SceneManager.LoadScene("JumpScare");
        }

        if (startRoom)
        {
            //view.RPC("GotoNR", RpcTarget.MasterClient);
            FindObjectOfType<LevelLoader>().LoadScene("Stage 2");
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


    //=========================================
    [PunRPC]
    public void GotoNextRoom()
    {
        startRoom = true;
    }

    [PunRPC]
    public void GotoJumpscare()
    {
        startDeath = true;
    }

    [PunRPC]
    public void GotoNR()
    {
        //view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        //PhotonNetwork.LoadLevel("JumpScare");
        if (PhotonNetwork.IsMasterClient)
        {
            startDeath = false;
            PhotonNetwork.LoadLevel("Stage 2");
        }
    }

    [PunRPC]
    public void GotoJ()
    {
        //view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        //PhotonNetwork.LoadLevel("JumpScare");
        if (PhotonNetwork.IsMasterClient)
        {
            startDeath = false;
            PhotonNetwork.LoadLevel("JumpScare");
        }
    }

    //=========================================

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }

    public void UseFunction(string fName)
    {
        switch (fName)
        {
            //case "GotoNextRoom": view.RPC("GotoNextRoom", RpcTarget.AllBuffered); break;
            //case "GotoJumpscare": view.RPC("GotoJumpscare", RpcTarget.AllBuffered); break;

            case "GotoNextRoom": GotoNextRoom(); break;
            case "GotoJumpscare": GotoJumpscare(); break;
        }
    }
}
