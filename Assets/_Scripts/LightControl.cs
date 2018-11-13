using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {

    bool inTransition;

    public Light lightSource;
    public GameObject lightObject;
    public ParticleSystem lightDust;

    public float minAlpha = 0.1f;
    public float fadeSpeed = 0.2f;
    public float recoverSpeed = 0.8f;

    public float lightMaxRange = 50f;
    public float lightMaxDistance = 50f;

    public bool fadeByDistance = false;
    public float fadeMaxDistance = 3f;
    public float fadeMinDistance = 0.8f;

    Material lightMaterial;

    public CustomFPC playerController;

    Color currentLightSurfaceColor;
    float currentAlpha;
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
        if (!inTransition)
        {
            float playerDistance = Vector3.Distance(playerController.transform.position, transform.position);
            float lightRangeRatio = playerDistance / lightMaxDistance;
            if (lightRangeRatio > 1)
            {
                lightRangeRatio = 1;
            }
            lightSource.range = lightRangeRatio * lightMaxRange;

            if (fadeByDistance)
            {
                currentLightIntensity = (playerDistance - fadeMinDistance) / (fadeMaxDistance - fadeMinDistance);
                currentAlpha = currentLightIntensity;
            }
            else
            {
                if (playerController.IsWalking())
                {
                    currentLightIntensity -= fadeSpeed * Time.deltaTime;
                    currentAlpha -= fadeSpeed * Time.deltaTime;
                    if (currentLightIntensity <minAlpha)
                    {
                        currentLightIntensity = minAlpha;
                    }
                    if (currentAlpha <minAlpha)
                    {
                        currentAlpha = minAlpha;
                    }
                    lightSource.intensity = currentLightIntensity;
                    lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha);
                    lightDustModule.startColor = currentLightSurfaceColor * currentAlpha;
                    return;
                }
                currentLightIntensity += recoverSpeed * Time.deltaTime;
                currentAlpha += recoverSpeed * Time.deltaTime;
            }
            if (currentLightIntensity > 1)
            {
                currentLightIntensity = 1;
            }
            if (currentAlpha > 1)
            {
                currentAlpha = 1;
            }
            lightSource.intensity = currentLightIntensity;
            lightMaterial.SetColor("_Color", currentLightSurfaceColor * currentAlpha);
            lightDustModule.startColor = currentLightSurfaceColor * currentAlpha;
            return;


        }
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

    public void Disappear()
    {
        Debug.Log("Disappear");
        //transitionState = TransitionState.FadeOut;
        float fadeTime = 1 / fadeSpeed;
        StartCoroutine(Transitions.Fade(lightSource, fadeTime, false));
        StartCoroutine(Transitions.Fade(lightMaterial, fadeTime, false));
        StartCoroutine(Transitions.Fade(lightDust, fadeTime, false));
        StartCoroutine(Transitions.WaitAndDisableObject(gameObject, fadeTime));
        inTransition = true;
    }

    public void Appear()
    {
        Debug.Log("Appear");
        if (!initialized) InitializeField();
        lightSource.intensity = 0;
        Color lightSurfaceColor = currentLightSurfaceColor;
        lightSurfaceColor.a = 0;
        lightMaterial.SetColor("_Color", lightSurfaceColor);
        lightDustModule.startColor = lightSurfaceColor;

        if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

        float fadeTime = 1 / fadeSpeed;
        StartCoroutine(Transitions.Fade(lightSource, fadeTime, true));
        StartCoroutine(Transitions.Fade(lightMaterial, fadeTime, true));
        StartCoroutine(Transitions.Fade(lightDust, fadeTime, true));
        inTransition = true;
        StartCoroutine(DisableTransition(fadeTime));
        //transitionState = TransitionState.FadeIn;
    }

    void InitializeField()
    {
        lightMaterial = lightObject.GetComponent<Renderer>().material;
        currentLightSurfaceColor = lightMaterial.GetColor("_Color");
        currentLightIntensity = lightSource.intensity;
        lightDustModule = lightDust.main;
        currentAlpha = currentLightSurfaceColor.a;
        initialized = true;
    }

    IEnumerator DisableTransition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        inTransition = false;
    }
}
