using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    public static StageManager instance = null;
    public GameObject[] stages;
    int currentLevel;
    // Use this for initialization

    public GameObject finalStageFollowObjects;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToNextStage()
    {
        stages[currentLevel].SetActive(false);
        currentLevel++;
        stages[currentLevel].SetActive(true);
        if (currentLevel == stages.Length -1)
        {
            finalStageFollowObjects.SetActive(true);
        }
    }
}
