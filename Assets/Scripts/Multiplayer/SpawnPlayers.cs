using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject hostPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject spawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                PhotonNetwork.Destroy(player);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if(!GameObject.FindGameObjectWithTag("Host")) PhotonNetwork.Instantiate(hostPrefab.name, new Vector3(0, 0, -18), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnpoint.transform.position, Quaternion.identity);
        }
    }
}
