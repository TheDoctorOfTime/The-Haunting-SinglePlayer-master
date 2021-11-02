using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordScript : MonoBehaviour
{
    [SerializeField] private GameObject cw;
    [SerializeField] private GameObject eb;

    void Start()
    {
        cw = GameObject.Find("crossword");
        eb = GameObject.Find("ExitCW");

        ToggleCW(false);
    }

    private void ToggleCW(bool val)
    {
        cw.SetActive(val);
        eb.SetActive(val);
    }

    public void HideCW()
    {
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(false);
        ToggleCW(false);
    }

    public void ShowCW()
    {
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(true);
        ToggleCW(true);
    }
}
