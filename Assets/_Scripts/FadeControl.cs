using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControl : MonoBehaviour {

    Material fadingMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void StartFadingIn(float time)
    {
        if (fadingMaterial == null)
        {
            fadingMaterial = GetComponent<Renderer>().material;
        }
        StartCoroutine(Transitions.Fade(fadingMaterial, time, false));
    }

    public void StartFadingOut(float time)
    {
        if (fadingMaterial == null)
        {
            fadingMaterial = GetComponent<Renderer>().material;
        }
        StartCoroutine(Transitions.Fade(fadingMaterial, time, true));
    }

}
