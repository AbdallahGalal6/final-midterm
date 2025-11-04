using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float catchDistance = 1.5f;
    public float agentSpeed = 5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("❌ No NavMeshAgent found on " + gameObject.name + "!");
            return;
        }

        agent.speed = agentSpeed;
        agent.stoppingDistance = catchDistance;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
            else
                Debug.LogError("❌ Player not found! Make sure your player has the tag 'Player'.");
        }
    }

    void Update()
    {
        if (agent == null || player == null)
            return;

        // Make the agent follow the player
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // Check if close enough to catch
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= catchDistance)
        {
            Debug.Log("💀 Enemy caught the player!");
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }
}