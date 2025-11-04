using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 20f;
    public float catchDistance = 1.5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        agent.stoppingDistance = catchDistance;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
            else
                Debug.LogError("❌ Player not found!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true; // Stop if player is out of range
        }

        // Optional: Check distance manually for catching
        if (distance <= catchDistance)
        {
            Debug.Log("❌ Player caught by enemy!");
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }

    // OnTriggerEnter only works if Collider is set as Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("❌ Player caught by enemy (Trigger)!");
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }
}