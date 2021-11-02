using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualControls : MonoBehaviour
{
    public GameObject mController;

    public bool visible = false;

    void Start()
    {
        mController = GameObject.Find("MobileControls");
        mController.SetActive(false);
    }

    public void ToggleControls()
    {
        visible = !visible;
        mController.SetActive(visible);
    }
}
