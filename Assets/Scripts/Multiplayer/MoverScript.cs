using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MoverScript : MonoBehaviour
{
    [SerializeField] private GameObject hostPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GameObject.FindGameObjectWithTag("Host")) PhotonNetwork.Instantiate(hostPrefab.name, new Vector3(transform.position.x, transform.position.y, -18), Quaternion.identity);
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.transform.position = transform.position;
        }
        
    }

}
