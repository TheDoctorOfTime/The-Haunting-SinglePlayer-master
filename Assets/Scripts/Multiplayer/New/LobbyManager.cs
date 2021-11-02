using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;

	public GameObject roomItemPrefab;
	[SerializeField] List<RoomItem> roomItems = new List<RoomItem>();
	public Transform slidebarContent;

	public GameObject postProcessing;
	public float minReloadTime = 1.5f;
	float nextReloadTime;

	List<PlayerItem> playerItems = new List<PlayerItem>();
	public PlayerItem playerItemPrefab;
	public Transform playerItemParent;

	public GameObject playButton;
	private void Start()
	{
		PhotonNetwork.JoinLobby();
	}

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
		{
			playButton.SetActive(true);
		}
		else
		{
			playButton.SetActive(false);
		}
	}

	public void OnClickCreate()
	{
		if (roomInputField.text.Length >= 1)
		{
			PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions { MaxPlayers = 6, BroadcastPropsChangeToAll = true });
		}
	}

	public override void OnJoinedRoom()
	{
		lobbyPanel.SetActive(false);
		roomPanel.SetActive(true);

		postProcessing.SetActive(true);
		roomName.text = PhotonNetwork.CurrentRoom.Name.ToLower();

		UpdatePlayerList();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		if (Time.time >= nextReloadTime)
		{
			UpdateRoomList(roomList);
			nextReloadTime = Time.time + minReloadTime;
		}
	}

	void UpdateRoomList(List<RoomInfo> list)
	{
		foreach (RoomItem item in roomItems)
		{
			Destroy(item.gameObject);
		}
		roomItems.Clear();

		foreach (RoomInfo room in list)
		{
			RoomItem newRoom = Instantiate(roomItemPrefab, slidebarContent).GetComponent<RoomItem>();
			newRoom.SetRoomName(room.Name);
			roomItems.Add(newRoom);
		}
	}

	public void JoinRoom(string roomID)
	{
		PhotonNetwork.JoinRoom(roomID);
	}

	public void onClickLeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		roomPanel.SetActive(false);
		lobbyPanel.SetActive(true);
		postProcessing.SetActive(false);
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public void UpdatePlayerList()
	{
		foreach (PlayerItem item in playerItems)
		{
			Destroy(item);
		}
		playerItems.Clear();

		if (PhotonNetwork.CurrentRoom == null)
			return;

		foreach (KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
		{
			PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
			newPlayerItem.SetPlayerInfo(player.Value);
			if (player.Value == PhotonNetwork.LocalPlayer)
			{
				newPlayerItem.ApplyLocalChanges();
			}
			playerItems.Add(newPlayerItem);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
		newPlayerItem.SetPlayerInfo(newPlayer);
		if (newPlayer == PhotonNetwork.LocalPlayer)
		{
			newPlayerItem.ApplyLocalChanges();
		}
		playerItems.Add(newPlayerItem);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdatePlayerList();
	}

	public void onClickPlayButton()
	{
		PhotonNetwork.LoadLevel("Stage 1");
	}
}
