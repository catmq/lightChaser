using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour {

    public GameObject textToTrigger;
    public GameObject textToDeactivate;

    bool triggered;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerNextText()
    {
        if (!triggered)
        {
            if (textToTrigger != null)
            {
                textToTrigger.GetComponent<TextControl>().TextFadeIn();
            }
            if (textToDeactivate != null)
            {
                textToDeactivate.GetComponent<TextControl>().TextFadeOut();
                //textToDeactivate.SetActive(false);
            }
            triggered = true;
        }
        //gameObject.SetActive(false);
    }

   
}
