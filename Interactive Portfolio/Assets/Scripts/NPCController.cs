using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator animator;

    bool _idleState;

    private Transform _position;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _idleState = false;
    }

    private void Start()
    {
        animator.SetBool("Walking", true);
        int tmp = Random.Range(0, NavigationPositions._positions.Count);
        _position = NavigationPositions._positions[tmp];
        NavigationPositions._positions.RemoveAt(tmp);
        _agent.destination = _position.position;
    }

    private void Update()
    {
        if (_idleState == false && _agent.remainingDistance < 1) 
        {
            StartCoroutine(IdleTime());
        }
    }


    private IEnumerator IdleTime() 
    {
        _idleState = true;
        animator.SetBool("Walking", false);
        FacePosition();

        yield return new WaitForSecondsRealtime(Random.Range(10f,15f));
        animator.SetBool("Walking", true);
        NavigationPositions._positions.Add(_position);

        int tmp = Random.Range(0, NavigationPositions._positions.Count-1);
        _position = NavigationPositions._positions[tmp];
        NavigationPositions._positions.RemoveAt(tmp);
        _agent.destination = _position.position;

        _idleState = false;
        
    }

    private void FacePosition()
    {
        if (_position != null)
        {
            // Get direction to the position (ignore Y-axis difference for level rotation)
            Vector3 direction = (_position.position - transform.position).normalized;
            direction.y = 0; // Keep the rotation level

            if (direction != Vector3.zero)
            {
                // Create rotation to look at the position
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
            }
        }
    }

}
