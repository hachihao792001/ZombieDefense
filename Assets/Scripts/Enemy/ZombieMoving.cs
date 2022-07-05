using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMoving : MonoBehaviour
{
    private readonly int WalkingHash = Animator.StringToHash("Walking");
    private readonly int DeadHash = Animator.StringToHash("Dead");

    [HideInInspector]
    [SerializeField]
    private NavMeshAgent _navMesh;
    [HideInInspector]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _stoppingDistance;

    public Transform Target;

    private void OnValidate()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SeekForTarget), 0f, 1f);
    }

    private void Update()
    {
        if (Target != null)
        {
            if (Vector3.Distance(transform.position, Target.position) <= _stoppingDistance)
            {
                StopMoving();
            }
            else
            {
                _navMesh.SetDestination(Target.position);
                _animator.SetBool(WalkingHash, true);
            }
        }
        else
        {
            StopMoving();
        }
    }

    private void StopMoving()
    {
        _navMesh.SetDestination(transform.position);
        _animator.SetBool(WalkingHash, false);
    }

    private void SeekForTarget()
    {
        if (GameController.Instance.Allies.Count == 0)
            Target = null;
        else
        {
            float[] distancesToAllies = GameController.Instance.GetDistanceToAllies(transform.position);
            int closestAllyIndex = 0;

            for (int i = 1; i < distancesToAllies.Length; i++)
            {
                if (distancesToAllies[i] < distancesToAllies[closestAllyIndex])
                {
                    closestAllyIndex = i;
                }
            }

            Target = GameController.Instance.Allies[closestAllyIndex];
        }
    }
}
