using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barack : Building
{
    [SerializeField] private GameObject button;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private MonoBehaviour[] scriptsButton;


    public override void Select() {
        base.Select();
        GameObject currentButton = Instantiate(button, menuBuilding.transform);
        currentButton.GetComponent<Image>().sprite = sprites[0];
        //currentButton.AddComponent <scriptsButton[0]>(); так нельзя, тогда каким способом можно через код повесить свой скрипт на объект?
    }

    public override void UnSelect() {
        Debug.Log(menuBuilding.transform.childCount);
        for (int i = 0; i < menuBuilding.transform.childCount; i++) {
            Destroy(menuBuilding.transform.GetChild(i));
        }
        base.UnSelect();
    }
}
