using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjects : MonoBehaviour {

    [SerializeField] private GameObject model;
    [SerializeField] private GameObject selectionIndicator;
    public GameObject SelectionIndicator => selectionIndicator;

    private Vector3 StartScale;


    public virtual void Start() {
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
        selectionIndicator.SetActive(true);
    }

    public virtual void UnSelect() {
        selectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point) {
    }
}
