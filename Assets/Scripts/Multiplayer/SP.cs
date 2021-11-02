using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class SP : MonoBehaviour
{
    [SerializeField] private GameObject hostPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject spawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(hostPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnpoint.transform.position, Quaternion.identity);
        }
    }
}
