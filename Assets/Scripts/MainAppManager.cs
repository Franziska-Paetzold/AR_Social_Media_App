﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAppManager : MonoBehaviour {

    public static MainAppManager mainAppManager;

    public MultiTargetARHandler ARHandler;
    public ScreenshotManager ScreenshotManager;
    public GameObject MainUIElements;
    public GameObject PostUIElements;
    public CloudUploading TargetUploader;

    private bool draw;
    private GameObject target = null;


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
    public void PushCreateButton(bool draw)
    {
        

        // Make a screenshot for the marker to upload
        ScreenshotManager.TakeAShot();



        ARHandler = FindObjectOfType<MultiTargetARHandler>();
        string targetName = ARHandler.BuildNewTarget();

        StartCoroutine(FindTarget(targetName));

        this.draw = draw;

        
        
    }

    IEnumerator FindTarget(string targetName)
    {

        
        target = GameObject.Find(targetName);

        while (target == null)
        {
            target = GameObject.Find(targetName);
            Debug.Log("searching " + targetName);
            yield return null;
        }

        ProcessPostRequest();
    }

    private void ProcessPostRequest()
    {
        Texture2D takenScreenshot = ScreenshotManager.GetScreenshotImage();

        MainUIElements.SetActive(false);
        PostUIElements.SetActive(true);

        if (draw)
        {
            target.GetComponentInChildren(typeof(Drawer), true).gameObject.SetActive(true);
        }
        else if (!draw)
            target.GetComponentInChildren(typeof(Texter), true).gameObject.SetActive(true);


        TargetUploader.texture = takenScreenshot;
        
    }

    public void PushPostButton()
    {
        TargetUploader.CallPostTarget();

        MainUIElements.SetActive(true);
        PostUIElements.SetActive(false);

        target.GetComponentInChildren<SwipeTrail>().enabled = false;
        target.GetComponentInChildren<ColorPicker>().gameObject.SetActive(false);
    }

}
