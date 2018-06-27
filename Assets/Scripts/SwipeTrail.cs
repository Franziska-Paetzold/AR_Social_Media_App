﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwipeTrail : MonoBehaviour {

    public List<Line> AllLines = new List<Line>();
    public int LineCounter = -1;

    public Material TrailMaterial;
    public bool TouchedAlready = false;
    private TrailRenderer TrailRenderer;
    public bool AllowRetraceLine; // currently set to true in the inspector
    public int RayPositionCounter = 1;
    private bool FirstTouch = true;

    // Use this for initialization
    void Awake () {
        TrailRenderer = GetComponent<TrailRenderer>();
        setTrailColour(Color.green, TrailRenderer);
        // Disabled to get rid of the accidental line in the first frame
        TrailRenderer.enabled = false;
    }
	

	void Update () {
        bool fingerOnScreen = (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0);
        // Check, if the screen is touched and if the finger / mouse is moving
        if (fingerOnScreen)
        {
            if (TouchedAlready == false) TouchedAlready = true;
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if(objPlane.Raycast(mRay, out rayDistance))
            {
                transform.position = mRay.GetPoint(rayDistance);
            }
        }

        if (fingerOnScreen && FirstTouch)
        {
            FirstTouch = false;
            TrailRenderer.enabled = true;
        }

        // Check, if the user has released the screen to store the last drawn line
        if (!fingerOnScreen && TouchedAlready)
        {
            Debug.Log("Lifted");
            StoreRay();
            CreateLineObject();
        }



        }

        void CreateLineObject()
        {
        GameObject newLine = new GameObject();
        newLine.transform.parent = transform;
        newLine.name = "Line segment " + LineCounter;
        newLine.AddComponent<LineRenderer>();
        TouchedAlready = false;
        RetraceLine(newLine.GetComponent<LineRenderer>(), LineCounter);
        
    }

        void StoreRay(){
        int arrayLength = TrailRenderer.positionCount;
        Color currentColour = TrailRenderer.endColor;
        Vector3[] rayPositions = new Vector3[arrayLength];
        GetComponent<TrailRenderer>().GetPositions(rayPositions);

        AllLines.Add(new Line(currentColour, rayPositions));
        LineCounter++;
        
        // Clears the "stage" so you can draw a new line
        TrailRenderer.Clear();
        FirstTouch = true;
        TrailRenderer.enabled = false;

    }


    public void RetraceLine(LineRenderer line, int lineNumber)
    {
        
        Line currentLine = AllLines[lineNumber];

        // Set the width of the Line Renderer
        line.SetWidth(0.1F, 0.1F);
        // Set the number of vertex fo the Line Renderer
        line.positionCount = currentLine.Positions.Length;
        line.material = TrailMaterial;
        setTrailColour(Color.green, null, line);
        line.SetPositions(currentLine.Positions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="tRenderer">Optional. Changes the colour of the passed trail renderer</param>
    /// <param name="lRenderer">Optional. Changes the colour of the passed line renderer</param>
    public void setTrailColour(Color color, TrailRenderer tRenderer = null, LineRenderer lRenderer = null)
    {
        if (tRenderer != null)
        {
            tRenderer.startColor = color;
            tRenderer.endColor = color;
            Debug.Log(tRenderer.endColor.ToString("F5"));
        }
        else if(lRenderer != null)
        {
            lRenderer.startColor = color;
            lRenderer.endColor = color;
        }
    }


  
}
