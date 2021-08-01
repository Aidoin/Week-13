using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnhancedMining : MonoBehaviour
{
    public Mine Mine;

    [SerializeField] private Button button;
    [SerializeField] private Text text;


    private void Update() {
        if (Mine.EnhancedMiningIsActive) {
            text.text = Mathf.CeilToInt(Mine.TimerEnhanced + Mine.TimerEnhancedCooldown).ToString();
            button.interactable = false;
        } else if (Mine.EnhancedIsCooldown) {
            text.text = Mathf.CeilToInt(Mine.TimerEnhancedCooldown).ToString();
            button.interactable = false;
        } else {
            text.text = "";
            button.interactable = true;
        }
    }


    public void PressingBottonEnhanced() {
        if (Mine.EnhancedMiningIsActive || Mine.EnhancedIsCooldown) {
            return;
        } else {
            Mine.EnhancedMiningEnabled();
        }
    }
}
