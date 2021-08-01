using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSwordman : MonoBehaviour
{
    public Barack Barack;

    [SerializeField] private GameObject swordmanPrefab;
    [SerializeField] private Text price;


    private void Start() {
        price.text = swordmanPrefab.GetComponent<Swordsman>().Price.Golds.ToString();
    }


    public void BuySwordman() {
        Barack.BuyUnit(swordmanPrefab);
    }
}
