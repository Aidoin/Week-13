using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBarack : MonoBehaviour
{
    [SerializeField] private Text priceText;
    [SerializeField] private GameObject barackPrefab;


    private void Start() {
        priceText.text = barackPrefab.GetComponent<Building>().Price.ToString();
    }
}
