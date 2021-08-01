using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _scaleHealth;

    private Transform _target;
    private Transform _cameraTransform;


    void Start() {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate() {
        transform.position = _target.position;
        transform.rotation = _cameraTransform.rotation;
    }


    public void Setup(Transform parent) {
        _target = parent;
    }

    public void SetHealth(int health, int maxHealth) {
        float xScale = (float)health / maxHealth;
        xScale = xScale < 0 ? 0 : xScale;
        _scaleHealth.localScale = new Vector3(xScale, 1f, 1f);
    }
}
