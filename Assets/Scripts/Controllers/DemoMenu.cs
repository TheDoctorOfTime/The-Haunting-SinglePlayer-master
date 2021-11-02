using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Stage 1");
    }
}
