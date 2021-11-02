using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpen = false;

    [SerializeField] GameObject openDoorAsset;

    // Start is called before the first frame update
    void Start()
    {
        openDoorAsset = GameObject.Find("OpenDoor");
        openDoorAsset.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen) openDoorAsset.SetActive(true);
        else openDoorAsset.SetActive(false);
    }
}
