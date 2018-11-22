using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transitions : MonoBehaviour {

    public static IEnumerator Fade(Image image, float time, bool fadeIn)
    {
        float currentTime = 0;
        Color currentColor;
        currentColor = image.color;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            if (fadeIn)
            {
                currentColor.a = timeRatio;
            }
            else
            {
                currentColor.a = 1 - timeRatio;
            }
            image.color = currentColor;
            yield return null;
        }
        currentColor = image.color;
        if (fadeIn)
        {
            currentColor.a = 1;
        }
        else
        {
            currentColor.a = 0;
        }
        image.color = currentColor;
    }

    public static IEnumerator Fade(Text text, float time, bool fadeIn)
    {
        float currentTime = 0;
        Color currentColor;
        currentColor = text.color;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            if (fadeIn)
            {
                currentColor.a = timeRatio;
            }
            else
            {
                currentColor.a = 1 - timeRatio;
            }
            text.color = currentColor;
            yield return null;
        }
        if (fadeIn)
        {
            currentColor.a = 1;
        }
        else
        {
            currentColor.a = 0;
        }
        text.color = currentColor;
    }

    public static IEnumerator Fade(Material mat, float time, bool fadeIn)
    {
        float currentTime = 0;
        Color currentColor;
        currentColor = mat.color;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            if (fadeIn)
            {
                currentColor.a = timeRatio;
            }
            else
            {
                currentColor.a = 1 - timeRatio;
            }
            mat.color = currentColor;
            yield return null;
        }
        if (fadeIn)
        {
            currentColor.a = 1;
        }
        else
        {
            currentColor.a = 0;
        }
        mat.color = currentColor;
    }

    public static IEnumerator Fade(ParticleSystem particleSys, float time, bool fadeIn)
    {
        ParticleSystem.MainModule particleModule = particleSys.main;
        float currentTime = 0;
        Color currentColor;
        currentColor = particleModule.startColor.color;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            if (fadeIn)
            {
                currentColor.a = timeRatio;
            }
            else
            {
                currentColor.a = 1 - timeRatio;
            }
            particleModule.startColor = currentColor;
            yield return null;
        }
        if (fadeIn)
        {
            currentColor.a = 1;
        }
        else
        {
            currentColor.a = 0;
        }
        particleModule.startColor = currentColor;
    }

    public static IEnumerator Fade(Light light, float startIntensity, float endIntensity, float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            light.intensity = timeRatio * (endIntensity - startIntensity) + startIntensity;
            yield return null;
        }
        light.intensity = endIntensity;
    }

    public static IEnumerator Fade(LightControl lightControl, float startValue, float endValue, float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            lightControl.transitionRatio = timeRatio * (endValue - startValue) + startValue;
            yield return null;
        }
        lightControl.transitionRatio = endValue;
    }

    public static IEnumerator Move(Transform transform, float time, Vector3 startPos, Vector3 targetPos,bool worldPos)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            if (worldPos)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, timeRatio);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPos, targetPos, timeRatio);
            }
            yield return null;
        }
        if (worldPos)
        {
            transform.position = targetPos;
        }
        else
        {
            transform.localPosition = targetPos;
        }

    }

    public static IEnumerator Scale(Transform transform, float time, Vector3 startScale, Vector3 targetScale)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float timeRatio = currentTime / time;
            if (timeRatio > 1) timeRatio = 1;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timeRatio);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    public static IEnumerator WaitAndDisableObject(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(false);
    }
}
