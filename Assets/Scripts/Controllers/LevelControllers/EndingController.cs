using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class EndingController : MonoBehaviour
{
    [SerializeField] private Slider timerDisplay;
    [SerializeField] private TMP_Text interaction_indicator;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;

    //PhotonView view;
    public bool NSL = false;

    public float timer = 6f;
    public bool glitching = false;

    public bool startEnding = false;

    private bool runOnce = false;

    void Start()
    {
        timerDisplay = GameObject.Find("TimerIndicator").GetComponent<Slider>();
        interaction_indicator = GameObject.Find("InteractionIndicator").GetComponent<TMP_Text>();

        timerDisplay.gameObject.SetActive(false);
        interaction_indicator.gameObject.SetActive(false);

        /*dialogueBox = GameObject.Find("DialogueBox");
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();
        descText = GameObject.Find("DescText").GetComponent<TMP_Text>();*/

        //dialogueBox.SetActive(false);

        //view = GetComponent<PhotonView>();
    }

    void Update()
    {
        //if (NSL) return;
        if (!runOnce)
        {
            GameObject.Find("Overlays").GetComponent<OverlayControls>().SetBSActive(true);
            runOnce = true;
        }

        if (timer >= 0f) timer -= Time.deltaTime;
        else
        {
            timer = glitching ? Random.Range(3f, 6f) : 0.4f;
            glitching = !glitching;

            GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(glitching);
        }

        if (startEnding)
            //view.RPC("GameEnding", RpcTarget.MasterClient);
            GameEnding();
    }

    /*public void resetDialogue()
    {
        dialogueBox.SetActive(true);
    }*/

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
    public void TriggerEnding()
    {
        startEnding = true;
    }

    [PunRPC]
    public void GameEnding()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            startEnding = false;
            PhotonNetwork.LoadLevel("GameEnd");
        }*/

        FindObjectOfType<LevelLoader>().LoadScene("GameEnd");
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
            case "TriggerEnding": 
                //view.RPC("TriggerEnding", RpcTarget.AllBuffered);
                TriggerEnding();
                break;
        }
    }

}
