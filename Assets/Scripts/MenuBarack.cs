using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarack : MenuBuilding
{
    [SerializeField] private CreateSwordman createSwordman;
    [SerializeField] private ButtonShield buttonShield;


    public void SetBuildingInMenu(Barack barack) {
        createSwordman.Barack = buttonShield.Barack = barack;
    }
}
