using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObjects
{
    [Header("Unit")]

    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform healthBarPosition;
    [SerializeField] private GameObject destinationPointEffectPrefab;

    private ParticleSystem destinationPointEffect;
    protected Vector3 positionStay;

    [SerializeField] private Resources price; public Resources Price => price;
    public int Health;
    private int currentHealth; public int CurrentHealth => currentHealth;
    private HealthBar healthBar;


    protected override void Start() {
        base.Start();
        currentHealth = Health;
        healthBar = Instantiate(healthBarPrefab).GetComponent<HealthBar>();
        healthBar.Setup(healthBarPosition);
        if (positionStay == Vector3.zero) {
            positionStay = transform.position;
        }
    }


    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);
        navMeshAgent.SetDestination(point);

        if (!destinationPointEffect)
            destinationPointEffect = Instantiate(destinationPointEffectPrefab).GetComponent<ParticleSystem>();

        destinationPointEffect.transform.position = point + Vector3.up * 0.15f;
        destinationPointEffect.Clear();
        destinationPointEffect.Emit(100);
        positionStay = point;
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
        if (FindObjectOfType<Management>())
            FindObjectOfType<Management>().UnSelect(this);

        if (healthBar)
            Destroy(healthBar.gameObject);

        if (destinationPointEffect)
            Destroy(destinationPointEffect.gameObject);
    }
}
