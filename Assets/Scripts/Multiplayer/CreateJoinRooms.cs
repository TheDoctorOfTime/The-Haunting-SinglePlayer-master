using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField Password;
    public TMP_InputField JoinInput;

    public void CreateRoom()
    {
        if (Password.text != "21BMHS") return;
        if (JoinInput.text == string.Empty) return;
        //string roomCode = "";
        //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890000000000000";
        //
        //for (int i = 0; i < 6; i++)
        //{
        //    roomCode += chars[Random.Range(0,35)];
        //}
        //PlayerPrefs.SetString("RoomCode", roomCode);
        PhotonNetwork.CreateRoom(JoinInput.text.ToUpper());
    }

    public void JoinRoom()
    {
        //PlayerPrefs.SetString("RoomCode", JoinInput.text.ToUpper());
        PhotonNetwork.JoinRoom(JoinInput.text.ToUpper());
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("WaitingRoom");
    }
}
