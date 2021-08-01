using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : SelectableObjects
{
    [Header("Building")]

    [SerializeField] private Resources price; public Resources Price => price;
    [SerializeField] private int xSize = 3; public int XSize => xSize;
    [SerializeField] private int zSize = 3; public int ZSize => zSize;
    [SerializeField] private Collider collider; public Collider Collider => collider;
    [SerializeField] private NavMeshObstacle navMeshObstacle; public NavMeshObstacle NavMeshObstacle => navMeshObstacle;
    [SerializeField] private Renderer renderer;
    [SerializeField] protected MenuBuilding menuBuilding;
    [SerializeField] protected AudioSource audioDestroy;
    [SerializeField] protected GameObject effectDestroy;

    private int currentHealth; public int CurrentHealth => currentHealth;
    private Color startColor = Color.white;
    public int Health;


    protected virtual void Awake() {
        startColor = renderer.material.color;
    }

    protected override void Start() {
        base.Start();
        currentHealth = Health;
    }


    private void OnDrawGizmos() {
        float sizeCell = FindObjectOfType<BuildingPiacer>().SizeCell;
        for (int x = 0; x < xSize; x++) {
            for (int z = 0; z < zSize; z++) {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * sizeCell, new Vector3(1, 0, 1) * sizeCell);
            }
        }
    }

    public void DisplayUnacceptablePosition() {
        renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition() {
        renderer.material.color = startColor;
    }

    public override void Select() {
        base.Select();
        menuBuilding.Show();
    }

    public override void UnSelect() {
        base.UnSelect();
        menuBuilding.Hide();
    }


    public virtual void takeDamage(int damageValue) {
        if (Alive) {
            currentHealth -= damageValue;

            if (currentHealth <= 0) {
                Debug.Log(name + " Dead");
                StartCoroutine(DerstroyBuilding());
            }
        }
    }


    private IEnumerator DerstroyBuilding() {
        DeathSelectable();
        UnSelect();
        FindObjectOfType<Management>().UnSelect(this);
        effectDestroy.SetActive(true);
        audioDestroy.Play();

        while (gameObject.transform.position.y > -15) {
            gameObject.transform.Translate(Vector3.down * 0.05f);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(gameObject.transform.position.y);

        Destroy(gameObject);
    }
}
