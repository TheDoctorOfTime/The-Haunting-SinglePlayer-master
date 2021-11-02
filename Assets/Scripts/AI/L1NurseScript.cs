using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Photon.Pun;

public class L1NurseScript : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    const float MAX_INTERVAL = 5f;

    float findLocationTimer = MAX_INTERVAL;
    float moveSpeed = 2f;

    Vector2 destination;

    [SerializeField] private GameObject dNurse;

    PhotonView view;

    bool triggerstart = false;
    bool NSL = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag("Player").transform.position;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        /*foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerScript>().ToggleMovement(false);
        }*/

        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        //if (NSL) return;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if ( GameObject.Find("Overlays").GetComponent<OverlayControls>().GSactive == false)
            GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(true);

        if (findLocationTimer >= 0f) findLocationTimer -= Time.deltaTime;
        else
        {
            findLocationTimer = MAX_INTERVAL;
            destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        }

/*        movement.x = destination.x < transform.position.x ? -1 : destination.x > transform.position.x ? 1 : 0;
        movement.y = destination.y < transform.position.y ? -1 : destination.y > transform.position.y ? 1 : 0;*/

        if (triggerstart)
        {
            /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);
            if (PhotonNetwork.IsMasterClient)
                view.RPC("TriggerDeath", RpcTarget.MasterClient);*/
            SceneManager.LoadScene("JumpScare");
        }
    }

    private void FixedUpdate()
    {
        agent.SetDestination(destination);
        agent.speed = moveSpeed;
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().isHiding)
            {
                triggerstart = true;
            }

            moveSpeed = 0f;
            AudioSource footsteps = transform.GetComponent<AudioSource>();
            footsteps.Stop();
            collision.gameObject.GetComponent<PlayerInteraction>().UseDialogue();
        }
    }

    public void DisableSelf()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerScript>().ToggleMovement(true);
        }

        PhotonNetwork.Instantiate(dNurse.name, gameObject.transform.position, Quaternion.identity);

        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(false);
        gameObject.SetActive(false);
    }


    [PunRPC]
    public void TriggerDeath()
    {
        if (NSL) return;

        view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("JumpScare");
        }
    }

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }
}
