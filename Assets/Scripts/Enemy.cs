using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState {
    Idle,
    WalkToBuilding,
    WalkToUnit,
    AttackBuilding,
    AttackUnit
}


public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState;
    public Building TargetBuilding;
    public Unit TargetUnit;
    public int Health;
    public int currentHealth;

    public float DistanceToFollow = 10f;
    public float DistanceToAttack = 1f;
    public int DamageAttack = 1;
    public float AttackPeriod = 2f;
    private float timerAttack;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform healthBarPosition;
    [SerializeField] private AudioSource audioAttackUnit;
    [SerializeField] private AudioSource audioAttackBuilding;

    private HealthBar healthBar;


    private void Start() {
        SetState(EnemyState.WalkToBuilding);

        currentHealth = Health;
        healthBar = Instantiate(healthBarPrefab).GetComponent<HealthBar>();
        healthBar.Setup(healthBarPosition);
    }

    private void Update() {
        if (CurrentState == EnemyState.Idle) {
            FindClosestBuilding();
            FindClosestUnit();

        } else if (CurrentState == EnemyState.WalkToBuilding) {
            FindClosestBuilding();
            FindClosestUnit();

            if (TargetBuilding == null) {
                CurrentState = EnemyState.Idle;
            } else {
                navMeshAgent.SetDestination(TargetBuilding.transform.position);

                float distanceToBuildind = Vector3.Distance(transform.position, TargetBuilding.transform.position);

                if (distanceToBuildind < DistanceToAttack) {
                    navMeshAgent.SetDestination(transform.position);
                    CurrentState = EnemyState.AttackBuilding;
                    //timerAttack = 0;
                }
            }

        } else if (CurrentState == EnemyState.WalkToUnit) {
            FindClosestUnit();

            if (TargetUnit == null) {
                CurrentState = EnemyState.Idle;
            } else {
                navMeshAgent.SetDestination(TargetUnit.transform.position);

                float distanceToUnit = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distanceToUnit > DistanceToFollow) {
                    CurrentState = EnemyState.Idle;
                }
                if (distanceToUnit < DistanceToAttack) {
                    navMeshAgent.SetDestination(transform.position);
                    CurrentState = EnemyState.AttackUnit;
                }
            }

        } else if (CurrentState == EnemyState.AttackUnit) {
            if (TargetUnit == null) {
                CurrentState = EnemyState.Idle;
            } else {
                AttackUnit();
                float distanceToUnit = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distanceToUnit > DistanceToAttack) {
                    CurrentState = EnemyState.WalkToUnit;
                }
            }
        } else if (CurrentState == EnemyState.AttackBuilding) {
            FindClosestUnit();

            if (TargetBuilding == null || TargetBuilding.Alive == false) {
                CurrentState = EnemyState.Idle;
            } else {
                float distanceToBuildind = Vector3.Distance(transform.position, TargetBuilding.transform.position);

                if (distanceToBuildind < DistanceToAttack) {
                    AttackBuilding();
                } else {
                    FindClosestBuilding();
                }
            }
        }
    }


    private void AttackUnit() {
        timerAttack += Time.deltaTime;
        if (timerAttack > AttackPeriod) {
            timerAttack = 0;
            TargetUnit.takeDamage(DamageAttack);
            audioAttackUnit.pitch = Random.Range(0.85f, 1.15f);
            audioAttackUnit.Play();
        }
    }

    private void AttackBuilding() {
        timerAttack += Time.deltaTime;
        if (timerAttack > AttackPeriod) {
            timerAttack = 0;
            TargetBuilding.takeDamage(DamageAttack);
            audioAttackBuilding.pitch = Random.Range(0.85f, 1.15f);
            audioAttackBuilding.Play();
        }
    }

    public void SetState(EnemyState enemyState) {
        CurrentState = enemyState;

        if (CurrentState == EnemyState.Idle) {


        } else if (CurrentState == EnemyState.WalkToBuilding) {
            FindClosestBuilding();
            if (TargetBuilding == null) {
                CurrentState = EnemyState.Idle;
            } else {
                navMeshAgent.SetDestination(TargetBuilding.transform.position);
            }

        } else if (CurrentState == EnemyState.WalkToUnit) {


        } else if (CurrentState == EnemyState.AttackUnit) {


        }
    }

    public void FindClosestBuilding() {

        Building[] buildings = FindObjectsOfType<Building>();
        float minDistance = Mathf.Infinity;
        Building ClosestBuilding = null;

        for (int i = 0; i < buildings.Length; i++) {
            if (buildings[i].Alive) {
                float distance = Vector3.Distance(transform.position, buildings[i].transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    ClosestBuilding = buildings[i];
                }
            }
        }
        TargetBuilding = ClosestBuilding;

        if (TargetBuilding != null) {
            CurrentState = EnemyState.WalkToBuilding;
        }
    }

    public void FindClosestUnit() {

        Unit[] units = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit ClosestUnit = null;

        for (int i = 0; i < units.Length; i++) {
            float distance = Vector3.Distance(transform.position, units[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                ClosestUnit = units[i];
            }
        }
        if (minDistance < DistanceToFollow) {
            TargetUnit = ClosestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }

    public void takeDamage(int damageValue) {
        currentHealth -= damageValue;
        healthBar.SetHealth(currentHealth, Health);

        if (currentHealth <= 0) {
            Debug.Log(name + " Dead");
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        if (healthBar)
            Destroy(healthBar.gameObject);
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
