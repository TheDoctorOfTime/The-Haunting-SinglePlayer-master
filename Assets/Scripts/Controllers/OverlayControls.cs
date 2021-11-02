using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayControls : MonoBehaviour
{
    [SerializeField] GameObject GS;
    [SerializeField] GameObject FS;
    [SerializeField] GameObject BS;

    public bool GSactive;
    public bool FSactive;
    public bool BSactive;

    void Start()
    {
        GS = GameObject.Find("GlitchScreen");
        FS = GameObject.Find("FaceScreen");
        BS = GameObject.Find("FadeBlack");
        GS.SetActive(false);
        FS.SetActive(false);
        BS.SetActive(false);
    }

    public void SetGSActive(bool val)
    {
        GS.SetActive(val);
        GSactive = val;
    }

    public void SetFSActive(bool val)
    {
        FS.SetActive(val);
        FSactive = val;
    }

    public void SetGFSActive(bool val)
    {
        SetGSActive(val);
        SetFSActive(val);
    }

    public void SetBSActive(bool val)
    {
        BS.SetActive(val);
        BSactive = val;
    }
}
