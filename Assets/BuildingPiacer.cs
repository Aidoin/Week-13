using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPiacer : MonoBehaviour
{
    [SerializeField] private float sizeCell;
    public float SizeCell => sizeCell;


    public Building CurrentBuilding;

    [SerializeField] private Camera camera;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();

    private void Update() {

        if (CurrentBuilding == null)
            return;

        float distance;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / sizeCell;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * sizeCell;

        if(CheckAllow(x,z, CurrentBuilding)) {
            CurrentBuilding.DisplayAcceptablePosition();

            if (Input.GetMouseButtonDown(0)) {
                InstalBuilding(x, z, CurrentBuilding);
                CurrentBuilding.SelectionIndicator.SetActive(false);
                CurrentBuilding.Collider.enabled = true;
                CurrentBuilding = null;
            }
        } else {
            CurrentBuilding.DisplayUnacceptablePosition();
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
    }

    public void CreateBuilding(GameObject buildingPrefab) {
        CurrentBuilding = Instantiate(buildingPrefab).GetComponent<Building>();
        CurrentBuilding.SelectionIndicator.SetActive(true);
        CurrentBuilding.Collider.enabled = false;
    }
}
