using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class WaitingRoom : MonoBehaviour
{
    [SerializeField] TMP_Text code;
    [SerializeField] GameObject host;
    [SerializeField] private Button startButton;

    string roomCode;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(host.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else startButton.gameObject.SetActive(false);

        roomCode = "";
        code.text = roomCode;
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("Stage 1");
        }
    }
}
