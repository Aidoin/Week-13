using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPiacer : MonoBehaviour
{
    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();

    [SerializeField] private float sizeCell; public float SizeCell => sizeCell;
    [SerializeField] private Camera camera;
    [SerializeField] private ResourceManager resourceManager;

    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Building currentBuilding;
    private Management management;
    private AdText adText;


    private void Start() {
        resourceManager = FindObjectOfType<ResourceManager>();
        management = FindObjectOfType<Management>();
        adText = FindObjectOfType<AdText>();
    }

    private void Update() {
        BuildingConstruction();
    }


    public void BuyBuilding(GameObject building) {
        Resources prise = building.GetComponent<Building>().Price;
        if (resourceManager.Buy(prise)) {
            CreateBuilding(building);
        } else {
            adText.Show("Не хватает " + Resources.ShowNegativeNumbers(resourceManager.Resources - prise), 1, 0.2f, 0.2f);
        }
    }

    public void CreateBuilding(GameObject building) {
        currentBuilding = Instantiate(building).GetComponent<Building>();
        currentBuilding.SelectionIndicator.SetActive(true);
        currentBuilding.Collider.enabled = false;
        currentBuilding.NavMeshObstacle.enabled = false;
        management.SetState(ManagementState.BuildingPiacing);
    }

    private bool IsEven(int a) {
        return (a % 2) == 0;
    }

    public void BuildingConstruction() {
        if (currentBuilding == null) return;

        float distance;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / sizeCell;

        int x, z;
        if (IsEven(currentBuilding.XSize)) {
            x = Mathf.CeilToInt(point.x);
        } else {
            x = Mathf.CeilToInt(point.x - sizeCell / 4);
        }
        if (IsEven(currentBuilding.ZSize)) {
            z = Mathf.CeilToInt(point.z);
        } else {
            z = Mathf.CeilToInt(point.z - sizeCell / 4);
        }
        currentBuilding.transform.position = new Vector3(x - currentBuilding.XSize / 2, 0, z - currentBuilding.ZSize / 2) * sizeCell;

        bool isInstallationPossible = false;

        if (CheckAllow(x, z, currentBuilding)) {
            currentBuilding.DisplayAcceptablePosition();
            isInstallationPossible = true;
        } else {
            currentBuilding.DisplayUnacceptablePosition();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (isInstallationPossible) {
                InstalBuilding(x, z, currentBuilding);
                currentBuilding.SelectionIndicator.SetActive(false);
                currentBuilding.Collider.enabled = true;
                currentBuilding = null;
            } else {
                adText.Show("Установка в данном месте невозможна", 2, 0.2f, 0.2f);
            }
        }
    }

    private bool CheckAllow(int xPosition, int zPosition, Building building) {
        for (int x = 0; x < building.XSize; x++) {
            for (int z = 0; z < building.ZSize; z++) {

                Vector2Int Coordinate = new Vector2Int(xPosition + x, zPosition + z);

                if (BuildingDictionary.ContainsKey(Coordinate)) {
                    return false;
                }
            }
        }
        return true;
    }

    private void InstalBuilding(int xPosition, int zPosition, Building building) {
        for (int x = 0; x < building.XSize; x++) {
            for (int z = 0; z < building.ZSize; z++) {

                Vector2Int Coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingDictionary.Add(Coordinate, building);
            }
        }
        currentBuilding.NavMeshObstacle.enabled = true;
        StartCoroutine(ReturnState());
        //management.SetState(ManagementState.Default);
    }

    private IEnumerator ReturnState() {
        while (true) {
            if (!Input.GetMouseButton(0))
                break;
            yield return null;
        }
        management.SetState(ManagementState.Default);
    }
}
