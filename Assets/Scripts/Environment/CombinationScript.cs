using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class CombinationScript : MonoBehaviour
{
    [SerializeField] GameObject combLock;
    [SerializeField] TMP_InputField password;

    char[] output = new char[4];
    string final = string.Empty;

    bool updated = false;

    PhotonView view;

    private void Start()
    {
        combLock = GameObject.Find("DoorLock");
        password = GameObject.Find("input_pw").GetComponent<TMP_InputField>();

        combLock.SetActive(false);

        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!updated) return;

        final = "";
        foreach(char x in output) if (x != '\0') final += x;

        password.text = final;

        updated = false;
    }

    public void add1()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '1';
                updated = true;
                return;
            }
        }
    }

    public void add2()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '2';
                updated = true;
                return;
            }
        }
    }

    public void add3()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '3';
                updated = true;
                return;
            }
        }
    }

    public void add4()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '4';
                updated = true;
                return;
            }
        }
    }

    public void add5()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '5';
                updated = true;
                return;
            }
        }
    }

    public void add6()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '6';
                updated = true;
                return;
            }
        }
    }

    public void add7()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '7';
                updated = true;
                return;
            }
        }
    }

    public void add8()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '8';
                updated = true;
                return;
            }
        }
    }

    public void add9()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '9';
                updated = true;
                return;
            }
        }
    }

    public void add0()
    {
        for(int i = 0; i < 4; i++)
        {
            if(output[i] == '\0')
            {
                output[i] = '0';
                updated = true;
                return;
            }
        }
    }

    public void clear()
    {
        for(int i = 0; i < 4; i++)
        {
            output[i] = '\0';
        }
        updated = true;
    }

    public void check()
    {
        string send = password.text;
        if (send == "5666")
        {
            //view.RPC("UnlockDoor", RpcTarget.AllBuffered);
            UnlockDoor();
        }
    }

    [PunRPC]
    public void UnlockDoor()
    {
        /*view = GetComponent<PhotonView>();
        view.RPC("UD", RpcTarget.AllBuffered);*/

        GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level2_Controller>().unlockDoor();
        hideDoorLock();
    }

    public void LockToggle(bool val)
    {
        combLock.SetActive(val);
    }

    public void showDoorLock()
    {
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(true);
        transform.GetComponent<CombinationScript>().LockToggle(true);
        clear();
    }

    public void hideDoorLock()
    {
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(false);
        transform.GetComponent<CombinationScript>().LockToggle(false);
    }
}
