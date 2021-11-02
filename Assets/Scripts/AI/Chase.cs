using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    [SerializeField] static Transform target;
    [SerializeField] float patrolSpeed;
    [SerializeField] float chasingSpeed;
    [SerializeField] float patrolRadius;
    [Header("Read Only")]
    [SerializeField] bool patrol;
    [SerializeField] bool moving;
    [SerializeField] CircleCollider2D eyesStrength;

    float normalEyeStrength;
    NavMeshAgent agent;
    Vector3 desiredLocation;
    // Start is called before the first frame update
    void Start()
    {
		if (target == null)
		{
            target = GameObject.FindGameObjectWithTag("Player").transform;
		}
        if (eyesStrength == null)
        {
            eyesStrength = GetComponent<CircleCollider2D>();
        }

        normalEyeStrength = eyesStrength.radius;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);

		if (!moving)
		{
            agent.speed = patrolSpeed;
            eyesStrength.radius = normalEyeStrength;

            desiredLocation = RandomNavmeshLocation(patrolRadius);
			agent.SetDestination(desiredLocation);
            moving = true;
		}
		else
		{
			if (Vector2.Distance(transform.position, desiredLocation) <= 2.0f)
                moving = false;
			else
				agent.SetDestination(desiredLocation);
		}
	}

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
		{
            desiredLocation = collision.transform.position;
            agent.speed = chasingSpeed;
            eyesStrength.radius *= 1.5f;
        }
    }
    private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, patrolRadius);
	}
}
