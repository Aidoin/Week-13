using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum UnitState {
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}


public class Swordsman : Unit
{
    [Header("Swordsman")]

    [SerializeField] private AudioSource audioAttackUnit;

    public Vector3 TargetPoint;
    public Enemy TargetEnemy;

    public float DistanceToFollow = 10f;
    public float DistanceToAttack = 1f;
    public int DamageAttack = 1;
    public float AttackPeriod = 2f;
    private float timerAttack;

    private UnitState currentState;


    protected override void Start() {
        base.Start();
    }

    private void Update() {
        if (currentState == UnitState.Idle) {
            FindClosestEnemy();
            navMeshAgent.SetDestination(positionStay);

        } else if (currentState == UnitState.WalkToPoint) {
            FindClosestEnemy();
            if (TargetPoint == Vector3.zero) {
                currentState = UnitState.Idle;
            } else {
                navMeshAgent.SetDestination(TargetPoint);
            }

        } else if (currentState == UnitState.WalkToEnemy) {
            FindClosestEnemy();
            if (TargetEnemy == null) {
                currentState = UnitState.Idle;
            } else {
                navMeshAgent.SetDestination(TargetEnemy.transform.position);

                float distanceToUnit = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distanceToUnit > DistanceToFollow) {
                    currentState = UnitState.Idle;
                }
                if (distanceToUnit < DistanceToAttack) {
                    navMeshAgent.SetDestination(transform.position);
                    currentState = UnitState.Attack;
                    timerAttack = 0;
                }
            }

        } else if (currentState == UnitState.Attack) {
            if (TargetEnemy == null) {
                currentState = UnitState.Idle;
            } else {
                Attack();
                float distanceToUnit = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distanceToUnit > DistanceToAttack) {
                    currentState = UnitState.WalkToEnemy;
                }
            }
        }
    }


    private void Attack() {
        timerAttack += Time.deltaTime;
        if (timerAttack > AttackPeriod) {
            timerAttack = 0;
            TargetEnemy.takeDamage(DamageAttack);
            audioAttackUnit.pitch = Random.Range(0.85f, 1.15f);
            audioAttackUnit.Play();
        }
    }

    public void SetState(UnitState unitState) {
        currentState = unitState;

        if (currentState == UnitState.Idle) {


        } else if (currentState == UnitState.WalkToPoint) {


        } else if (currentState == UnitState.WalkToEnemy) {


        } else if (currentState == UnitState.Attack) {
            timerAttack = 0;
        }
    }

    public void FindClosestEnemy() {

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDistance = Mathf.Infinity;
        Enemy ClosestEnemy = null;

        for (int i = 0; i < enemies.Length; i++) {
            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                ClosestEnemy = enemies[i];
            }
        }
        if (minDistance < DistanceToFollow) {
            TargetEnemy = ClosestEnemy;
            SetState(UnitState.WalkToEnemy);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif
}
