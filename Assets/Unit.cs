using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObjects
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        navMeshAgent.SetDestination(point);
    }
}
