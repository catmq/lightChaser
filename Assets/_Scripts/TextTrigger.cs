using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour {

    public GameObject textToTrigger;
    public GameObject textToDeactivate;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
        }
        gameObject.SetActive(false);
    }
}
