using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator animator;

    bool _idleState;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _idleState = false;
    }

    private void Start()
    {
        animator.SetBool("Walking", true);
        _agent.destination = NavigationPositions._positions[Random.Range(0, NavigationPositions._positions.Length)].position;
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
        yield return new WaitForSecondsRealtime(Random.Range(10f,15f));
        animator.SetBool("Walking", true);
        _agent.destination = NavigationPositions._positions[Random.Range(0, NavigationPositions._positions.Length)].position;
        _idleState = false;
    }

}
