using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ManagementState {
    Default,
    BuildingPiacing,
    FrameSelection
}


public class Management : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private SelectableObjects howerd;
    private List<SelectableObjects> listOfSelected = new List<SelectableObjects>(); public List<SelectableObjects> ListOfSelected => listOfSelected;

    [SerializeField] private Image selectrdFrame;
    private Vector2 frameStart;
    private Vector2 frameEnd;
    private List<Unit> unitInFrame = new List<Unit>();
    private Vector3 startClick = Vector3.zero;
    private bool selectedBuilding;
    private ManagementState currentState;


    private void Update() {
        Selection();
        UnitsSelectionFrame();
        UnitMove();
    }


    private void Selection() {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            if (currentState == ManagementState.Default) {
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
            }
        } else {
            UnhoverCurrent();
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Input.GetKey(KeyCode.LeftControl) == false) {
                UnSelectAll();
            }
            if (howerd && currentState == ManagementState.Default) {
                if (listOfSelected.Contains(howerd)) {
                    UnSelect(howerd);
                } else {
                    Select(howerd);
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

            Vector2 pointMin = Vector2.Min(frameStart, frameEnd);
            Vector2 PointMax = Vector2.Max(frameStart, frameEnd);
            Vector2 frameSize = PointMax - pointMin;

            if (currentState == ManagementState.Default && frameSize.magnitude > 10 && !EventSystem.current.IsPointerOverGameObject()) {
                SetState(ManagementState.FrameSelection);
                if (Input.GetKey(KeyCode.LeftControl) == false) {
                    UnSelectAll();
                }
            }

            if (currentState == ManagementState.FrameSelection) {
                selectrdFrame.rectTransform.anchoredPosition = pointMin;
                selectrdFrame.rectTransform.sizeDelta = frameSize;
                selectrdFrame.enabled = true;

                Rect rect = new Rect(pointMin, frameSize);

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

        if (currentState == ManagementState.FrameSelection && Input.GetMouseButtonUp(0)) {
            selectrdFrame.enabled = false;
            for (int i = 0; i < unitInFrame.Count; i++) {
                if (unitInFrame[i]) {
                    Select(unitInFrame[i]);
                }
            }
            unitInFrame.Clear();
            SetState(ManagementState.Default);
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

    public void UnSelect(SelectableObjects selectableObjects) {
        if (listOfSelected.Contains(selectableObjects)) {
            listOfSelected.Remove(selectableObjects);
            selectableObjects.UnSelect();
        }
    }

    private void UnhoverCurrent() {
        if (howerd) {
            howerd.OnUnhover();
            howerd = null;
        }
    }

    public void SetState(ManagementState state) {
        currentState = state;

        if (state == ManagementState.BuildingPiacing) {
            UnhoverCurrent();
            UnSelectAll();
            selectrdFrame.enabled = false;
        }
    }

    private void UnitMove() {
        if (listOfSelected.Count == 0) return;

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (Input.GetMouseButtonDown(1)) {
            startClick = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1)) {
            if (startClick == Input.mousePosition) {
                if (hit.collider.tag == "Ground") {
                    if (listOfSelected.Count > 1) {
                        int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(listOfSelected.Count));

                        for (int i = 0; i < listOfSelected.Count; i++) {

                            int row = i / rowNumber;
                            int column = i % rowNumber;
                            Vector3 point = hit.point + new Vector3((float)row - (rowNumber / 4f), 0f, (float)column - (rowNumber / 4f)) * 2f;

                            listOfSelected[i].WhenClickOnGround(point);
                        }
                    } else {
                        listOfSelected[0].WhenClickOnGround(hit.point);
                    }
                }
            }
        }
    }
}