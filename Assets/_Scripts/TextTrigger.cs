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
            textToTrigger.SetActive(true);
            if (textToDeactivate != null)
            {
                textToDeactivate.SetActive(false);
            }
        }
    }
}
