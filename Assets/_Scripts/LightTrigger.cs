using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public LightControl nextLight;
    public bool disappearAfterTrigger = false;

    bool triggered;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void TriggerNextLight()
    {
        if (!triggered)
        {
            if (nextLight != null)
                nextLight.Appear();
            LightControl lightControl = GetComponent<LightControl>();
            if (disappearAfterTrigger && lightControl != null)
                lightControl.Disappear();
            triggered = true;
        }

    }

}
