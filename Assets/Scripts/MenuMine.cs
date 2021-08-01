using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMine : MenuBuilding
{
    [SerializeField] private ButtonEnhancedMining buttonEnhancedMining;


    public void SetBuildingInMenu(Mine mine) {
        buttonEnhancedMining.Mine = buttonEnhancedMining.Mine = mine;
    }
}
