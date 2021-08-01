using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoove : MonoBehaviour
{
    [SerializeField] private Camera cameraRay;
    [SerializeField] private Transform cameraContainer;

    private Vector3 startPoint;
    private Vector3 cameraStartPoint;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);


    private void Update() {
        float distance;
        Ray ray = cameraRay.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        float distanceFromCameraCenter;
        Ray rayFromCameraCenter = cameraRay.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        plane.Raycast(rayFromCameraCenter, out distanceFromCameraCenter);
        Vector3 pointFromCameraCenter = rayFromCameraCenter.GetPoint(distanceFromCameraCenter);


        if (Input.GetMouseButtonDown(1)) {
            startPoint = point;
            cameraStartPoint = transform.position;
        }

        if (Input.GetMouseButton(1)) {
            Vector3 offset = point - startPoint;
            transform.position = cameraStartPoint - offset;
        }

        if (Input.GetMouseButtonUp(1)) {
            UpdatePositionCameraContainer();
        }

        if (Input.GetMouseButton(2)) {
            cameraContainer.RotateAround(pointFromCameraCenter, Vector3.up, Input.GetAxis("Mouse X") * 5f);
        }

        if (Input.mouseScrollDelta.y != 0) {
            UpdatePositionCameraContainer();
        }

        if (Input.mouseScrollDelta.y != 0) {
            if (transform.position.y < 20 && Input.mouseScrollDelta.y > 0)
                return;
            if (transform.position.y > 55 && Input.mouseScrollDelta.y < 0)
                return;

            transform.Translate(0, 0, Input.mouseScrollDelta.y * 150 * Time.deltaTime);
            cameraRay.transform.Translate(0, 0, Input.mouseScrollDelta.y * 150 * Time.deltaTime);
            UpdatePositionCameraContainer();
        }
    }

    private void UpdatePositionCameraContainer() {
        cameraContainer.position = transform.position;
        transform.localPosition = Vector3.zero;
        cameraRay.transform.localPosition = Vector3.zero;
    }

}
