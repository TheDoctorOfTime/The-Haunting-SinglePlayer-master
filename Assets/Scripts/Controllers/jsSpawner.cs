using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jsSpawner : MonoBehaviour
{

    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject point;
    private bool used = false;

    private float min = 20f;
    private float max = 45f;
    private float timer = 0;

    private void Start()
    {
        point = transform.Find("spawnPoint").gameObject;

        timer = Random.Range(min, max);
    }

    private void Update()
    {
        if (timer >= 0) timer -= Time.deltaTime;
        else
        {
            if (!used)
            {
                if (GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().ntUsed) return;
                Instantiate(ghost, point.transform.position, Quaternion.identity);
                used = true;
            }
        }
    }
}
