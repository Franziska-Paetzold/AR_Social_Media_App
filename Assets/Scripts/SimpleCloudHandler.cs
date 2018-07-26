﻿using UnityEngine;
using Vuforia;
public class SimpleCloudHandler : MonoBehaviour, ICloudRecoEventHandler
{
    public ImageTargetBehaviour ImageTargetTemplate;
    public GameObject ObjectToAugment;
    public GameObject CameraOffset;
    public PostReconstructor Reconstructor; // As well TrackableEventHandler

    private ObjectTracker tracker;
    public CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
   
    // Use this for initialization
    void Awake()
    {
        
        // register this event handler at the cloud reco behaviour
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour)
        {
            mCloudRecoBehaviour.RegisterEventHandler(this);
        }
    }


    public void OnInitialized()
    {
        tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            
            tracker.TargetFinder.ClearTrackables(false);
        }
    }
    

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {

        if (targetSearchResult.MetaData == null) return;
        // do something with the target metadata
        mTargetMetadata = targetSearchResult.MetaData;
        if (mTargetMetadata.Contains("Graffiti"))
            Reconstructor.setLineManager(JsonUtility.FromJson<LineManager>(mTargetMetadata));
        else if (mTargetMetadata.Contains("Text"))
            Reconstructor.setTextManager(JsonUtility.FromJson<TextManager>(mTargetMetadata));


            // stop the target finder (i.e. stop scanning the cloud)
            //mCloudRecoBehaviour.CloudRecoEnabled = false;

            ImageTargetBehaviour imageTargetBehaviour = (ImageTargetBehaviour)tracker.TargetFinder.EnableTracking(
             targetSearchResult, ImageTargetTemplate.gameObject);

        // Build augmentation based on target
        if (ImageTargetTemplate != null)
        {
            mCloudRecoBehaviour.CloudRecoEnabled = false;
            // enable the new result with the same ImageTargetBehaviour:

            
        }


    }



}