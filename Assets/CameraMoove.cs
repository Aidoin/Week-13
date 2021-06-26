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


    private void Start() {
        
    }

    private void Update() {

        float distance;
        Ray ray = cameraRay.ScreenPointToRay(Input.mousePosition);

        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

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
            cameraContainer.Rotate(0, Input.GetAxis("Mouse X") * 500 * Time.deltaTime, 0);
        }

        if(Input.mouseScrollDelta.y != 0) {
            UpdatePositionCameraContainer();
        }

        transform.Translate(0, 0, Input.mouseScrollDelta.y * 200 * Time.deltaTime);
        cameraRay.transform.Translate(0, 0, Input.mouseScrollDelta.y * 200 * Time.deltaTime);
    }

    private void UpdatePositionCameraContainer() {
        cameraContainer.position = transform.position;
        transform.localPosition = Vector3.zero;
        cameraRay.transform.localPosition = Vector3.zero;
    }

}
