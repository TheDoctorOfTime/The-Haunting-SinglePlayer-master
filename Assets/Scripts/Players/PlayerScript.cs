using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    public const float PLAYER_SPEED = 2.4f;

    public float moveSpeed = PLAYER_SPEED;
    public Rigidbody2D rb;
    public bool movementIsActive = true;
    public bool inInteraction = false;

    private bool facingRight = true;

    private Animator animator;

    [SerializeField] Vector2 movement;

    //PhotonView view;
    GameObject cam;

    private void Start()
    {
        //view = GetComponent<PhotonView>();

        rb = transform.GetComponent<Rigidbody2D>();
        moveSpeed = PLAYER_SPEED;

        animator = transform.GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        //SceneSwitched
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera");
        }

        //if (!view.IsMine) return;

        movement.x = SimpleInput.GetAxis("Horizontal") > 0 ? 1f : SimpleInput.GetAxis("Horizontal") < 0 ? -1f : 0;
        movement.y = SimpleInput.GetAxis("Vertical") > 0 ? 1f : SimpleInput.GetAxis("Vertical") < 0 ? -1f : 0;

        if (movement.y > 0) 
        {
            animator.SetBool("movingUp", true);
            animator.SetBool("movingDown", false);
            animator.SetBool("moving", false);

            animator.SetBool("isMoving", true);
        } 
        else if (movement.y < 0)
        {
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", true);
            animator.SetBool("moving", false);

            animator.SetBool("isMoving", true);
        }
        else if (movement.x != 0)
        {
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
            animator.SetBool("moving", true);

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
            animator.SetBool("moving", false);

            animator.SetBool("isMoving", false);
        }

        if (movement.x < 0 && movementIsActive && facingRight)
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
            facingRight = false;
        }
        else if (movement.x > 0 && movementIsActive && !facingRight)
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
            facingRight = true;
        }

        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -18);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void ToggleMovement(bool canMove)
    {
        moveSpeed = canMove ? PLAYER_SPEED : 0f;
        movementIsActive = canMove;
    }
}
