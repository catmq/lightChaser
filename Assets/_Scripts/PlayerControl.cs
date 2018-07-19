using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomFPC))]
public class PlayerControl : MonoBehaviour {

    CustomFPC fpc;

    bool inTransition = false;
    float currentSpeed;
    float targetSpeed;
    float acceleration;
    // Use this for initialization

    public FadeControl fadingSurface;
    public float fadeTime;
    public float walkSpeed;
    public Vector3 resetPosition;

    void Start () {
        fpc = GetComponent<CustomFPC>();

        if (!fadingSurface.gameObject.activeInHierarchy)
        {
            fadingSurface.gameObject.SetActive(true);
        }
        StartNewLevel();
    }
	
	// Update is called once per frame
	void Update () {
		if (inTransition)
        {
            if (acceleration < 0 && currentSpeed > targetSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed <= targetSpeed)
                {
                    currentSpeed = targetSpeed;
                    inTransition = false;
                }
                fpc.SetWalkSpeed(currentSpeed);
                return;
            }
            if (acceleration > 0 && currentSpeed < targetSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed >= targetSpeed)
                {
                    currentSpeed = targetSpeed;
                    inTransition = false;
                }
                fpc.SetWalkSpeed(currentSpeed);
                return;
            }
            
        }
	}

    public void WalkSpeedChange (float time,float target)
    {
        inTransition = true;
        currentSpeed = fpc.GetWalkSpeed();
        targetSpeed = target;
        acceleration = (targetSpeed - currentSpeed) / time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LevelTrigger")
        {
            if (!fadingSurface.gameObject.activeInHierarchy)
            {
                fadingSurface.gameObject.SetActive(true);
            }
            fadingSurface.StartFadingOut(fadeTime);
            WalkSpeedChange(fadeTime, 0);
            StartCoroutine(WaitAndStartNewLevel(fadeTime));

        }
        if (other.gameObject.tag == "WaypointLight")
        {
            other.gameObject.GetComponent<LightTrigger>().TriggerNextLight();
        }
    }

    public void StartNewLevel()
    {
        transform.position = resetPosition;
        fadingSurface.StartFadingIn(fadeTime);
        WalkSpeedChange(fadeTime, walkSpeed);
    }

    IEnumerator WaitAndStartNewLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StageManager.instance.GoToNextStage();
        StartNewLevel();
    }
}
