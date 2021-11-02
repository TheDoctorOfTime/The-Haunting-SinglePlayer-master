using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
	Image backgroundImage;
	public Color highlightColor;
	public GameObject leftArrowButton;
	public GameObject rightArrowButton;

	ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
	public Image playerAvatar;
	public Sprite[] avatars;

	Player player;
	private void Awake()
	{
		backgroundImage = GetComponent<Image>();	
	}

	public void SetPlayerInfo(Player _player)
	{
		player = _player;
		playerName.text = player.NickName;
		UpdatePlayerItem(player);
	}

	public void ApplyLocalChanges()
	{
		backgroundImage.color = highlightColor;
		leftArrowButton.SetActive(true);
		rightArrowButton.SetActive(true);
	}

	public void onClickLeftArrow()
	{
		if ((int)playerProperties["playerAvatar"] == 0)
		{
			playerProperties["playerAvatar"] = avatars.Length - 1;
		}
		else
		{
			playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
		}
		PhotonNetwork.SetPlayerCustomProperties(playerProperties);
	}

	public void onClickRightArrow()
	{
		if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
		{
			playerProperties["playerAvatar"] = 0;
		}
		else
		{
			playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
		}
		PhotonNetwork.SetPlayerCustomProperties(playerProperties);	
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		if (player == targetPlayer)
		{
			UpdatePlayerItem(targetPlayer);
		}
	}

	void UpdatePlayerItem(Player player)
	{
		if (player.CustomProperties.ContainsKey("playerAvatar"))
		{
			playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
			playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
		}
		else
		{
			playerProperties["playerAvatar"] = 0;
		}
	}
}