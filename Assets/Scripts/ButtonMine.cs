using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMine : MonoBehaviour
{
    [SerializeField] private Text priceText;
    [SerializeField] private GameObject mine;


    private void Start() {
        priceText.text = mine.GetComponent<Building>().Price.ToString();
    }
}
