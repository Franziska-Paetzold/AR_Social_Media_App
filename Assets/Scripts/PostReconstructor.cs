using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PostReconstructor : MonoBehaviour, ITrackableEventHandler {
    public LineManager lineManager = new LineManager();

    private TrackableBehaviour mTrackableBehaviour;
    public SimpleCloudHandler CloudHandler;
    public Material TrailMaterial;
    public GameObject PostParent;

    // Use this for initialization
    void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void ShowDownloadedPost()
    {
        for (int i = 0; i < lineManager.AllLines.Count; i++)
        {
           CreateLineObject(i);
        }
    }


    void CreateLineObject(int lineCounter)
    {
        GameObject newLine = new GameObject();
        newLine.transform.parent = PostParent.transform;
        newLine.name = "Line segment " + lineCounter;
        newLine.AddComponent<LineRenderer>();
        RetraceLine(newLine.GetComponent<LineRenderer>(), lineCounter);

    }

    void RetraceLine(LineRenderer line, int lineNumber)
    {

        Line currentLine = lineManager.AllLines[lineNumber];

        // Set the width of the Line Renderer
        line.SetWidth(0.01f, 0.01f);
        // Set the number of vertex fo the Line Renderer
        line.positionCount = currentLine.Positions.Length;
        line.material = TrailMaterial;
        line.useWorldSpace = false;
        SetTrailColour(line, currentLine.colour);
        line.SetPositions(currentLine.Positions);
    }

    void SetTrailColour(LineRenderer lineRenderer, Vector3 colour)
    {
        float r = colour.x;
        float g = colour.y;
        float b = colour.z;
        lineRenderer.startColor = new Color(r, g, b);
        lineRenderer.endColor = new Color(r, g, b);
    }


    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }


    private void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        ShowDownloadedPost();
    }


    private void OnTrackingLost()
    {
        // Activate the cloud scanning again
        CloudHandler.mCloudRecoBehaviour.CloudRecoEnabled = true;

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

        foreach (Transform child in PostParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }


}
