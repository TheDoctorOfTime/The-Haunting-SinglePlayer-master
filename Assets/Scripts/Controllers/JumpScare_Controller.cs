using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class JumpScare_Controller : MonoBehaviour
{
    [SerializeField] private const float MAX_TIMER = 3f;
    float timer = MAX_TIMER;

    bool ranOnce = false;
    bool NSL = false;

    //PhotonView view;

    void Start()
    {
        //view = GetComponent<PhotonView>();
    }

    void Update()
    {
        //if (NSL) return;

        if (timer >= 0) timer -= Time.deltaTime;
        else
        {
            if (!ranOnce)
            {
                GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGFSActive(true);
                timer = MAX_TIMER;
                ranOnce = true;
                GameObject.Find("GlitchScreen").GetComponent<AudioSource>().Play();
            }
            else
            {
                //PhotonNetwork.AutomaticallySyncScene = true;
                timer = -1f;
                //if (!PhotonNetwork.IsMasterClient) return;
                //view.RPC("TriggerDeath", RpcTarget.MasterClient);
                TriggerDeath();
            }
        }
    }


    [PunRPC]
    public void TriggerDeath()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Stage 1");
        }*/
        SceneManager.LoadScene("Stage 1");
    }

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }
}
