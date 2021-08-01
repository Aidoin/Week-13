using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct AdTextMassage
{
    public string Message;
    public float AppearanceTime;
    public float DisplayTime;
    public float AttenuationTime;

    public AdTextMassage(string Message, float AppearanceTime, float DisplayTime, float AttenuationTime) {
        this.Message = Message;
        this.AppearanceTime = AppearanceTime;
        this.DisplayTime = DisplayTime;
        this.AttenuationTime = AttenuationTime;
    }
}

public class AdText : MonoBehaviour
{
    [SerializeField] private Text text;
    [TextArea(1, 5)] [SerializeField] private string startMessage;
    [SerializeField] private float defaultAppearanceTime = 1f;
    [SerializeField] private float defaultDisplayTime = 5f;
    [SerializeField] private float defaultAttenuationTime = 1f;

    private List<AdTextMassage> displayQueue = new List<AdTextMassage>();
    private bool currentMessageIslyDisplayed = false;


    private void Start() {
        Show(startMessage);
    }

    private void Update() {
        if (displayQueue.Count > 0 && currentMessageIslyDisplayed == false) {
            currentMessageIslyDisplayed = true;
            StartCoroutine(Appearance(displayQueue[0]));
            displayQueue.RemoveAt(0);
        }
    }


    public void Show(string massage) {
        displayQueue.Add(new AdTextMassage(massage, defaultAppearanceTime, defaultDisplayTime, defaultAttenuationTime));
    }

    public void Show(string massage, float displayTime) {
        displayQueue.Add(new AdTextMassage(massage, defaultAppearanceTime, displayTime, defaultAttenuationTime));
    }

    public void Show(string massage, float displayTime, float appearanceTime) {
        displayQueue.Add(new AdTextMassage(massage, appearanceTime, displayTime, defaultAttenuationTime));
    }

    public void Show(string massage, float displayTime, float appearanceTime, float attenuationTime) {
        displayQueue.Add(new AdTextMassage(massage, appearanceTime, displayTime, attenuationTime));
    }


    IEnumerator Appearance(AdTextMassage adTextMassage) {
        Color newColor = text.color;
        if (adTextMassage.Message == null) {
            text.text = "Error!  Сообщение не найдено\nMessage == null";
        } else {
            text.text = adTextMassage.Message;
        }

        while (text.color.a != 1) {
            newColor.a += Time.deltaTime / adTextMassage.AppearanceTime;

            if (newColor.a > 1) {
                newColor.a = 1;
            }
            text.color = newColor;
            yield return null;
        }
        yield return new WaitForSeconds(adTextMassage.DisplayTime);
        StartCoroutine(Attenuation(adTextMassage.AttenuationTime));
    }

    IEnumerator Attenuation(float attenuationTime) {
        Color newColor = text.color;

        while (text.color.a != 0) {
            newColor.a -= Time.deltaTime / attenuationTime;

            if (newColor.a < 0) {
                newColor.a = 0;
            }
            text.color = newColor;
            yield return null;
        }
        currentMessageIslyDisplayed = false;
    }
}
