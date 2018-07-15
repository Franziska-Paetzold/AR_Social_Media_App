using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAppManager : MonoBehaviour {

    public static MainAppManager mainAppManager;

    public MultiTargetARHandler ARHandler;
    public ScreenshotManager screenshotManager;
    public GameObject UIElements;

    private bool draw;


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
        UIElements.SetActive(false);
        ARHandler = FindObjectOfType<MultiTargetARHandler>();
        string targetName = ARHandler.BuildNewTarget();

        StartCoroutine(FindTarget(targetName));

        this.draw = draw;

        //screenshotManager.TakeAShot();
        //Texture2D takenScreenshot = screenshotManager.GetScreenshotImage();
    }

    IEnumerator FindTarget(string targetName)
    {

        GameObject target = null;
        target = GameObject.Find(targetName);

        while (target == null)
        {
            target = GameObject.Find(targetName);
            Debug.Log("searching " + targetName);
            yield return null;
        }

        ProcessPostRequest(target);
    }

    private void ProcessPostRequest(GameObject target)
    {
        if (draw)
        {
            target.GetComponentInChildren(typeof(Drawer), true).gameObject.SetActive(true);
            TrailRenderer traili = target.GetComponentInChildren(typeof(TrailRenderer)) as TrailRenderer;
            traili.enabled = false;
            Debug.Log(traili, traili.gameObject);
        }
        else if (!draw)
            target.GetComponentInChildren(typeof(Texter), true).gameObject.SetActive(true);
    }

}
