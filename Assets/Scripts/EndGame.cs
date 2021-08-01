using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private AdText adText;
    [SerializeField] private Image blackScreen;
    [TextArea(1, 5)] [SerializeField] private string endText;
    [SerializeField] private AudioSource audioBackground;
    [SerializeField] private AudioClip endMusic;

    private bool isEnd = false;


    private void FixedUpdate() {
        if (isEnd == false && resourceManager.Resources.Golds > 2500) {
            StartCoroutine(End());
        }
    }


    private IEnumerator End() {
        adText.Show(endText, 3f);
        adText.Show("Победа!", 50f, 1f, 50);

        audioBackground.clip = endMusic;
        audioBackground.volume = 0.1f;
        audioBackground.Play();

        resourceManager.Buy(new Resources(2500));
        Color color = blackScreen.color;

        while (color.a < 1) {
            color.a += Time.deltaTime / 5;

            if (color.a > 1) color.a = 1;

            blackScreen.color = color;

            yield return null;
        }
    }
}
