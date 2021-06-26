using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private SelectableObjects howerd;
    private List<SelectableObjects> listOfSelected = new List<SelectableObjects>();

    [SerializeField] private Image selectrdFrame;
    private Vector2 frameStart;
    private Vector2 frameEnd;
    private List<Unit> unitInFrame = new List<Unit>();

    private Vector3 startClick = Vector3.zero;

    private bool selectedBuilding;


    private void Update() {

        Selection();
        UnitsSelectionFrame();

    }

    private void Selection() {

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.GetComponent<SelectableCollider>()) {

                SelectableObjects hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObjects;
                if (howerd) {
                    if (howerd != hitSelectable) {
                        howerd.OnUnhover();
                        howerd = hitSelectable;
                        howerd.OnHover();
                    }
                } else {
                    howerd = hitSelectable;
                    howerd.OnHover();
                }

            } else {
                UnhoverCurrent();
            }
        } else {
            UnhoverCurrent();
        }


        if (Input.GetMouseButtonUp(0)) {
            if (Input.GetKey(KeyCode.LeftControl) == false) {
                UnSelectAll();
            }

            if (howerd) {
                Select(howerd);
            }
        }


        if (Input.GetMouseButtonDown(1)) {
            startClick = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1)) {
            if (startClick == Input.mousePosition) {
                if (hit.collider.tag == "Ground") {

                    for (int i = 0; i < listOfSelected.Count; i++) {
                        listOfSelected[i].WhenClickOnGround(hit.point);
                    }
                }
            }
        }
    }

    private void UnitsSelectionFrame() {

        if (Input.GetMouseButtonDown(0)) {
            frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {

            frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(frameStart, frameEnd);
            Vector2 max = Vector2.Max(frameStart, frameEnd);
            Vector2 size = max - min;

            if (size.magnitude > 10) {
                selectrdFrame.rectTransform.anchoredPosition = min;
                selectrdFrame.rectTransform.sizeDelta = size;
                selectrdFrame.enabled = true;

                Rect rect = new Rect(min, size);

                Unit[] allUnits = FindObjectsOfType<Unit>();

                for (int i = 0; i < allUnits.Length; i++) {

                    Vector2 unitScreenPosition = camera.WorldToScreenPoint(allUnits[i].transform.position);

                    if (rect.Contains(unitScreenPosition)) {
                        if (unitInFrame.Contains(allUnits[i]) == false) {
                            unitInFrame.Add(allUnits[i]);
                            allUnits[i].OnHover();
                        }
                    } else {
                        unitInFrame.Remove(allUnits[i]);
                        allUnits[i].OnUnhover();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            selectrdFrame.enabled = false;
            for (int i = 0; i < unitInFrame.Count; i++) {
                Select(unitInFrame[i]);
            }
            unitInFrame.Clear();
        }
    }

    private void Select(SelectableObjects selectableObjects) {
        if (selectableObjects as Building) {
            UnSelectAll();
            selectedBuilding = true;
        } else if (selectedBuilding) {
            UnSelectAll();
            selectedBuilding = false;
        }

        if (listOfSelected.Contains(selectableObjects) == false) {
            listOfSelected.Add(selectableObjects);
            selectableObjects.Select();
        }
    }

    private void UnSelectAll() {
        for (int i = 0; i < listOfSelected.Count; i++) {
            listOfSelected[i].UnSelect();
        }
        listOfSelected.Clear();
    }

    private void UnhoverCurrent() {
        if (howerd) {
            howerd.OnUnhover();
            howerd = null;
        }
    }
}