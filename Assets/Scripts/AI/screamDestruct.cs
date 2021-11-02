using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class screamDestruct : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] AudioSource scare;
	Vector3 destination;

    bool hasStarted = false;

    float timer = 2f;
    float moveSpeed = 3f;

    Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        scare = transform.GetComponent<AudioSource>();
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        destination = GameObject.FindGameObjectWithTag("Player").transform.position;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (GameObject.Find("LEVEL_CONTROLLER").GetComponent<Level1_Controller>().ntUsed)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            destination = GameObject.FindGameObjectWithTag("Player").transform.position;
		}
		else
		{
            destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerScript>().inInteraction == true) return;
        }

/*        rawX = target.transform.position.x < transform.position.x ? -1 : target.transform.position.x > transform.position.x ? 1 : 0;
        rawY = target.transform.position.y < transform.position.y ? -1 : target.transform.position.y > transform.position.y ? 1 : 0;

        movement.x = rawX;
        movement.y = rawY;*/

        if (!hasStarted) return;

        if (timer >= 0f) timer -= Time.deltaTime;
        else
        {
            moveSpeed = 0;
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) player.GetComponent<PlayerScript>().ToggleMovement(true);
            GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGFSActive(false);
            Destroy(gameObject);
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
            if (hasStarted) return;

            collision.GetComponent<PlayerScript>().ToggleMovement(false);
            scare.Play();
            GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGFSActive(true);
            hasStarted = true;

        }
    }
}
