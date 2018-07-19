using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour {

    public Text textComponent;

    public float minAlpha = 0f;
    public float fadeSpeed = 0.2f;
    public float recoverSpeed = 0.8f;

    public CustomFPC playerController;

    float currentTextAlpha;

    Color textColor;
	// Use this for initialization
	void Start () {
        textColor = textComponent.color;

    }

    // Update is called once per frame
    void Update() {
        if (textComponent.gameObject.activeInHierarchy)
        {
            if (playerController.IsWalking())
            {
                currentTextAlpha -= fadeSpeed * Time.deltaTime;
                if (currentTextAlpha < 0)
                {
                    currentTextAlpha = 0;

                }
                textColor.a = currentTextAlpha;
                textComponent.color = textColor;
                return;
            }
            currentTextAlpha += recoverSpeed * Time.deltaTime;
            if (currentTextAlpha > 1)
            {
                currentTextAlpha = 1;

            }
            textColor.a = currentTextAlpha;
            textComponent.color = textColor;
            return;
        }
    }
}
