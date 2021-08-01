using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjects : MonoBehaviour
{
    [Header("Selectable")]

    [SerializeField] private GameObject selectionIndicator; public GameObject SelectionIndicator => selectionIndicator;
    [SerializeField] private GameObject model;

    private bool alive = true; public bool Alive => alive;
    private Vector3 StartScale;


    protected virtual void Start() {
        StartScale = model.transform.localScale;
        UnSelect();
    }


    public virtual void OnHover() {
        model.transform.localScale = StartScale * 1.1f;
    }
    public virtual void OnUnhover() {
        model.transform.localScale = StartScale;
    }

    public virtual void Select() {
        if (selectionIndicator)
            selectionIndicator.SetActive(true);
    }
    public virtual void UnSelect() {
        if (selectionIndicator)
            selectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point) {
    }

    protected void DeathSelectable() {
        alive = false;
    }
    protected void CameToLifeSelectable() {
        alive = true;
    }

    private void OnDestroy() {
        Management management = FindObjectOfType<Management>();

        if (management.ListOfSelected.Contains(this))
            FindObjectOfType<Management>().UnSelect(this);
    }
}
