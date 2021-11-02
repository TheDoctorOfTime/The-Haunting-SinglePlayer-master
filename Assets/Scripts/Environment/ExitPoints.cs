using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ExitPoints : MonoBehaviour
{
    public bool isUnlocked = false;
    public bool NSL = false;
    public bool start = false;

    [SerializeField] private string[] locked;
    [SerializeField] private string[] unlocked;

    [SerializeField] private GameObject openDoorAsset;

    //PhotonView view;

    private void Start()
    {
        openDoorAsset = GameObject.Find("OpenDoor");
        openDoorAsset.SetActive(false);

        //view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (isUnlocked)
        {
            transform.GetComponent<DialogueData>().desc = unlocked;
            openDoorAsset.SetActive(true);
        }
        else if (!isUnlocked)  transform.GetComponent<DialogueData>().desc = locked;

        //if (NSL) return;

        if (start)
        {
            //view.RPC("GoNext", RpcTarget.MasterClient);
            GoNext();
        }

    }

    public void NextStage()
    {
        if (!isUnlocked) return;
        //view.RPC("startRoom", RpcTarget.AllBuffered);
        startRoom();
    }

    [PunRPC]
    public void startRoom()
    {
        start = true;
    }

    [PunRPC]
    public void GoNext()
    {
        //view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        //PhotonNetwork.LoadLevel("JumpScare");
        /*if (PhotonNetwork.IsMasterClient)
        {
            start = false;
            PhotonNetwork.LoadLevel("Hallway 1");
        }*/
        FindObjectOfType<LevelLoader>().LoadScene("Hallway 1");
    }

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }
}
