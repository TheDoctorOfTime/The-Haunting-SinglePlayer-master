using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;

public class Level4_Controller : MonoBehaviour
{
    [SerializeField] private Slider timerDisplay;
    [SerializeField] private TMP_Text interaction_indicator;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    //PhotonView view;
    public bool NSL = false;
    public bool SD = false;
    public bool SR = false;

    void Start()
    {
        timerDisplay = GameObject.Find("TimerIndicator").GetComponent<Slider>();
        interaction_indicator = GameObject.Find("InteractionIndicator").GetComponent<TMP_Text>();

        timerDisplay.gameObject.SetActive(false);
        interaction_indicator.gameObject.SetActive(false);

        dialogueBox = GameObject.Find("DialogueBox");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        descText = GameObject.Find("DescText").GetComponent<TMP_Text>();

        dialogueBox.SetActive(false);

        //view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (NSL) return;

        if (SR) {
            //view.RPC("GoNext4", RpcTarget.MasterClient); 
            GoNext4();
        }
        if (SD) {
            //view.RPC("GoNurseJS", RpcTarget.MasterClient); 
            GoNurseJS();
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

    [PunRPC]
    public void GN4()
    {
        SR = true;
    }

    [PunRPC]
    public void GN4End()
    {
        SD = true;
    }

    [PunRPC]
    public void GoNext4()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Ending");
        }*/

        FindObjectOfType<LevelLoader>().LoadScene("Ending");
    }

    [PunRPC]
    public void GoNurseJS()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("NurseJS");
        }*/

        SceneManager.LoadScene("NurseJS");
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
            case "Win":
                //view.RPC("GN4", RpcTarget.AllBuffered); 
                GN4();
                break;
            case "Lose": 
                //view.RPC("GN4End", RpcTarget.AllBuffered);
                GN4End();
                break;
        }
    }
}
