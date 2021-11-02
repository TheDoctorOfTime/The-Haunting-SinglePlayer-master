using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class NursePatrol : MonoBehaviour
{
    public const float SPEED = 2.2f;

    public float moveSpeed = SPEED;
    public Rigidbody2D rb;
    public bool movementIsActive = true;

    private bool facingRight = true;
    private bool movingRight = true;
    private bool NSL = false;

    private Animator animator;

    [SerializeField] Vector2 movement;
    [SerializeField] GameObject DeadNurse;
    [SerializeField] GameObject NurseCard;

    bool killPlayer = false;
    float kTimer = 3f;

    //PhotonView view;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        moveSpeed = SPEED;

        animator = transform.GetComponent<Animator>();
        //view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (NSL) return;
        movement.x = movingRight ? 1 : -1;

        if (killPlayer)
        {
            if (kTimer >= 0f) kTimer -= Time.deltaTime;
            else
            {
                moveSpeed = 0f;
                //view.RPC("LoadNurseJS", RpcTarget.MasterClient);
                LoadNurseJS();
            }
        }

        if (killPlayer) return;
        transform.GetComponent<SpriteRenderer>().flipX = movingRight ? false : true;
        transform.GetComponent<BoxCollider2D>().offset = movingRight ? new Vector2(1.91204f, 0.1470025f) : new Vector2(-1.91204f, 0.1470025f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Changer"))
        {
            movingRight = !movingRight;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "collider") return;
        if (collision.gameObject.CompareTag("Player"))
        {
            print("called");
            //view.RPC("StartJumpscare", RpcTarget.MasterClient);
            StartJumpscare();
        }
    }

    [PunRPC]
    private void StartJumpscare()
    {
        killPlayer = true;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerScript>().ToggleMovement(false);
            moveSpeed = 0f;
        }
        Destroy(transform.GetComponent<BoxCollider2D>());
    }

    [PunRPC]
    public void DestroySelf()
    {
        //view.RPC("RPC_DestroySelf", RpcTarget.AllBuffered);
        RPC_DestroySelf();
    }

    [PunRPC]
    public void RPC_DestroySelf()
    {
        if (!GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().hasMidozalam)
            //view.RPC("StartJumpscare", RpcTarget.MasterClient);
            StartJumpscare();

        /*if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(DeadNurse.name, gameObject.transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate(NurseCard.name, gameObject.transform.position, Quaternion.identity);
        }*/
        Instantiate(DeadNurse, gameObject.transform.position, Quaternion.identity);
        Instantiate(NurseCard, gameObject.transform.position, Quaternion.identity);

        GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level3_Controller>().hasMidozalam = false;
        //PhotonNetwork.Destroy(gameObject);
        Destroy(gameObject);
    }

    [PunRPC]
    public void LoadNurseJS()
    {
        /*view.RPC("NewSceneLoaded", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("NurseJS");
        }*/

        SceneManager.LoadScene("NurseJS");
    }

    [PunRPC]
    public void NewSceneLoaded()
    {
        NSL = true;
    }
}
