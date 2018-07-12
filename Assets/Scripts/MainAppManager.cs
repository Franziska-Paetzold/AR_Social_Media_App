using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAppManager : MonoBehaviour {

    public static MainAppManager mainAppManager;

    public MultiTargetARHandler ARHandler;
    public ScreenshotManager screenshotManager;


    void Awake () {
		if(mainAppManager == null)
        {
            mainAppManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	

	void Update () {
		
	}

    /// <summary>
    /// Is called when the user triggers a button to create a post
    /// </summary>
    /// <param name="draw">True when user wants to draw; false for a text post</param>
    public void PushPostButton(bool draw)
    {
        ARHandler = FindObjectOfType<MultiTargetARHandler>();
        string targetName = ARHandler.BuildNewTarget();
        GameObject target = GameObject.Find(targetName);
        Debug.Log(target.name + "gasd");
        if (draw)
            target.GetComponentInChildren<SwipeTrail>().gameObject.SetActive(true);

        screenshotManager.TakeAShot();
    }

}
