using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {

    //bool inTransition;

    public Light lightSource;
    public GameObject lightObject;
    public ParticleSystem lightDust;
    public ParticleSystem lightFadeOut;

    public float minAlpha = 0.1f;
    public float fadeSpeed = 0.2f;
    public float recoverSpeed = 0.8f;

    public float lightMaxRange = 50f;
    public float lightMinIntensity = 1f;
    public float lightMaxIntensity = 5f;
    public float lightMaxDistance = 50f;

    public bool fadeByDistance = false;
    public float fadeMaxDistance = 3f;
    public float fadeMinDistance = 0.5f;

    Material lightMaterial;

    public CustomFPC playerController;

    Color currentLightSurfaceColor;
    float currentAlpha;
    float alphaRatio;
    [HideInInspector]
    public float transitionRatio;
    float currentLightIntensity;

    ParticleSystem.MainModule lightDustModule;

    bool initialized;

    // Use this for initialization
    void Start() {
        if (!initialized)
            InitializeField();
    }

    // Update is called once per frame
    void Update() {
        float playerDistance = Vector3.Distance(playerController.transform.position, transform.position);

        if (lightSource != null) CalculateLightIntensity(playerDistance);

            if (fadeByDistance)
            {
                alphaRatio = (playerDistance - fadeMinDistance) / (fadeMaxDistance - fadeMinDistance);
                //currentAlpha = fadeRatio * (1-minAlpha) + minAlpha;
            }
            else
            {
                if (playerController.IsWalking())
                {
                    alphaRatio -= fadeSpeed * Time.deltaTime;
                    //currentAlpha -= fadeSpeed * Time.deltaTime;
                    //if (fadeRatio <minAlpha)
                    //{
                    //    fadeRatio = minAlpha;
                    //}
                    //if (currentAlpha <minAlpha)
                    //{
                    //    currentAlpha = minAlpha;
                    //}
                    //lightSource.intensity = currentLightIntensity * currentAlpha * transitionRatio;
                    //lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha * transitionRatio);
                    //lightDustModule.startColor = currentLightSurfaceColor * currentAlpha * transitionRatio;
                    //return;
                }
                else
                {
                    alphaRatio += recoverSpeed * Time.deltaTime;
                }
                //currentAlpha += recoverSpeed * Time.deltaTime;
            }
            if (alphaRatio > 1)
            {
                alphaRatio = 1;
            }
            if (alphaRatio < 0)
            {
                alphaRatio = 0;
            }
            currentAlpha = alphaRatio * (1 - minAlpha) + minAlpha;
        //if (currentAlpha > 1)
        //{
        //    currentAlpha = 1;
        //}
        //lightSource.intensity = currentLightIntensity;
        //lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha);
        //lightDustModule.startColor = currentLightSurfaceColor * currentAlpha;
        if (lightSource != null) lightSource.intensity = currentLightIntensity * currentAlpha * transitionRatio;
        if (lightObject !=null) lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha * transitionRatio);
        if (lightDust !=null) lightDustModule.startColor = currentLightSurfaceColor * currentAlpha * transitionRatio;
        return;
        //if (transitionState == TransitionState.FadeIn)
        //{
        //    currentLightIntensity += fadeSpeed * Time.deltaTime;
        //    currentAlpha += fadeSpeed * Time.deltaTime;
        //    if (currentLightIntensity > minAlpha)
        //    {
        //        currentLightIntensity = minAlpha;
        //    }
        //    if (currentAlpha > minAlpha)
        //    {
        //        currentAlpha = minAlpha;
        //        transitionState = TransitionState.None;
        //    }
        //    lightSource.intensity = currentLightIntensity;
        //    lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha);
        //    lightDustModule.startColor = currentLightSurfaceColor * currentAlpha;
        //    return;
        //}
        //if (transitionState == TransitionState.FadeOut)
        //{
        //    currentLightIntensity -= fadeSpeed * Time.deltaTime;
        //    currentAlpha -= fadeSpeed * Time.deltaTime;
        //    if (currentLightIntensity < 0)
        //    {
        //        currentLightIntensity = 0;
        //    }
        //    if (currentAlpha < 0)
        //    {
        //        currentAlpha = 0;
        //        transitionState = TransitionState.None;
        //        gameObject.SetActive(false);
        //    }
        //    lightSource.intensity = currentLightIntensity;
        //    lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha);
        //    lightDustModule.startColor = currentLightSurfaceColor * currentAlpha;
        //    return;
        //}
    }

    void CalculateLightIntensity(float playerDistance)
    {
        float lightRatio = playerDistance / lightMaxDistance;
        if (lightRatio > 1)
        {
            lightRatio = 1;
        }
        lightSource.range = lightRatio * lightMaxRange;
        currentLightIntensity = 1f/((1f / lightMinIntensity - 1f / lightMaxIntensity) * lightRatio + 1f /lightMaxIntensity);
    }

    public void Disappear()
    {
        Debug.Log("Disappear");
        //transitionState = TransitionState.FadeOut;
        float fadeTime = 1 / fadeSpeed;
        //StartCoroutine(Transitions.Fade(lightSource, fadeTime, false));
        //StartCoroutine(Transitions.Fade(lightMaterial, fadeTime, false));
        //StartCoroutine(Transitions.Fade(lightDust, fadeTime, false));
        StartCoroutine(Transitions.WaitAndDisableObject(gameObject, fadeTime));
        ParticleSystem.MainModule lightFadeModule = lightFadeOut.main;
        lightFadeModule.startColor = lightMaterial.color;
        lightObject.gameObject.SetActive(false);
        lightFadeOut.gameObject.SetActive(true);
        lightFadeOut.Play();
        //inTransition = true;
    }

    public void Appear()
    {
        Debug.Log("Appear");
        if (!initialized) InitializeField();
        //float playerDistance = Vector3.Distance(playerController.transform.position, transform.position);
        //CalculateLightIntensity(playerDistance);
        //float lightIntensity = lightSource.intensity;
        //Debug.Log(lightIntensity);
        //lightSource.intensity = 0;
        //Color lightSurfaceColor = currentLightSurfaceColor;
        //lightSurfaceColor.a = 0;
        //lightMaterial.SetColor("_Color", lightSurfaceColor);
        //lightDustModule.startColor = lightSurfaceColor;

        if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

        float fadeTime = 1 / fadeSpeed;
        //StartCoroutine(Transitions.Fade(lightSource, 0, lightIntensity, fadeTime));
        //StartCoroutine(Transitions.Fade(lightMaterial, fadeTime, true));
        //StartCoroutine(Transitions.Fade(lightDust, fadeTime, true));
        StartCoroutine(Transitions.Fade(this, 0, 1, fadeTime));
        //inTransition = true;
        //StartCoroutine(DisableTransition(fadeTime));
        //transitionState = TransitionState.FadeIn;
    }

    void InitializeField()
    {
        if (lightObject != null)
        {
            lightMaterial = lightObject.GetComponent<Renderer>().material;
            currentLightSurfaceColor = lightMaterial.GetColor("_Color");
            //currentAlpha = currentLightSurfaceColor.a;
        }
        //fadeRatio = lightSource.intensity;
        if (lightDust != null)
        {
            lightDustModule = lightDust.main;
        }
        alphaRatio = 1;
        transitionRatio = 1;
        initialized = true;
    }

    //IEnumerator DisableTransition(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    inTransition = false;
    //}


}
