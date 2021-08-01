using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : Building
{
    [SerializeField] private AdText adText;
    [SerializeField] private Management management;
    [SerializeField] private ResourceManager resourceManager;


    protected override void Awake() {
        base.Awake();
        menuBuilding = FindObjectOfType<MenuBarack>();
    }

    private void Update() {
        if (Alive == false) {
            adText.Show("Поражение!\nГлавное здание уничтожено");
            management.enabled = false;
            resourceManager.Block();
        }
    }
}
