using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{ 
    Transform target;
    NavMeshAgent agent;
    //For regular patrol
    int currentNode;
    int previousNode;

    //For linear patrol
    int node1;
    int node2;

    [SerializeField] bool linearPatrol;
    public enum EnemyState
    {
        patrol,
        chase
    };

    public enum PatrolState
    {
        regularPatrol,
        linearPatrol
    };

    EnemyState enemyState = EnemyState.patrol;

    // Start is called before the first frame update
    void Start()
    {       
        agent = GetComponent<NavMeshAgent>();
        if (linearPatrol)
        {
            node1 = Random.Range(0, GameManager.gm.nodes.Length);
            node2 = Random.Range(0, GameManager.gm.nodes.Length);
            currentNode = node1;
            previousNode = node2;
        }
        else
        {
            currentNode = Random.Range(0, GameManager.gm.nodes.Length);
            previousNode = currentNode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.patrol: Patrol(); break;
            case EnemyState.chase: Chase(); break;
            default: break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "node")
        {
            if (linearPatrol)
            {
                if (currentNode == node1)
                {
                    currentNode = node2;
                    previousNode = node1;
                }
                else
                {
                    currentNode = node1;
                    previousNode = node2;
                }
            }
            else
            {
                currentNode = Random.Range(0, GameManager.gm.nodes.Length);
                while (currentNode == previousNode)
                {
                    currentNode = Random.Range(0, GameManager.gm.nodes.Length);
                }
                previousNode = currentNode;
            }
        }

        if(other.tag == "Player")
        {
            enemyState = EnemyState.chase;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyState = EnemyState.patrol;
        }
    }

    void Patrol()
    {
        agent.destination = GameManager.gm.nodes[currentNode].position;
    }

    void Chase()
    {
        agent.destination = GameManager.gm.player.transform.position;
    }
}
