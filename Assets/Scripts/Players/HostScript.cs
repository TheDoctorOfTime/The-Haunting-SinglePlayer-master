using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class HostScript : MonoBehaviour
{
    GameObject cam;

    GameObject[] players;
    int currentlySpectatingPlayer;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
            if (currentlySpectatingPlayer + 1 >= players.Length)
            {
                currentlySpectatingPlayer = 0;
            }
            else
                currentlySpectatingPlayer++;

            cam.transform.parent = null;
            cam.transform.parent = players[currentlySpectatingPlayer].transform;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentlySpectatingPlayer - 1 <= 0)
            {
                currentlySpectatingPlayer = players.Length - 1;
            }
            else
                currentlySpectatingPlayer--;


            cam.transform.parent = null;
            cam.transform.parent = players[currentlySpectatingPlayer].transform;
        }
    }

}
