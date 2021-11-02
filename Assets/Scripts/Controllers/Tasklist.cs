using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Tasklist : MonoBehaviour
{
    [SerializeField] private TMP_Text myObjective;

    void Start()
    {
        myObjective = GameObject.Find("Objective").GetComponent<TMP_Text>();
        SetObjective("");
    }

    public void SetObjective(string objective)
    {
        myObjective.text = "Current Objective: " + objective + "";
    }
}
