using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThicknessTheSelectingFrame : MonoBehaviour
{
    [SerializeField] private Image Image;


    void FixedUpdate() {
        Image.pixelsPerUnitMultiplier = (270f / Screen.height) * 10;
    }
}
