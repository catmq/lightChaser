using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour {

    public Text textComponent;

    //public float minAlpha = 0f;
    public float fadeSpeed = 0.2f;
    //public float recoverSpeed = 0.8f;
    public float textAppearDistance = 4f;

    public CustomFPC playerController;

    float currentTextAlpha;

    //Color textColor;
	// Use this for initialization
	void Start () {
        //textColor = textComponent.color;
    }

    // Update is called once per frame
    void Update() {
        //if (textComponent.gameObject.activeInHierarchy)
        //{
        //    if (playerController.IsWalking())
        //    {
        //        currentTextAlpha -= fadeSpeed * Time.deltaTime;
        //        if (currentTextAlpha < minAlpha)
        //        {
        //            currentTextAlpha = minAlpha;

        //        }
        //        textColor.a = currentTextAlpha;
        //        textComponent.color = textColor;
        //        return;
        //    }
        //    currentTextAlpha += recoverSpeed * Time.deltaTime;
        //    if (currentTextAlpha > 1)
        //    {
        //        currentTextAlpha = 1;

        //    }
        //    textColor.a = currentTextAlpha;
        //    textComponent.color = textColor;
        //    return;
        //}
    }
    public void TextFadeIn()
    {
        if (textComponent.isActiveAndEnabled) return;
        Vector3 playerPos = playerController.transform.position;
        playerPos.y = 0;
        Vector3 textPlacementDirection = new Vector3(playerController.transform.forward.x, 0, playerController.transform.forward.z);
        Debug.Log(textPlacementDirection);
        Vector3 textPos = playerPos + textPlacementDirection.normalized * textAppearDistance;
        transform.position = textPos;
        transform.LookAt(playerPos);
        transform.Rotate(Vector3.up, 180f);
        Color currentColor = textComponent.color;
        currentColor.a = 0;
        textComponent.color = currentColor;
        textComponent.gameObject.SetActive(true);
        StartCoroutine(Transitions.Fade(textComponent, 1/fadeSpeed, true));
    }

    public void TextFadeOut()
    {
        if (!textComponent.isActiveAndEnabled) return;
        StartCoroutine(Transitions.Fade(textComponent, 1 / fadeSpeed, false));
        StartCoroutine(Transitions.WaitAndDisableObject(gameObject, 1 / fadeSpeed));
    }


}
