using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAppManager : MonoBehaviour {

    public static MainAppManager mainAppManager;

    public MultiTargetARHandler ARHandler;
    public ScreenshotManager ScreenshotManager;
    public GameObject MainUIElements;
    public GameObject PostUIElements;
    public GameObject CancelUI;
    public CloudUploading TargetUploader;


    private bool draw;
    private GameObject target = null;


    private void Awake () {
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


    /// <summary>
    /// Is called when the user triggers a button to create a post
    /// </summary>
    /// <param name="draw">True when user wants to draw; false for a text post</param>
    public void PushCreateButton(bool draw)
    {
        if (!MultiTargetEventHandler.ObjectDetected && !PostReconstructor.ObjectDetected)
        {
            string targetName = ARHandler.BuildNewTarget();
            if (targetName == null) return;

            //! Make a screenshot for the marker to upload
            ScreenshotManager.TakeAShot();

            StartCoroutine(FindTarget(targetName));

            this.draw = draw;
        }


    }

    /// <summary>
    /// Finds the GameObject of the new created target for further computing
    /// </summary>
    /// <param name="targetName">The name of the created object in the PushCreateButton method</param>
    /// <returns></returns>
    private IEnumerator FindTarget(string targetName)
    {

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
        Debug.Log("ProcessPostRequest()");
        MainUIElements.SetActive(false);
        PostUIElements.SetActive(true);

        if (draw)
        {
            target.GetComponentInChildren(typeof(Drawer), true).gameObject.SetActive(true);
        }
        else if (!draw)
        {
            GameObject textController = target.GetComponentInChildren(typeof(Texter), true).gameObject;
            textController.SetActive(true);
            //! bool textPostButton allows to open the keyboard
            textController.GetComponent<KeyBoardController>().textPostButton = true;

        }
        
    }

    public void PushPostButton()
    {
        
            Texture2D takenScreenshot = ScreenshotManager.GetScreenshotImage();
            TargetUploader.texture = takenScreenshot;


            MainUIElements.SetActive(true);
            PostUIElements.SetActive(false);

            if (draw)
            {
                //! Deactivate SwipeTrail to stop user drawing on screen
                target.GetComponentInChildren<SwipeTrail>().enabled = false;
                target.GetComponentInChildren<ColorPicker>().gameObject.SetActive(false);
                TargetUploader.metadataStr = target.GetComponentInChildren<SwipeTrail>().getJsonGraffiti();
            }
            else if (!draw)
            {
                TargetUploader.metadataStr = target.GetComponentInChildren<KeyBoardController>().getJsonGraffiti();
            }

            TargetUploader.CallPostTarget();
            ScreenshotManager.DeleteLastScreenshot();
            target = null;

    }


    public void PushCancelButton()
    {
        CancelUI.SetActive(true);
    }

    public void PushKeepButton()
    {
            CancelUI.SetActive(false);
    }

    public void PushDiscardButton()
    {
        CancelUI.SetActive(false);
        MainUIElements.SetActive(true);
        PostUIElements.SetActive(false);
        ARHandler.DestroyLastTrackable();
        Destroy(target);
        target = null;
        ScreenshotManager.DeleteLastScreenshot();
    }

}
