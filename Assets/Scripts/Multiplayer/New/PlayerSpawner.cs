using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
	public GameObject[] playerPrefabs;
	public Transform[] spawnPoints;

	public GameObject hostPrefab;

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Instantiate(hostPrefab);
		}
		else
		{
			int random = Random.Range(0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints[random];
			GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
			PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
		}
	}
}
