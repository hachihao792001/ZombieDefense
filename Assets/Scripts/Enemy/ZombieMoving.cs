using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMoving : MonoBehaviour
{
    private readonly int WalkingHash = Animator.StringToHash("Walking");

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
        Init();
    }

    public void Init()
    {
        enabled = true;
        StartCoroutine(SeekForTarget());
    }

    private void Update()
    {
        if (Target != null)
        {

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _stoppingDistance))
            {
                if (hitInfo.transform == Target)
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

    IEnumerator SeekForTarget()
    {
        while (true)
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

            yield return new WaitForSeconds(1f);
        }
    }

    public void OnDied()
    {
        StopCoroutine(SeekForTarget());
        Target = null;
        StopMoving();
        enabled = false;
    }

    public void WarpAgent(Vector3 pos)
    {
        _navMesh.Warp(pos);
    }
}
