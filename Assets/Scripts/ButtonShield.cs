using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShield : MonoBehaviour
{
    public Barack Barack;

    [SerializeField] private Button button;
    [SerializeField] private Text text;


    private void Update() {
        if (Barack.ShieldIsCooldown) {
            text.text = Mathf.CeilToInt(Barack.TimerShieldCooldown).ToString();
            button.interactable = false;
        } else if (Barack.ShieldIsActive) {
            text.text = Mathf.CeilToInt(Barack.TimerActiveShield).ToString();
            button.interactable = true;
        } else {
            text.text = "";
            button.interactable = true;
        }
    }


    public void PressingBottonShield() {
        if (Barack.ShieldIsCooldown) {
            return;
        } else if (Barack.ShieldIsActive) {
            Barack.ShieldDisabled();
        } else {
            Barack.ShieldEnabled();
        }
    }
}
