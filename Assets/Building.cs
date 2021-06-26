using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : SelectableObjects
{

    [SerializeField] private int price;
    public int Price => price;

    [SerializeField] private int xSize = 3;
    public int XSize => xSize;

    [SerializeField] private int zSize = 3;
    public int ZSize => zSize;

    [SerializeField] private Collider collider;
    public Collider Collider => collider;

    [SerializeField] private Renderer renderer;



    private Color startColor = Color.white;
    protected MenuBuilding menuBuilding;


    private void Awake() {
        startColor = renderer.material.color;
        menuBuilding = FindObjectOfType<MenuBuilding>();
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
        menuBuilding.gameObject.SetActive(true);
    }

    public override void UnSelect() {
        base.UnSelect();
        menuBuilding.gameObject.SetActive(true);
    }
}
